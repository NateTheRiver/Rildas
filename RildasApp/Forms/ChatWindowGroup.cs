using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using RildasApp.Models;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace RildasApp.Forms
{
    public partial class ChatWindowGroup : MetroFramework.Forms.MetroForm
    {
        private readonly ChatGroup _chatGroup;
        private readonly List<User> _loggedUsers;
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

        public ChatWindowGroup(ChatGroup chatGroup, List<User> loggedUsers )
        {
            _chatGroup = chatGroup;
            _loggedUsers = loggedUsers;
            _pressed = new List<Keys>();
            InitializeComponent();
            richTextBox1.Location = new Point(1, 1);
            panel1.Size = new Size(richTextBox1.Size.Width + 2, richTextBox1.Size.Height + 2);
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
            // TODO: Resize všech komponent
        }
        public void AppendMessage(string username, string message, DateTime time)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Append(richTextBox1, String.Format("[{0}]", time.ToString("HH:mm")), Color.FromArgb(231, 76, 60));
                Append(richTextBox1, username, Color.FromArgb(231, 76, 60));
                Append(richTextBox1, ": " + message, Color.White);
                Append(richTextBox1, Environment.NewLine, Color.White);
                richTextBox1.ScrollToCaret();
            }));


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
            foreach (var user in _chatGroup.members)
            {
                //user.
            }
        }
    }
}
