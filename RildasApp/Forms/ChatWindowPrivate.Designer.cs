using System;
using System.Drawing;

namespace RildasApp.Forms
{
    partial class ChatWindowPrivate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbMessage = new MetroFramework.Controls.MetroTextBox();
            this.cbAlwaysOnTop = new MetroFramework.Controls.MetroCheckBox();
            this.btnSend = new MetroFramework.Controls.MetroButton();
            this.btnNotice = new MetroFramework.Controls.MetroButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMessage
            // 
            // 
            // 
            // 
            this.tbMessage.CustomButton.Image = null;
            this.tbMessage.CustomButton.Location = new System.Drawing.Point(392, 1);
            this.tbMessage.CustomButton.Name = "";
            this.tbMessage.CustomButton.Size = new System.Drawing.Size(67, 67);
            this.tbMessage.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbMessage.CustomButton.TabIndex = 1;
            this.tbMessage.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbMessage.CustomButton.UseSelectable = true;
            this.tbMessage.CustomButton.Visible = false;
            this.tbMessage.Lines = new string[0];
            this.tbMessage.Location = new System.Drawing.Point(23, 305);
            this.tbMessage.MaxLength = 32767;
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.PasswordChar = '\0';
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbMessage.SelectedText = "";
            this.tbMessage.SelectionLength = 0;
            this.tbMessage.SelectionStart = 0;
            this.tbMessage.Size = new System.Drawing.Size(460, 69);
            this.tbMessage.Style = MetroFramework.MetroColorStyle.Lime;
            this.tbMessage.TabIndex = 0;
            this.tbMessage.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tbMessage.UseSelectable = true;
            this.tbMessage.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbMessage.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.tbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyDown);
            // 
            // cbAlwaysOnTop
            // 
            this.cbAlwaysOnTop.AutoSize = true;
            this.cbAlwaysOnTop.Checked = true;
            this.cbAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAlwaysOnTop.Location = new System.Drawing.Point(500, 32);
            this.cbAlwaysOnTop.Name = "cbAlwaysOnTop";
            this.cbAlwaysOnTop.Size = new System.Drawing.Size(98, 15);
            this.cbAlwaysOnTop.TabIndex = 2;
            this.cbAlwaysOnTop.Text = "Always on top";
            this.cbAlwaysOnTop.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cbAlwaysOnTop.UseSelectable = true;
            this.cbAlwaysOnTop.CheckedChanged += new System.EventHandler(this.cbAlwaysOnTop_CheckedChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(489, 305);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(109, 42);
            this.btnSend.Style = MetroFramework.MetroColorStyle.Lime;
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSend.UseSelectable = true;
            this.btnSend.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // btnNotice
            // 
            this.btnNotice.Location = new System.Drawing.Point(489, 353);
            this.btnNotice.Name = "btnNotice";
            this.btnNotice.Size = new System.Drawing.Size(109, 21);
            this.btnNotice.Style = MetroFramework.MetroColorStyle.Lime;
            this.btnNotice.TabIndex = 3;
            this.btnNotice.Text = "Notice me senpai";
            this.btnNotice.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnNotice.UseSelectable = true;
            this.btnNotice.Click += new System.EventHandler(this.btnNotice_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(1, 1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(572, 230);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.ForeColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(23, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 232);
            this.panel1.TabIndex = 6;
            // 
            // ChatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 408);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNotice);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cbAlwaysOnTop);
            this.Controls.Add(this.tbMessage);
            this.Name = "ChatWindow";
            this.Style = MetroFramework.MetroColorStyle.Lime;
            this.Text = "Private chat - NateTheRiver";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.ChatWindow_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroTextBox tbMessage;
        private MetroFramework.Controls.MetroCheckBox cbAlwaysOnTop;
        private MetroFramework.Controls.MetroButton btnSend;
        private MetroFramework.Controls.MetroButton btnNotice;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;

       
    }
}