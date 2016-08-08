using Host.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    public static class AutoXDCCUpdater
    {
        public static DateTime lastUpdated = new DateTime(1970,1,1);
        public static List<XDCCPackageDetails> listOfDetails = new List<XDCCPackageDetails>();
        public static List<string> botlist = new List<string>() { "Gintoki", "HelloKitty", "[xdcc]Ecchilicious", "NIBL|Arutha" };
        public static List<string> channelsToJoin = new List<string>() { "#intel", "#Ecchilicious", "#NIBL" };
        private static List<XDCCPackageDetails> tempList = new List<XDCCPackageDetails>();
        public static void DownloadAndParse()
        {
            try
            {
                tempList.Clear();
#if DEBUG
                XDCCService.Connect("RildasServiceDebugMode");
#else

                XDCCService.Connect("RildasService");
#endif
                while (!XDCCService.isConnected) System.Threading.Thread.Sleep(500);
                foreach (string channel in channelsToJoin)
                {
                    XDCCService.client.JoinChannel(channel);
                }
                System.Threading.Thread.Sleep(15000);
                XDCCService.DownloadCompleted += XDCCService_DownloadCompleted;
                foreach (string bot in botlist)
                {
                    while (true)
                    {
                        if (XDCCService.isProcessing)
                        {
                            System.Threading.Thread.Sleep(5000);
                            continue;
                        }
                        XDCCService.GetPackage(bot, "-1", Path.Combine(Path.GetTempPath(), "RildasXDCCFiles"));
                        break;
                    }
                }
                while (XDCCService.isProcessing) System.Threading.Thread.Sleep(1000);


            }
            catch (Exception e)
            {
                Logger.Log("Error in XDCC parsing: " + e.Message);
            }


            listOfDetails = new List<XDCCPackageDetails>(tempList);

            //System.Threading.Thread.Sleep(3 * 60 * 60 * 1000);
            XDCCService.IRCChannelMessage += XDCCService_IRCChannelMessage;

        }

        private static void XDCCService_IRCChannelMessage(ChatSharp.PrivateMessage message)
        {
            try
            {
                if (!message.Source.StartsWith("#")) return;
                XDCCPackageDetails details = null;
                switch (message.User.Nick)
                {
                    case "Ginpachi-Sensei":
                        {
                            if (message.Source != "#NIBL") return;
                            details = new XDCCPackageDetails();
                            string[] splitBySlash = message.Message.Split('/');
                            details.botName = splitBySlash[splitBySlash.Length - 1].Split(' ')[1];
                            details.packageNum = splitBySlash[splitBySlash.Length - 1].Split(' ')[4];
                            details.packageNum = details.packageNum.Substring(0, details.packageNum.Length - 1);
                            details.packageSize = message.Message.Split('-')[1].Length > 9 ? "" : message.Message.Split('-')[1];
                            details.filename = details.packageSize == "" ? String.Join("-", message.Message.Split('-').Reverse().Skip(1 + details.botName.Count(x => x == '-')).Reverse().Skip(1)) :
                                                                           String.Join("-", message.Message.Split('-').Reverse().Skip(1 + details.botName.Count(x => x == '-')).Reverse().Skip(2));
                            details.filename = details.filename.Substring(4);
                            details.filename = details.filename.Substring(0, details.filename.Length - 2);

                        }; break;
                    case "MyMelody":
                        {
                            if (message.Source != "#NIBL") return;
                            details = new XDCCPackageDetails();
                            string[] splitBySpace = message.Message.Split(' ');
                            details.botName = "HelloKitty";
                            details.packageNum = splitBySpace[splitBySpace.Length - 1];
                            details.packageSize = splitBySpace[1];
                            details.filename = String.Join(" ", splitBySpace.Skip(2).Reverse().Skip(5).Reverse());
                             ConnectionManager.SendToAll("DATA_IRCXDCCDATA_ADDVERSION_" + Serializer.Serialize(details));

                        }
                        break;
                    case "NIBL|Arutha":
                        {
                            if (message.Source != "#NIBL") return;
                            details = new XDCCPackageDetails();
                            string[] splitByStar = message.Message.Split('*');
                            details.botName = "NIBL|Arutha";
                            details.packageNum = message.Message.Split(' ')[message.Message.Split(' ').Length - 1];
                            details.packageNum = details.packageNum.Substring(0, details.packageNum.Length - 2);
                            details.packageSize = splitByStar[1];
                            details.filename = String.Join("*", splitByStar.Skip(2).Reverse().Skip(1).Reverse());
                            details.filename = details.filename.Substring(1);
                            // ConnectionManager.SendToAll("CHANGEDATA_ADD_IRCXDCCPACKAGE_" + Serializer.Serialize(details));

                        }
                        break;
                }
                int n;
                if (details == null || !int.TryParse(details.packageNum, out n)) return;
                details.addedToList = DateTime.Now;
                Logger.Log(String.Format("Adding file {0}. Bot: {1}, Size: {2}, PackageNum: {3}", details.filename, details.botName, details.packageSize, details.packageNum), Logger.SEVERITY.INFO);

                details.fansubGroup = "(unknown)";
                if (details.filename.Trim(' ').StartsWith("["))
                {
                    details.fansubGroup = details.filename.Substring(details.filename.IndexOf('['), details.filename.IndexOf(']') - details.filename.IndexOf('[') + 1);
                }
                details.quality = "(unknown)";
                if (details.filename.Contains("360p")) details.quality = "360p";
                if (details.filename.Contains("480p")) details.quality = "480p";
                if (details.filename.Contains("640x480")) details.quality = "480p";
                if (details.filename.Contains("720p")) details.quality = "720p";
                if (details.filename.Contains("1280x720")) details.quality = "720p";
                if (details.filename.Contains("1080p")) details.quality = "1080p";
                if (details.filename.Contains("1920x1080")) details.quality = "1080p";
                listOfDetails.Add(details);
            }
            catch(Exception e)
            {
                Logger.Log(e.Message);
            }
        }

        public static List<XDCCPackageDetails> GetPackageDetails()
        {
            return listOfDetails;
        }
        private static void XDCCService_DownloadCompleted(string filePath)
        {
            string line;
            string botname = "";
            StreamReader file = new StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                if(line.Contains("XDCC SEND x"))
                {
                    botname = line.Split('/')[1].Split(' ')[1];
                }
                if(line.StartsWith("#"))
                {
                    if (botname == "")
                    {
                        Logger.Log(String.Format("Failed to parse file: {0}. Botname was not set.", filePath));
                        return;
                    }
                    XDCCPackageDetails details = new XDCCPackageDetails();
                    line = System.Text.RegularExpressions.Regex.Replace(line, @"\s+", " ");
                    line = line.Replace("[ ", "[");
                    string[] split = line.Split(' ');
                    details.packageNum = split[0].Substring(1);
                    details.packageSize = split[2];
                    details.botName = botname;
                    details.filename = String.Join(" ", split.Skip(3));
                    details.addedToList = DateTime.Now;
                    details.fansubGroup = "(unknown)";
                    if (details.filename.Trim(' ').StartsWith("["))
                    {
                        details.fansubGroup = details.filename.Substring(details.filename.IndexOf('['), details.filename.IndexOf(']') - details.filename.IndexOf('[') + 1);
                    }
                    details.quality = "(unknown)";
  
                    if (details.filename.Contains("360p")) details.quality = "360p";
                    if (details.filename.Contains("480p")) details.quality = "480p";
                    if (details.filename.Contains("640x480")) details.quality = "480p";
                    if (details.filename.Contains("720p")) details.quality = "720p";
                    if (details.filename.Contains("1280x720")) details.quality = "720p";
                    if (details.filename.Contains("1080p")) details.quality = "1080p";
                    if (details.filename.Contains("1920x1080")) details.quality = "1080p";
                    tempList.Add(details);
                }
            }
            file.Close();
        }
    }
}
