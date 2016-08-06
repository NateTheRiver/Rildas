using RildasApp.Models;
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

namespace RildasApp.Forms
{
    public partial class UpdateNotification : Form
    {
        ApplicationVersion version;
        MetroFramework.Controls.MetroButton loginButton;
        public UpdateNotification(FileVersionInfo fileVersion, ApplicationVersion version, ref MetroFramework.Controls.MetroButton loginButton)
        {
            InitializeComponent();
            this.version = version;
            this.loginButton = loginButton;
            label_update.Text = String.Format("New version found. Your version: {0}. Current version: {1}. You can continue with old version, but there is no guarantee that it will work properly.", fileVersion.FileVersion, version.version);
            label_update.Size = new Size(button_no.Location.X + button_no.Width - button_yes.Location.X, button_yes.Location.Y - label_update.Location.Y);
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                ApplicationUpdateInformationForm updateForm = new ApplicationUpdateInformationForm(version);
                updateForm.Show();
                updateForm.Activate();
            }));
            Properties.Settings.Default.FirstRun = true;
            Properties.Settings.Default.Save();

            Process p = new Process();
            p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RildasAppUpdater.exe");
            p.StartInfo.Arguments = version.downloadLocation;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Verb = "runas";
            p.Start();
            this.Close();
        }

        private void button_no_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FirstRun)
            {
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
                this.Invoke(new MethodInvoker(delegate
                {
                    ApplicationUpdateInformationForm updateForm = new ApplicationUpdateInformationForm(version);
                    updateForm.Show();
                    updateForm.Activate();
                }));


            }
            loginButton.Invoke(new MethodInvoker(delegate { loginButton.Enabled = true; }));
            this.Close();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
