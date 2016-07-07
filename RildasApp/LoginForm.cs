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
using KnightsWarriorAutoupdater;

namespace RildasApp
{
    public partial class LoginForm : MetroForm
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer progressBarTimer = new System.Timers.Timer();

        int nextValue = 0;
        public LoginForm()
        {
            Global.Init();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 15000;

            InitializeComponent();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!ConnectionManager.IsConnected) LoginFailed();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["username"] != "") cbSave.Checked = true;
            textUsername.Text = ConfigurationManager.AppSettings["username"];
            ConnectionManager.Recieved += ConnectionManager_Recieved;
            ConnectionManager.Disconnected += ConnectionManager_Disconnected;
        }

        private void ConnectionManager_Disconnected()
        {
            LoginFailed();
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
                case "CLIENT_CONNECTION_READY": Auth(); break;
                case "CLIENT_LOGIN_SUCCESS": LoginSuccess(rest); break;
                case "CLIENT_LOGIN_FAILED": LoginFailed(); break;
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
            nextValue = 85;
            RildasServerAPI.GetAllUsers();
            nextValue = 100;
            System.Threading.Thread.Sleep(50);
            progressBarTimer.Stop();
            ConnectionManager.Recieved -= ConnectionManager_Recieved;
            ConnectionManager.Disconnected -= ConnectionManager_Disconnected;
            LoadDone();
        }

        private void ProgressBarTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            metroProgressBar1.Invoke(new MethodInvoker(delegate {
                if (metroProgressBar1.Value < nextValue) metroProgressBar1.Value++; }));
        }

        public void LoadDone()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Dashboard dash = new Dashboard();
                dash.Show();
                dash.Activate();
                dash.FormClosed += Dash_FormClosed;
            }));
            this.Invoke(new MethodInvoker(delegate { this.Visible = false; }));
        }

        private void Dash_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        public void LoginFailed()
        {
            label_fail.Invoke(new MethodInvoker(delegate { label_fail.Visible = true; }));
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
            timer.Start();
            if (!ConnectionManager.IsConnected)
            {
                ConnectionManager.Connect();
                /*
                Thread thrd = new Thread(ConnectionManager.Connect);
                thrd.Start();*/
            }
            else RildasServerAPI.Login(textUsername.Text, textPassword.Text);
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
