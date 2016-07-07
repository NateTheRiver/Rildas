using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RildasApp
{
    public partial class XDCCDownloadingForm : MetroFramework.Forms.MetroForm
    {
        private string botName, packNum, downloadPath;
        public XDCCDownloadingForm(string botName, string packNum, string downloadPath)
        {
            InitializeComponent();
            this.botName = botName;
            this.packNum = packNum;
            this.downloadPath = downloadPath;
            this.Text = String.Format("Downloading #{0} from {1}.", packNum, botName);
        }


        private void XDCCService_UpdateProgess(XDCCService.DownloadInfo info)
        {
            _lbState.Invoke(new MethodInvoker(delegate
            {
                switch (info.status)
                {
                    case XDCCService.DownloadInfo.DownloadStatus.COMPLETED: _lbState.Text = "State: Download completed."; break;
                    case XDCCService.DownloadInfo.DownloadStatus.DOWNLOADING: _lbState.Text = "State: Download in progress."; break;
                    case XDCCService.DownloadInfo.DownloadStatus.ERROR_CONNECTING: _lbState.Text = "State: Connection problem occured in downloading. Please try again."; break;
                    case XDCCService.DownloadInfo.DownloadStatus.ERROR_TIMEOUT: _lbState.Text = "State: Connection problem: TIMEOUT. Please check your connection and try again."; break;
                    case XDCCService.DownloadInfo.DownloadStatus.ERROR_UNKNOWN: _lbState.Text = "State: Unknown connection error occured."; break;

                }
            }));
            _lbFileSize.Invoke(new MethodInvoker(delegate
            {
                _lbFileSize.Text = String.Format("FileSize: {0} MB", info.fileSize / (1024 * 1024));
            }));
            _lbFileName.Invoke(new MethodInvoker(delegate
            {
                _lbFileName.Text = "FileName: " + info.fileName;
            }));
            _lbProgress.Invoke(new MethodInvoker(delegate
            {
                _lbProgress.Text = info.Progress + "%";
            }));
            _progressBar.Invoke(new MethodInvoker(delegate
            {
                _progressBar.Value = (int)info.Progress;
            }));
            _lbDownloaded.Invoke(new MethodInvoker(delegate
            {
                _lbDownloaded.Text = String.Format("Downloaded: {0} MB", info.downloadedBytes / (1024 * 1024));
            }));
            _lbSpeed.Invoke(new MethodInvoker(delegate
            {
                if (info.Bytes_Seconds < 1000) _lbSpeed.Text = String.Format("Download speed: {0} B/s", Math.Round(info.Bytes_Seconds));
                else if (info.KBytes_Seconds < 1000) _lbSpeed.Text = String.Format("Download speed: {0} KB/s", Math.Round(info.KBytes_Seconds));
                else _lbSpeed.Text = String.Format("Download speed: {0} MB/s", Math.Round(info.MBytes_Seconds,1));
            }));
            int numberOfETASeconds = (int)((info.fileSize - info.downloadedBytes) / info.Bytes_Seconds);
            _lbETA.Invoke(new MethodInvoker(delegate
            {
                if (numberOfETASeconds / 3600 == 0)
                {
                    _lbETA.Text = String.Format("ETA: {0}m {1}s", numberOfETASeconds / 60, numberOfETASeconds % 60);
                }
                else
                {
                    _lbETA.Text = String.Format("ETA: {0}h {1}m", numberOfETASeconds / 3600, (numberOfETASeconds % 3600) / 60);
                }
            }));

        }
        private void StartConnecting()
        {
            _lbState.Invoke(new MethodInvoker(delegate
            {
                _lbState.Text = "State: Trying to connect";
            }));
            XDCCService.IRCNetworkError += XDCCService_IRCNetworkError;
            XDCCService.UpdateProgess += XDCCService_UpdateProgess;
            XDCCService.Connect(Global.loggedUser.username + "Rildas");
            while (!XDCCService.isConnected) System.Threading.Thread.Sleep(500);
            _lbState.Invoke(new MethodInvoker(delegate
            {
                _lbState.Text = "State: Connected. Preparing to download.";
            }));
            foreach (string channel in Global.GetXDCCChannels())
            {
                XDCCService.client.JoinChannel(channel);
            }
            System.Threading.Thread.Sleep(100);
            if (XDCCService.isProcessing)
            {
                System.Threading.Thread.Sleep(5000);
            }
            if (!XDCCService.GetPackage(botName, packNum, downloadPath))
            {
                _lbState.Invoke(new MethodInvoker(delegate
                {
                    _lbState.Text = "State: XDCC Bot is not responding. Retrying in 5s.";
                }));
                System.Threading.Thread.Sleep(5000);
                if (!XDCCService.GetPackage(botName, packNum, downloadPath))
                {
                    _lbState.Invoke(new MethodInvoker(delegate
                    {
                        _lbState.Text = "State: Failed to contact XDCC Bot. Please try different bot or try again in a while.";
                    }));
                }
                else
                {
                    _lbState.Invoke(new MethodInvoker(delegate
                   {
                       _lbState.Text = "State: Preparation completed. Starting download.";
                   }));
                }
            }
            else
            {
                _lbState.Invoke(new MethodInvoker(delegate
                {
                    _lbState.Text = "State: Preparation completed. Starting download.";
                }));
            }
        }
        private void XDCCDownloadingForm_Shown(object sender, EventArgs e)
        {
            System.Threading.Thread thr = new System.Threading.Thread(StartConnecting);
            thr.Start();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", downloadPath);
        }

        private void XDCCService_IRCNetworkError(string message)
        {
            _lbState.Text = "State: Connection error. Please try again.";
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            XDCCService.AbortDownloader();
            _lbState.Text = "State: Aborted";
        }
    }
}
