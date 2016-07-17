using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Threading;
using System.Configuration;
using RildasApp.Models;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace RildasApp.Forms
{
    public partial class LoginForm : MetroForm
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer progressBarTimer = new System.Timers.Timer();
        private const string CONNECTION_FAILED = "Připojení selhalo, aplikace se pokusí spojení obnovit.";
        private const string LOGIN_FAILED = "Chybné uživatelské jméno nebo heslo, zkuste to prosím znovu.";
        int nextValue = 0;
        public LoginForm()
        {
            InitializeComponent();
            Global.Init();
            ConnectionManager.Recieved += ConnectionManager_Recieved;
            ConnectionManager.Disconnected += ConnectionManager_Disconnected;
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 10000;
            if(!ConnectionManager.Connect())
            {
                label_fail.Text = CONNECTION_FAILED;
                label_fail.Visible = true;
            }
            else
            {

            }
            timer.Start();
            
            
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!ConnectionManager.Connect())
            {
                label_fail.Invoke(new MethodInvoker(delegate { 
                label_fail.Text = CONNECTION_FAILED;
                label_fail.Visible = true;
                }));
                metroButton1.Invoke(new MethodInvoker(delegate {
                    metroButton1.Enabled = false;
                }));
            }
            else
            {
                label_fail.Invoke(new MethodInvoker(delegate {
                    if (label_fail.Text == CONNECTION_FAILED) label_fail.Visible = false;
                }));
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["username"] != "") cbSave.Checked = true;
            textUsername.Text = ConfigurationManager.AppSettings["username"];

        }

        private void ConnectionManager_Disconnected()
        {
            label_fail.Invoke(new MethodInvoker(delegate {
                label_fail.Text = CONNECTION_FAILED;
                label_fail.Visible = true;
            }));
        }

        private void ConnectionManager_Recieved(string data)
        {

            string[] split = data.Split('_');
            string determinator = String.Join("_", split[0], split[1], split[2]);
            string rest;
            if (determinator.Length == data.Length) rest = "";
            else rest = data.Substring(determinator.Length + 1, data.Length - determinator.Length - 1);

            switch (determinator)
            {
                case "CLIENT_CONNECTION_READY": CheckVersion(rest); break;
                case "CLIENT_LOGIN_SUCCESS": LoginSuccess(rest); break;
                case "CLIENT_LOGIN_FAILED": LoginFailed(); break;
            }

        }

        private void CheckVersion(string rest)
        {
#if DEBUG
            metroButton1.Invoke(new MethodInvoker(delegate { metroButton1.Enabled = true; }));
            return;
#endif
            ApplicationVersion version = Serializer.Deserialize<ApplicationVersion>(rest);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            if (fvi.FileVersion != version.version)
            {
                DialogResult result = MetroFramework.MetroMessageBox.Show(new Form { TopMost = true, StartPosition = FormStartPosition.CenterParent }, String.Format("New version found. Your version: {0}. Current version: {1}. You can continue with old version, but there is no guarantee that it will work properly.", fvi.FileVersion, version.version),  "New version", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
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
                   

                }
                else
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
                    metroButton1.Invoke(new MethodInvoker(delegate { metroButton1.Enabled = true; }));
                }
            }
            else
            {
                metroButton1.Invoke(new MethodInvoker(delegate { metroButton1.Enabled = true; }));
            }
        }

        public void LoginSuccess(string userdata)
        {
            User user = Serializer.Deserialize<User>(userdata);
            if (user == null)
            {
                label_fail.Invoke(new MethodInvoker(delegate { label_fail.Visible = true; }));
                metroButton1.Invoke(new MethodInvoker(delegate { metroButton1.Visible = true; }));
                loginSpinner.Invoke(new MethodInvoker(delegate { loginSpinner.Visible = false; }));
            }
            else
            {

                Global.loggedUser = user;
                if (cbSave.Checked) setSetting("username", textUsername.Text);
                else setSetting("username", "");
                // todo: loading
                Thread thrd = new Thread(LoadAllComponents);
                thrd.Start();

            }
        }
        public void LoadAllComponents()
        {

            progressBarTimer.Interval = 30;
            progressBarTimer.Elapsed += ProgressBarTimer_Elapsed;
            progressBarTimer.Start();
            metroProgressBar1.Invoke(new MethodInvoker(delegate { metroProgressBar1.Visible = true; }));
            nextValue = 15;
            RildasServerAPI.GetAllAnimes();
            nextValue = 35;
            RildasServerAPI.GetAllEpisodes();
            nextValue = 55;
            RildasServerAPI.GetAllEpisodeVersions();
            nextValue = 70;
            RildasServerAPI.GetAllXDCCVersions();
            nextValue = 80;
            RildasServerAPI.GetAllChatGroups();
            nextValue = 85;
            RildasServerAPI.GetTeamMembers();
            nextValue = 100;
            RildasServerAPI.GetLoggedUsers();
            System.Threading.Thread.Sleep(50);
            progressBarTimer.Stop();
            ConnectionManager.Recieved -= ConnectionManager_Recieved;
            ConnectionManager.Disconnected -= ConnectionManager_Disconnected;
            LoadDone();
        }

        private void ProgressBarTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                metroProgressBar1.Invoke(new MethodInvoker(delegate
                {
                    if (metroProgressBar1.Value < nextValue) metroProgressBar1.Value++;
                }));
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadDone()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Dashboard dash = new Dashboard();
                dash.Show();
                dash.Activate();
                dash.FormClosed += Dash_FormClosed;
                this.Visible = false;
            }));
        }

        private void Dash_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        public void LoginFailed()
        {
            label_fail.Invoke(new MethodInvoker(delegate { label_fail.Text = LOGIN_FAILED; label_fail.Visible = true; }));
            metroButton1.Invoke(new MethodInvoker(delegate { metroButton1.Visible = true; }));
            loginSpinner.Invoke(new MethodInvoker(delegate { loginSpinner.Visible = false; }));
        }
        public bool setSetting(string pstrKey, string pstrValue)
        {
            Configuration objConfigFile =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool blnKeyExists = false;

            foreach (string strKey in objConfigFile.AppSettings.Settings.AllKeys)
            {
                if (strKey == pstrKey)
                {
                    blnKeyExists = true;
                    objConfigFile.AppSettings.Settings[pstrKey].Value = pstrValue;
                    break;
                }
            }
            if (!blnKeyExists)
            {
                objConfigFile.AppSettings.Settings.Add(pstrKey, pstrValue);
            }
            objConfigFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            return true;
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            
            label_fail.Visible = false;
            metroButton1.Visible = false;
            loginSpinner.Visible = true;
            Thread thrd = new Thread(Auth);
            thrd.Start();
            /*Dashboard dash = new Dashboard();
            dash.Show();
            dash.Activate();
            this.Visible = false;*/
        }
        private void Auth()
        {
            if (!ConnectionManager.IsConnected)
            {
                ConnectionManager.Connect();
                /*
                Thread thrd = new Thread(ConnectionManager.Connect);
                thrd.Start();*/
            }
            else
            {
                RildasServerAPI.Login(textUsername.Text, textPassword.Text);
                Global.password = textPassword.Text;
            }
        }
        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (metroButton1.Visible == false) return;
            if (e.KeyCode == Keys.Enter)
            {
                label_fail.Visible = false;
                metroButton1.Visible = false;
                loginSpinner.Visible = true;
                Thread thrd = new Thread(Auth);
                thrd.Start();
                e.Handled = e.SuppressKeyPress = true;
            }
        }
    }
}
