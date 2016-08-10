using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using RildasApp.Models;
using System.Runtime.InteropServices;
using RildasApp.Properties;
using System.IO;
namespace RildasApp.Forms
{
    public partial class ChatWindowPrivate : MetroFramework.Forms.MetroForm
    {
        // To support flashing.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RildasLogs");
        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;
        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;
        bool _focus = false;
        List<Keys> pressed;
        protected override bool ShowWithoutActivation => true;

        public ChatWindowPrivate()
        {
            InitializeComponent();
            pressed = new List<Keys>();
            Global.UserDisconnected += UserLeave;
            Global.UserConnected += UserEnter;
            this.FormClosing += OnFormClosing;
            richTextBox1.Location = new Point(1, 1);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        private void RichTextBox1OnLinkClicked(object sender, LinkClickedEventArgs linkClickedEventArgs)
        {

        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            Global.UserDisconnected -= UserLeave;
            Global.UserConnected -= UserEnter;
        }

        public void GetOnTop()
        {
            this.TopMost = true;
            this.Focus();
            this.tbMessage.SelectionStart = this.tbMessage.Text.Length;
            this.BringToFront();
            this.Activate();
            this.TopMost = cbAlwaysOnTop.Checked;
        }

        private void UserEnter(User user)
        {
            if ((this.Tag as User).id == user.id)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.Style = MetroFramework.MetroColorStyle.Lime;
                    Append(richTextBox1, (this.Tag as User).username + " se nad Vámi slitoval a opět je tady.", Color.White);
                    Append(richTextBox1, Environment.NewLine, Color.White);
                    richTextBox1.ScrollToCaret();
                    this.Refresh();
                }));
            }
        }

        private void UserLeave(User user)
        {
            if ((this.Tag as User).id == user.id)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.Style = MetroFramework.MetroColorStyle.Red;
                    Append(richTextBox1, (this.Tag as User).username+ " se na Vás vykašlal a prostě to vypnul.", Color.White);
                    Append(richTextBox1, Environment.NewLine, Color.White);
                    richTextBox1.ScrollToCaret();
                    this.Refresh();
                }));
            }
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
            const int textboxPadding = 10;
            const int buttonPading = 5;
            
            tbMessage.Location = new Point(padding, this.Height - (padding + tbMessage.Height));
            tbMessage.Size = new Size(btnNotice.Location.X - (tbMessage.Location.X + buttonPading), tbMessage.Height);
            richTextBox1.Location = new Point(padding, 63);
            richTextBox1.Size = new Size(this.Width - (padding * 2), tbMessage.Location.Y - (richTextBox1.Location.Y + textboxPadding));
            cbAlwaysOnTop.Location = new Point(richTextBox1.Location.X + richTextBox1.Width - cbAlwaysOnTop.Width, cbAlwaysOnTop.Location.Y);
            btnNotice.Location = new Point(this.Width - (btnNotice.Width + padding), this.Height - (btnNotice.Height + padding));
            btnSend.Location = new Point(this.Width - (btnSend.Width + padding), btnNotice.Location.Y - buttonPading - btnSend.Height);            
            // TODO: Resize všech komponent
        }
        public void AppendMessage(string message, DateTime time)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Append(richTextBox1, String.Format("[{0}]", time.ToString("HH:mm")), Color.FromArgb(231, 76, 60));
                Append(richTextBox1, (this.Tag as User).username, Color.FromArgb(231, 76, 60));
                Append(richTextBox1, ": " + message, Color.White);
                Append(richTextBox1, Environment.NewLine, Color.White);
                richTextBox1.ScrollToCaret();
            }));

            string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RildasLogs");

            try
            {
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                File.AppendAllText(Path.Combine(dirPath, (this.Tag as User).username + ".txt"), String.Format("[{0}]{1}: {2}", time.ToString("HH:mm"), (this.Tag as User).username, message) + Environment.NewLine);

            }
            catch (Exception)
            {
            }   
        }
        private static void Append(RichTextBox box, string text, Color color)
        {

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
        private void cbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = cbAlwaysOnTop.Checked;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (tbMessage.Text == "") return;
            Append(richTextBox1, String.Format("[{0}]", DateTime.Now.ToString("HH:mm")), Color.FromArgb(60, 130, 231));
            Append(richTextBox1, Global.loggedUser.username, Color.FromArgb(60, 130, 231));
            Append(richTextBox1, ": " + tbMessage.Text, Color.White);
            Append(richTextBox1, Environment.NewLine, Color.White);
            richTextBox1.ScrollToCaret();
            RildasServerAPI.SendMessage(((User)this.Tag).id, tbMessage.Text);
            this.tbMessage.Focus();
            this.tbMessage.SelectionStart = this.tbMessage.Text.Length;
            string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RildasLogs");

            try
            {
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                File.AppendAllText(Path.Combine(dirPath, (this.Tag as User).username + ".txt"), String.Format("[{0}]{1}: {2}", DateTime.Now.ToString("HH:mm"), Global.loggedUser.username, tbMessage.Text) + Environment.NewLine);

            }
            catch (Exception)
            {
            }
            tbMessage.Text = "";

        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            RildasServerAPI.SendNoticeRequest(((User) Tag).id);
            Append(richTextBox1, String.Format("[{0}]", DateTime.Now.ToString("HH:mm")), Color.FromArgb(60, 130, 231));
            Append(richTextBox1, " Žádost o pozornost odeslána.", Color.FromArgb(60, 130, 231));
            Append(richTextBox1, Environment.NewLine, Color.White);
            richTextBox1.ScrollToCaret();

        }
        internal void NoticeRequest()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Append(richTextBox1, String.Format("[{0}]", DateTime.Now.ToString("HH:mm")), Color.FromArgb(231, 76, 60));
                Append(richTextBox1, " " + ((User)this.Tag).username + " si vyžaduje vaši pozornost.", Color.FromArgb(231, 76, 60));
                Append(richTextBox1, Environment.NewLine, Color.White);
                richTextBox1.ScrollToCaret();
                this.Activate();

            }));
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (pressed.Exists(x => x == Keys.ControlKey))
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
                Append(richTextBox1, String.Format("[{0}]", DateTime.Now.ToString("HH:mm")), Color.FromArgb(60, 130, 231));
                Append(richTextBox1, Global.loggedUser.username, Color.FromArgb(60, 130, 231));
                Append(richTextBox1, ": " + tbMessage.Text, Color.White);
                Append(richTextBox1, Environment.NewLine, Color.White);
                richTextBox1.ScrollToCaret();
                RildasServerAPI.SendMessage(((User)this.Tag).id, tbMessage.Text);
                e.Handled = e.SuppressKeyPress = true;
                string dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RildasLogs");

                try
                {
                    if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
                    File.AppendAllText(Path.Combine(dirPath, (this.Tag as User).username + ".txt"), String.Format("[{0}]{1}: {2}", DateTime.Now.ToString("HH:mm"), Global.loggedUser.username, tbMessage.Text) + Environment.NewLine);

                }
                catch (Exception)
                {
                }
                tbMessage.Text = "";

            }
            if (!pressed.Exists(x => x == e.KeyCode))
            {
                pressed.Add(e.KeyCode);
            }

        }

        private void tbMessage_Enter(object sender, EventArgs e)
        {
            _focus = true;
            this.Refresh();
            this.Invalidate();
        }

        private void tbMessage_Leave(object sender, EventArgs e)
        {
            _focus = false;
            this.Refresh();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            // Border around richtextbox
            g.DrawRectangle(new Pen(Color.Gray), richTextBox1.Location.X - 1, richTextBox1.Location.Y - 1, richTextBox1.Width + 2, richTextBox1.Height + 2);

            if (_focus)
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
            }
        }
        private void ChatWindowPrivate_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void tbMessage_KeyUp(object sender, KeyEventArgs e)
        {
            pressed.Remove(e.KeyCode);
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void ChatWindowPrivate_Load(object sender, EventArgs e)
        {
            if (Global.GetLoggedUsers().Exists(x => x.username == (this.Tag as User).username))
            {
                this.Style = MetroFramework.MetroColorStyle.Lime;
            }
            else
            {
                this.Style = MetroFramework.MetroColorStyle.Red;
            }
        }

        private void ChatWindowPrivate_Activated(object sender, EventArgs e)
        {
            this.tbMessage.Focus();
            this.tbMessage.SelectionStart = this.tbMessage.Text.Length;
        }
    }
}
