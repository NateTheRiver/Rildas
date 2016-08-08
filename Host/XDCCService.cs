using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSharp;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

namespace Host
{
    class XDCCService
    {
        public struct DownloadInfo
        {
            public enum DownloadStatus { DOWNLOADING, ERROR_CONNECTING, ERROR_TIMEOUT, ERROR_UNKNOWN, COMPLETED }
            public double Bytes_Seconds;
            public double MBytes_Seconds;
            public double KBytes_Seconds;        
            public double Progress;
            public long downloadedBytes;
            public int fileSize;
            public string botname;
            public string packNumber;
            public DownloadStatus status;
        }
        public static IrcClient client;
        private static string nickname;
        private static string newDccString;
        private static string curDownloadDir;
        private static string botName;
        private static string packNum;
        private static Thread downloader;
        private static string newFileName;
        private static int newFileSize;
        private static string newIp;
        private static int newPortNum;
        public static bool isProcessing { get; private set; }
        public static bool isConnected { get; private set; }
        public static bool isDownloading { get; private set; }

        private static string downloadPath;
        private static double Progress;
        private static bool gotResponse;
        static XDCCService()
        {
            isConnected = false;
            isProcessing = false;
            isDownloading = false;
        }
        public static void Connect(string nickname)
        {
            if (isConnected) return;
            XDCCService.nickname = nickname;
            client = new IrcClient("irc.rizon.net", new IrcUser(nickname, nickname));
            client.ConnectAsync();
            client.ConnectionComplete += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connected.");
                Console.ForegroundColor = ConsoleColor.White;
                isConnected = true;
            };
            client.NetworkError += (s, e) =>
            {
                if (IRCNetworkError != null)
                {
                    IRCNetworkError(e.SocketError.ToString());
                }
            };
            client.ChannelMessageRecieved += (s, e) =>
            {
                if (IRCChannelMessage != null) IRCChannelMessage(e.PrivateMessage);
            };
            client.NoticeRecieved += (s, e) =>
            {
                Console.WriteLine("Notice received: " + e.Notice);
            };
            client.RawMessageRecieved += Client_RawMessageRecieved;

        }
        public static bool GetPackage(string bot, string package, string downloadPath)
        {
            if(!isConnected)
            {
                if (DebugMessage != null) DebugMessage("Client is not connected yet.");
                return false;
            }
            if(isProcessing)
            {

                if (DebugMessage != null) DebugMessage("Already downloading. Please wait.");
                return false;
            }
            XDCCService.downloadPath = downloadPath;
            
            System.Threading.Thread.Sleep(100);
            isProcessing = true;
            botName = bot;
            packNum = package;
            gotResponse = false;
            client.SendMessage("xdcc send " + package, bot);
            Thread.Sleep(8000);
            if (!gotResponse)
            {
                Console.WriteLine("Did not get response. :<");
                isProcessing = false;
                return false;
            }
            return true;
        }

        private static void Client_RawMessageRecieved(object sender, ChatSharp.Events.RawMessageEventArgs e)
        {
            //Console.WriteLine("Raw:" + e.Message);
            if (e.Message.Contains("DCC SEND") && e.Message.Contains(nickname))
            {
                gotResponse = true;
                System.Diagnostics.Debug.WriteLine("Got response. Downloading started.");
                startDownloader(e.Message, downloadPath, botName, packNum);
            }
        }
        public static void startDownloader(string dccString, string downloaddir, string bot, string pack)
        {
            if ((dccString ?? downloaddir ?? bot ?? pack) != null && dccString.ToUpper().Contains("SEND") && !isDownloading)
            {
                newDccString = dccString;
                curDownloadDir = downloaddir;
                bool isParsed = parseData();
                if (isParsed)
                {
                    downloader = new Thread(new ThreadStart(Downloader));
                    downloader.Start();
                }
                else
                {
                    client.SendMessage("xdcc remove " + packNum, botName);
                    client.SendMessage("xdcc cancel", botName);
                    isProcessing = false;
                }
            }
            else
            {
                if (isDownloading)
                {
                   // simpleirc.DebugCallBack("You are already downloading! Removing from que\n");
                }
                else
                {
                   // simpleirc.DebugCallBack("DCC String does not contain SEND and/or invalid values for parsing! Removing from que\n");

                }
                client.SendMessage("xdcc remove " + packNum, botName);
                client.SendMessage("xdcc cancel", botName);
                isProcessing = false;
            }

        }

        //parses data received by the dcc strings, necesary for details to connect to the right tcp server where file is located
        private static bool parseData()
        {
            /*
           * :bot PRIVMSG nickname :DCC SEND \"filename\" ip_networkbyteorder port filesize
           *AnimeDispenser!~desktop@Rizon-6AA4F43F.ip-37-187-118.eu PRIVMSG WeebIRCDev :DCC SEND "[LNS] Death Parade - 02 [BD 720p] [7287AE5C].mkv" 633042523 59538 258271780  
           *HelloKitty!~nyaa@ny.aa.ny.aa PRIVMSG WeebIRCDev :DCC SEND [Coalgirls]_Spirited_Away_(1280x692_Blu-ray_FLAC)_[5805EE6B].mkv 3281692293 35567 10393049211
          */
            Regex regex = new Regex(@"^(?=.*(?<filename>(?<=\SEND )(.*?)(?=(\d{9,10}))))(?=.*(?<bitwiseip>((\s)+(\d{9,10}))+(\s)))(?=.*(?<port>((\s)+(\d{4,6})+(\s))))(?=.*(?<size>(\s)+(\d+)(?!.*\d)))");
            Match matches = regex.Match(newDccString.Trim());

            if (matches.Success)
            {
                newFileName = matches.Groups["filename"].Value.Trim();
                char[] invalidFileChars = Path.GetInvalidFileNameChars();

                string pattern = "[\\~#%&*{}/:<>?|\"-]";
                Regex regEx = new Regex(pattern);
                newFileName = Regex.Replace(regEx.Replace(newFileName, ""), @"\s+", " ");
              //  simpleirc.DebugCallBack("New Filename: " + newFileName + "\n");
               // simpleirc.DebugCallBack(" newFileName(without illigal chars): " + newFileName + "\n");


                newFileSize = Convert.ToInt32(matches.Groups["size"].Value.Trim());
                //convert bitwise ip to normal ip
                
                try
                {
                    newFileName = Regex.Replace(regEx.Replace(newFileName, ""), @"\s+", " ");
        //            simpleirc.DebugCallBack("New Filename: " + newFileName + "\n");

      //              simpleirc.DebugCallBack(" newIpBW: " + matches.Groups["bitwiseip"].Value.Trim() + "\n");
                    long newIpBW = Convert.ToInt64(matches.Groups["bitwiseip"].Value.Trim());
                    IPEndPoint hostIPEndPoint = new IPEndPoint(newIpBW, newPortNum);
                    string[] ipadressinfoparts = hostIPEndPoint.ToString().Split(':');
                    string[] ipnumbers = ipadressinfoparts[0].Split('.');
                    string ip = ipnumbers[3] + "." + ipnumbers[2] + "." + ipnumbers[1] + "." + ipnumbers[0];
                    newIp = ip;
                }
                catch (Exception e)
                {
                 //   simpleirc.DebugCallBack("GOT AN ERROR TRYING PARSE IP: " + e.ToString() + "\n");
                }


                newPortNum = Convert.ToInt32(matches.Groups["port"].Value.Trim());
                return true;
            }
            else
            {
             //   simpleirc.DebugCallBack("Regex Failed at parsing XDCC Command :X \n");
                return false;
            }

        }

        //creates a tcp socket connection for the retrieved ip/port from the dcc tcp by the dcc bot/server
        //and creates a file, to where it writes the incomming data from the tcp connection.
        public static void Downloader()
        {
            if (!Directory.Exists(curDownloadDir))
            {
                Directory.CreateDirectory(curDownloadDir);
            }
            string[] pathToCombine = new string[] { curDownloadDir, newFileName };
            string dlDirAndFileName = Path.Combine(pathToCombine);
            string finalFileName = dlDirAndFileName;
            for (int i = 1; i < 10000; i++)
            {
                if (File.Exists(finalFileName))
                {
                    finalFileName = Path.Combine(curDownloadDir, String.Format("{0}({1}){2}", Path.GetFileNameWithoutExtension(dlDirAndFileName), i, Path.GetExtension(dlDirAndFileName)));
                }
                else break;
            }
            try
            {

                //start connection with tcp server
                using (TcpClient dltcp = new TcpClient(newIp, newPortNum))
                {
                    using (NetworkStream dlstream = dltcp.GetStream())
                    {
                        //succesfully connected to tcp server, status is downloading

                        //values to keep track of progress
                        long bytesReceived = 0;
                        long oldBytesReceived = 0;
                        long oneprocent = newFileSize / 100;
                        DateTime start = DateTime.Now;
                        bool timedOut = false;

                        //values to keep track of download position
                        int count;

                        //efficient buffer select
                        byte[] buffer;
                        if (newFileSize > 1048576)
                        {
                            System.Diagnostics.Debug.WriteLine("DCC Downloader: Big file, big buffer (1 mb) \n ");
                            buffer = new byte[1048576];
                        }
                        else if (newFileSize < 1048576 && newFileSize > 2048)
                        {
                            System.Diagnostics.Debug.WriteLine("DCC Downloader: Smaller file (< 1 mb), smaller buffer (2 kb) \n ");
                            buffer = new byte[2048];
                        }
                        else if (newFileSize < 2048 && newFileSize > 128)
                        {
                            System.Diagnostics.Debug.WriteLine("DCC Downloader: Small file (< 2kb mb), small buffer (128 b) \n ");
                            buffer = new byte[128];
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("DCC Downloader: Tiny file (< 128 b), tiny buffer (2 b) \n ");
                            buffer = new byte[2];
                        }


                        //create file to write to
                        using (FileStream writeStream = new FileStream(finalFileName, FileMode.Append, FileAccess.Write, FileShare.Read))
                        {
                            isDownloading = true;
                            //download while connected and filesize is not reached
                            while (dltcp.Connected && bytesReceived < newFileSize)
                            {
                                //count bytes received
                                count = dlstream.Read(buffer, 0, buffer.Length);

                                //write to file
                                writeStream.Write(buffer, 0, count);

                                //count bytes received
                                bytesReceived += count;

                                Progress = (int)(bytesReceived / oneprocent);


                                //keep track of progress
                                DateTime end = DateTime.Now;
                                if (start.Second != end.Second)
                                {
                                    DownloadInfo info = new DownloadInfo();
                                    info.botname = botName;
                                    info.Bytes_Seconds = bytesReceived - oldBytesReceived;
                                    info.downloadedBytes = bytesReceived;
                                    info.fileSize = newFileSize;
                                    info.KBytes_Seconds = (int)(info.Bytes_Seconds / 1024);
                                    info.MBytes_Seconds = (info.KBytes_Seconds / 1024);
                                    info.packNumber = packNum;
                                    info.Progress = Progress;
                                    info.status = DownloadInfo.DownloadStatus.DOWNLOADING;
                                    if(UpdateProgess != null) UpdateProgess(info);
                                    start = DateTime.Now;
                                }


                                //check if data is still available, to avoid stalling of download thread
                                int timeOut = 0;
                                while (!dlstream.DataAvailable)
                                {
                                    if (timeOut == 1000)
                                    {
                                        break;
                                    }
                                    else if (!dltcp.Connected)
                                    {
                                        break;
                                    }
                                    timeOut++;
                                    Thread.Sleep(1);
                                }

                                //stop download thread if timeout is reached
                                if (timeOut == 1000)
                                {
                                    timedOut = true;
                                    break;
                                }
                            }

                            //close all connections and streams (just to be save)
                            dltcp.Close();
                            dlstream.Dispose();
                            writeStream.Close();

                            //consider 95% downloaded as done, files are sequentually downloaded, sometimes download stops early, but the file still is usable
                            if (Progress < 95)
                            {
                                if(DebugMessage != null) DebugMessage("Download stopped at < 95 % finished, deleting file: " + newFileName + " \n");
                                File.Delete(finalFileName);
                                timedOut = false;
                                DownloadInfo info = new DownloadInfo();
                                info.botname = botName;
                                info.downloadedBytes = bytesReceived;
                                info.fileSize = newFileSize;
                                info.packNumber = packNum;
                                info.Progress = Progress;
                                info.status = DownloadInfo.DownloadStatus.ERROR_UNKNOWN;
                                if (UpdateProgess != null) UpdateProgess(info);

                            }
                            else if (timedOut && Progress < 95)
                            {
                                DownloadInfo info = new DownloadInfo();
                                info.botname = botName;
                                info.downloadedBytes = bytesReceived;
                                info.fileSize = newFileSize;
                                info.packNumber = packNum;
                                info.Progress = Progress;
                                info.status = DownloadInfo.DownloadStatus.ERROR_TIMEOUT;
                                if (UpdateProgess != null) UpdateProgess(info);
                                if (DebugMessage != null) DebugMessage("Download timed out at < 95 % finished, deleting file: " + newFileName + " \n");
                                File.Delete(finalFileName);
                                timedOut = false;
                            }
                            else
                            {
                                DownloadInfo info = new DownloadInfo();
                                info.botname = botName;
                                info.downloadedBytes = bytesReceived;
                                info.fileSize = newFileSize;
                                info.packNumber = packNum;
                                info.Progress = Progress;
                                info.status = DownloadInfo.DownloadStatus.COMPLETED;
                                if (UpdateProgess != null) UpdateProgess(info);
                                if (DownloadCompleted != null) DownloadCompleted(finalFileName);
                            }
                        }

                    }
                }
            }

            catch (SocketException e)
            {
                System.Diagnostics.Debug.WriteLine("Something went wrong while downloading the file! Removing from xdcc que to be sure! Error:\n" + e.ToString());
                client.SendMessage("xdcc remove " + packNum, botName);
                client.SendMessage("xdcc cancel", botName);
                DownloadInfo info = new DownloadInfo();
                info.botname = botName;
                info.fileSize = newFileSize;
                info.packNumber = packNum;
                info.Progress = Progress;
                info.status = DownloadInfo.DownloadStatus.ERROR_CONNECTING;
                isProcessing = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Something went wrong while downloading the file! Removing from xdcc que to be sure! Error:\n" + ex.ToString());
                client.SendMessage("xdcc remove " + packNum, botName);
                client.SendMessage("xdcc cancel", botName);
                DownloadInfo info = new DownloadInfo();
                info.botname = botName;
                info.fileSize = newFileSize;
                info.packNumber = packNum;
                info.Progress = Progress;
                info.status = DownloadInfo.DownloadStatus.ERROR_UNKNOWN;

            }
            isDownloading = false;
            isProcessing = false;
        }
        //stops the downloader
        public void abortDownloader()
        {
         //   simpleirc.DebugCallBack("Downloader Stopped");
            isDownloading = false;
            isProcessing = false;
            downloader.Abort();
        }
        public delegate void DownloadCompletedHandler(string filePath);
        public static event DownloadCompletedHandler DownloadCompleted;

        public delegate void DownloadProgressUpdate(DownloadInfo info);
        public static event DownloadProgressUpdate UpdateProgess;

        public delegate void DebugMessageHandler(string message);
        public static event DebugMessageHandler DebugMessage;

        public delegate void IRCChannelMessageHandler(PrivateMessage message);
        public static event IRCChannelMessageHandler IRCChannelMessage;

        public delegate void IRCConnectionNetworkError(string message);
        public static event IRCConnectionNetworkError IRCNetworkError;
    }
    
}
