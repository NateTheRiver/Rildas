﻿using System;
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
using System.Threading;

namespace RildasApp.Forms
{
    public partial class ChatWindowPrivate : MetroFramework.Forms.MetroForm
    {
        // To support flashing.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        public const UInt32 FLASHW_ALL = 3;
        // Flash continuously until the window comes to the foreground. 
        public const UInt32 FLASHW_TIMERNOFG = 12;

        bool focus = false;
        List<Keys> pressed;
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
        public ChatWindowPrivate()
        {
            InitializeComponent();
            pressed = new List<Keys>();
            Global.UserDisconnected += UserLeave;
            Global.UserConnected += UserEnter;
            richTextBox1.Location = new Point(1, 1);
            panel1.Size = new Size(richTextBox1.Size.Width + 2, richTextBox1.Size.Height + 2);
            this.SetStyle(ControlStyles.UserPaint, true);

        }

        private void UserEnter(User user)
        {
            if ((this.Tag as User).id == user.id)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    Append(richTextBox1, "Uživatel " + (this.Tag as User).username + " se nad Vámi slitoval a opět je tady.", Color.White);
                    Append(richTextBox1, Environment.NewLine, Color.White);
                    richTextBox1.ScrollToCaret();
                }));
            }
        }

        private void UserLeave(User user)
        {
            if ((this.Tag as User).id == user.id)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    Append(richTextBox1, "Uživatel "+ (this.Tag as User).username+ " se na Vás vykašlal a prostě to vypnul.", Color.White);
                    Append(richTextBox1, Environment.NewLine, Color.White);
                    richTextBox1.ScrollToCaret();
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
            if (cbAlwaysOnTop.Checked)
            {
                this.TopMost = true;
            }
            else this.TopMost = false;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (tbMessage.Text == "") return;
            Append(richTextBox1, String.Format("[{0}] ", DateTime.Now.ToString("HH:mm")), Color.FromArgb(60, 130, 231));
            Append(richTextBox1, Global.loggedUser.username, Color.FromArgb(60, 130, 231));
            Append(richTextBox1, ": " + tbMessage.Text, Color.White);
            Append(richTextBox1, Environment.NewLine, Color.White);
            richTextBox1.ScrollToCaret();
            RildasServerAPI.SendMessage((this.Tag as User).id, tbMessage.Text);
            tbMessage.Text = "";
            this.tbMessage.Focus();
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            RildasServerAPI.SendNoticeRequest((Tag as User).id);
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
                Append(richTextBox1, " " + (this.Tag as User).username + " si vyžaduje vaši pozornost.", Color.FromArgb(231, 76, 60));
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
                RildasServerAPI.SendMessage((this.Tag as User).id, tbMessage.Text);
                tbMessage.Text = "";
                e.Handled = e.SuppressKeyPress = true;
            }
            if (!pressed.Exists(x => x == e.KeyCode))
            {
                pressed.Add(e.KeyCode);
            }

        }

        private void tbMessage_Enter(object sender, EventArgs e)
        {
            focus = true;
            this.Refresh();
            this.Invalidate();
        }

        private void tbMessage_Leave(object sender, EventArgs e)
        {
            focus = false;
            this.Refresh();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (focus)
            {
                tbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
                Pen p = new Pen(Color.FromArgb(142, 188, 0));
                Graphics g = e.Graphics;
                int variance = 3;
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
    }
}
