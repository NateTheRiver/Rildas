using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using RildasApp.Models;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using MetroFramework;
using Microsoft.Win32;
using RildasApp.Properties;

namespace RildasApp.Forms
{
    public partial class ChatWindowGroup : MetroFramework.Forms.MetroForm
    {
        public struct GroupMessage
        {
            public string username;
            public string text;
            public DateTime time;
        }
        private readonly ChatGroup _chatGroup;
        private static List<GroupMessage> loggedMessages = new List<GroupMessage>();
            // To support flashing.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;
        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;
        readonly List<Keys> _pressed;
        protected override bool ShowWithoutActivation => true;

        public ChatWindowGroup(ChatGroup chatGroup)
        {
            _chatGroup = chatGroup;
            _pressed = new List<Keys>();
            InitializeComponent();
            richTextBox1.Location = new Point(1, 1);
            Global.OnlineUsersListUpdated += LoadLoggedState;
            Global.UserConnected += Global_UserConnected;
            this.FormClosing += ChatWindowGroup_FormClosing;

            Global.UserDisconnected += Global_UserDisconnected;
            if (loggedMessages.Any())
            {
                loggedMessages = loggedMessages.Skip(Math.Max(0, loggedMessages.Count - 50)).ToList();
                foreach (var message in loggedMessages)
                {
                    AppendMessage(message.username, message.text, message.time, true);
                }
            }
            else
            {
                var groupMessages = Global.GetLoggedGroupMessages(chatGroup.id);
                groupMessages = groupMessages.Skip(Math.Max(0, groupMessages.Count() - 50)).ToList();
                foreach (var message in groupMessages)
                {
                    AppendMessage(Global.GetUser(message.senderId).username, message.text, message.time);
                }
            }
        }

        private void ChatWindowGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Global.OnlineUsersListUpdated -= LoadLoggedState;
            Global.UserConnected -= Global_UserConnected;
        }

        private void Global_UserDisconnected(User user)
        {
            Append(richTextBox1, user.username + " se na to vykašlal a prostě to vypnul.", Color.White);
            Append(richTextBox1, Environment.NewLine, Color.White);
            this.Invoke(new MethodInvoker(delegate
            {
                richTextBox1.ScrollToCaret();
                ((PictureBox) usersPanel.Controls.Find(user.username + "_state", false)[0]).Image = Resources.red;
            }));
        }

        private void Global_UserConnected(User user)
        {
            Append(richTextBox1, user.username + " se rozhodl naší skupinu obdařit svou božskou přítomností.", Color.White);
            Append(richTextBox1, Environment.NewLine, Color.White);
            this.Invoke(new MethodInvoker(delegate
            {
                richTextBox1.ScrollToCaret();
                ((PictureBox) usersPanel.Controls.Find(user.username + "_state", false)[0]).Image = Resources.green;
            }));

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        // Do the flashing - this does not involve a raincoat.
        public bool FlashWindowEx()
        {
            try
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                this.Invoke(new MethodInvoker(delegate
                {
                    IntPtr hWnd = this.Handle;
                    fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                    fInfo.hwnd = hWnd;
                    fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
                    fInfo.uCount = UInt32.MaxValue;
                    fInfo.dwTimeout = 0;


                }));
                return FlashWindowEx(ref fInfo);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void ChatWindow_Resize(object sender, EventArgs e)
        {
            const int padding = 24;
            const int topPadding = 63;
            const int textboxPadding = 10;
            const int buttonPading = 5;
            const int panelPadding = 20;

            usersPanel.Location = new Point(this.Width - (padding + usersPanel.Width), topPadding);
            metroLabel13.Location = new Point(usersPanel.Location.X, metroLabel13.Location.Y);
            tbMessage.Location = new Point(padding, this.Height - (padding + tbMessage.Height));
            tbMessage.Size = new Size(usersPanel.Location.X - (tbMessage.Location.X + buttonPading + btnSend.Width + panelPadding), tbMessage.Height);
            btnSend.Location = new Point(tbMessage.Location.X + tbMessage.Width + buttonPading, tbMessage.Location.Y);
            richTextBox1.Location = new Point(padding, topPadding);
            richTextBox1.Size = new Size(this.Width - (padding + (this.Width - usersPanel.Location.X) + panelPadding), tbMessage.Location.Y - (richTextBox1.Location.Y + textboxPadding));
            cbAlwaysOnTop.Location = new Point(richTextBox1.Location.X + richTextBox1.Width - cbAlwaysOnTop.Width, cbAlwaysOnTop.Location.Y);
            // TODO: Resize všech komponent
        }
        public void AppendMessage(string username, string message, DateTime time, bool doNotLog = false)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                var md5 = MD5.Create();
                var value = username;
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                var color = Color.FromArgb(hash[0], hash[1], hash[2]);
                if (username == Global.loggedUser.username) color = Color.FromArgb(60, 130, 231);
                Append(richTextBox1, String.Format("[{0}]", time.ToString("HH:mm")), color);
                Append(richTextBox1, username, color);
                Append(richTextBox1, ": " + message, Color.White);
                Append(richTextBox1, Environment.NewLine, Color.White);
                richTextBox1.ScrollToCaret();
            }));

            if (doNotLog) return;
            try
            {
                string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RildasLogs");
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                File.AppendAllText(Path.Combine(dirPath, "group_" + _chatGroup.name + ".txt"), String.Format("[{0}]{1}: {2}", time.ToString("HH:mm"), username, message) + Environment.NewLine);
                loggedMessages.Add(new GroupMessage() { username = username, text = message, time = time });
            }
            catch (Exception)
            {
            }
            
        }
        private static void Append(RichTextBox box, string text, Color color)
        {
            box.Invoke(new MethodInvoker(delegate
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;

                box.SelectionColor = color;
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
            }));
        }
        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = cbAlwaysOnTop.Checked;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (tbMessage.Text == "") return;
            RildasServerAPI.SendGroupMessage(_chatGroup.id, tbMessage.Text);
            tbMessage.Text = "";
            this.tbMessage.Focus();
        }


        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (_pressed.Exists(x => x == Keys.ControlKey))
            {
                if (e.KeyCode == Keys.A)
                {
                    tbMessage.SelectAll();
                    e.Handled = e.SuppressKeyPress = true;
                    return;
                }
                if (e.KeyCode == Keys.Back)
                {
                    if (tbMessage.SelectionStart != 0 && tbMessage.SelectionLength == 0)
                    {
                        int selection = tbMessage.SelectionStart;
                        selection--;
                        if (tbMessage.Text[selection] == ' ')
                        {
                            selection--;
                        }
                        while (selection > 0 && tbMessage.Text[selection - 1] != ' ')
                        {
                            selection--;
                        }
                        int len = tbMessage.SelectionStart - selection;
                        tbMessage.Text = tbMessage.Text.Remove(selection, len);
                        tbMessage.SelectionStart = tbMessage.Text.Length;
                    }
                }
                if (e.KeyCode == Keys.Delete)
                {
                    if (tbMessage.SelectionStart != tbMessage.Text.Length && tbMessage.SelectionLength == 0)
                    {
                        int selection = tbMessage.SelectionStart;
                        if (tbMessage.Text[selection] == ' ')
                        {
                            while (selection < (tbMessage.Text.Length - 1) && tbMessage.Text[selection + 1] == ' ')
                            {
                                selection++;
                            }
                        }
                        else
                        {
                            while (selection < (tbMessage.Text.Length - 1) && tbMessage.Text[selection] != ' ')
                            {
                                selection++;
                            }
                        }
                        int len = selection - tbMessage.SelectionStart + 1;
                        int tmp = tbMessage.SelectionStart;
                        tbMessage.Text = tbMessage.Text.Remove(tbMessage.SelectionStart, len);
                        tbMessage.SelectionStart = tmp;
                    }
                    /*int argbColor = (int)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
                    var color = System.Drawing.Color.FromArgb(argbColor);*/
                    e.Handled = e.SuppressKeyPress = true;
                }
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (tbMessage.Text == "") return;
                RildasServerAPI.SendGroupMessage(_chatGroup.id, tbMessage.Text);
                tbMessage.Text = "";
                e.Handled = e.SuppressKeyPress = true;
            }
            if (!_pressed.Exists(x => x == e.KeyCode))
            {
                _pressed.Add(e.KeyCode);
            }

        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            _pressed.Remove(e.KeyCode);
        }

        private void ChatWindowGroup_Shown(object sender, EventArgs e)
        {
            LoadLoggedState();
        }
        public void GetOnTop()
        {
            this.TopMost = true;
            this.Focus();
            this.BringToFront();
            this.Activate();
            this.TopMost = cbAlwaysOnTop.Checked;
        }
        private void LoadLoggedState()
        {
            try
            {
                List<User> logged = Global.GetLoggedUsers();
                int positionIterator = 0;

                usersPanel.Invoke(new MethodInvoker(delegate
                {
                    usersPanel.Controls.Clear();
                    _chatGroup.members.Sort((x, y) => String.Compare(x.username, y.username, StringComparison.Ordinal));
                    foreach (User user in _chatGroup.members)
                    {
                        MetroLink name = new MetroLink();
                        PictureBox state = new PictureBox();
                        name.Click += Name_Click;
                        name.FontSize = MetroFramework.MetroLinkSize.Medium;
                        name.Location = new System.Drawing.Point(25, positionIterator * 25);
                        name.Name = "namePrivateChat" + user.username;
                        name.Size = new System.Drawing.Size(150, 23);
                        name.TabIndex = 3;
                        name.Text = user.username;
                        name.Tag = user;
                        name.Theme = MetroFramework.MetroThemeStyle.Dark;
                        name.UseSelectable = true;
                        name.TextAlign = ContentAlignment.TopLeft;
                        state.Size = new Size(20, 20);
                        state.Location = new Point(1, name.Location.Y);
                        state.Name = user.username + "_state";
                        state.Image = logged.Exists(x => x.username == user.username) ? Resources.green : Resources.red;
                        usersPanel.Controls.Add(name);
                        usersPanel.Controls.Add(state);
                        state.BringToFront();
                        positionIterator++;
                    }
                    usersPanel.Refresh();
                }));
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            // Border around richtextbox
            g.DrawRectangle(new Pen(Color.Gray), richTextBox1.Location.X - 1, richTextBox1.Location.Y - 1, richTextBox1.Width + 2, richTextBox1.Height + 2);

            /*if (_focus)
            {
                tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
                Pen p;
                switch (this.Style)
                {
                    case MetroFramework.MetroColorStyle.Red: p = new Pen(Color.FromArgb(209, 17, 65)); break;
                    case MetroFramework.MetroColorStyle.Yellow: p = new Pen(Color.Yellow); break; // TODO: Najít barvu pro Yellow
                    default: p = new Pen(Color.FromArgb(142, 188, 0)); break;
                }

                int variance = 1;
                g.DrawRectangle(p, new Rectangle(tbMessage.Location.X - variance, tbMessage.Location.Y - variance, tbMessage.Width + variance, tbMessage.Height + variance));
            }
            else
            {
                tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }*/
        }

        private void Name_Click(object sender, EventArgs e)
        {
            User user = (sender as MetroLink).Tag as User;
            Global.OpenIfNeeded(user, userTriggeredAction: true);
        }

        private void ChatWindowGroup_Activated(object sender, EventArgs e)
        {
            this.tbMessage.Focus();
        }

        private void ChatWindowGroup_Load(object sender, EventArgs e)
        {
            ChatWindow_Resize(this, new EventArgs());
        }
    }
}
