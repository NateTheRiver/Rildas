using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Forms;
namespace RildasApp.VLC
{
    public partial class RildasPlayer : Form
    {
        public RildasPlayer()
        {
            InitializeComponent();
        }

        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x86\"));
            else
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"..\..\..\lib\x64\"));

            if (!e.VlcLibDirectory.Exists)
            {
                var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                folderBrowserDialog.Description = "Select Vlc libraries folder.";
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderBrowserDialog.ShowNewFolderButton = true;
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                }
            }
        }

        private void OnButtonPlayClicked(object sender, EventArgs e)
        {
            myVlcControl.Play(new Uri(@"C:\Users\Richard\Desktop\[GJM]_Tanakakun_wa_Kyou_mo_Kedaruge__35_[8B852707].mkv"));
        }

        private void OnButtonStopClicked(object sender, EventArgs e)
        {
            myVlcControl.Stop();
        }

        private void OnButtonPauseClicked(object sender, EventArgs e)
        {
            myVlcControl.Pause();
        }

        private void OnVlcMediaLengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            myLblMediaLength.Invoke(new MethodInvoker(delegate
            {
                myLblMediaLength.Text = new DateTime(new TimeSpan((long) e.NewLength).Ticks).ToString("T");
            }));
        }

        private void OnVlcPositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            var position = myVlcControl.GetCurrentMedia().Duration.Ticks * e.NewPosition;

            myLblVlcPosition.Invoke(new MethodInvoker(delegate
            {
                myLblVlcPosition.Text = new DateTime((long) position).ToString("T");
            }));


        }

        private void OnVlcPaused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            myLblState.Invoke(new MethodInvoker(delegate {
                  myLblState.Text = "Paused";
            }));
        }

        private void OnVlcStopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
          //  myLblState.InvokeIfRequired(l => l.Text = "Stopped");

            myCbxAspectRatio.InvokeIfRequired(c =>
            {
                c.Text = string.Empty;
                c.Enabled = false;
            });
            myLblAudioCodec.Text = "Codec: ";
            myLblAudioChannels.Text = "Channels: ";
            myLblAudioRate.Text = "Rate: ";
            myLblVideoCodec.Text = "Codec: ";
            myLblVideoHeight.Text = "Height: ";
            myLblVideoWidth.Text = "Width: ";



        }
       
        private void OnVlcPlaying(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            myLblState.InvokeIfRequired(l => l.Text = "Playing");

            myLblAudioCodec.InvokeIfRequired(l => l.Text = "Codec: ");
            myLblAudioChannels.InvokeIfRequired(l => l.Text = "Channels: ");
            myLblAudioRate.InvokeIfRequired(l => l.Text = "Rate: ");
            myLblVideoCodec.InvokeIfRequired(l => l.Text = "Codec: ");
            myLblVideoHeight.InvokeIfRequired(l => l.Text = "Height: ");
            myLblVideoWidth.InvokeIfRequired(l => l.Text = "Width: ");

            var mediaInformations = myVlcControl.GetCurrentMedia().TracksInformations;
            foreach (var mediaInformation in mediaInformations)
            {
                if (mediaInformation.Type == Vlc.DotNet.Core.Interops.Signatures.MediaTrackTypes.Audio)
                {
                    myLblAudioCodec.InvokeIfRequired(l => l.Text += mediaInformation.CodecName);
                    myLblAudioChannels.InvokeIfRequired(l => l.Text += mediaInformation.Audio.Channels);
                    myLblAudioRate.InvokeIfRequired(l => l.Text += mediaInformation.Audio.Rate);
                }
                else if (mediaInformation.Type == Vlc.DotNet.Core.Interops.Signatures.MediaTrackTypes.Video)
                {
                    myLblVideoCodec.InvokeIfRequired(l => l.Text += mediaInformation.CodecName);
                    myLblVideoHeight.InvokeIfRequired(l => l.Text += mediaInformation.Video.Height);
                    myLblVideoWidth.InvokeIfRequired(l => l.Text += mediaInformation.Video.Width);
                }
            }

            myCbxAspectRatio.InvokeIfRequired(c =>
            {
                c.Text = myVlcControl.Video.AspectRatio;
                c.Enabled = true;
            });
        }

        private void myCbxAspectRatio_TextChanged(object sender, EventArgs e)
        {
            myVlcControl.Video.AspectRatio = myCbxAspectRatio.Text;
            ResizeVlcControl();
        }

        private void Sample_SizeChanged(object sender, EventArgs e)
        {
            ResizeVlcControl();
        }

        void ResizeVlcControl()
        {
            if (!string.IsNullOrEmpty(myCbxAspectRatio.Text))
            {
                var ratio = myCbxAspectRatio.Text.Split(':');
                int width, height;
                if (ratio.Length == 2 && int.TryParse(ratio[0], out width) && int.TryParse(ratio[1], out height))
                    myVlcControl.Width = myVlcControl.Height * width / height;
            }
        }

    }
}
