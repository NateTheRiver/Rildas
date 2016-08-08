namespace RildasApp.Forms
{
    partial class ChatWindowGroup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatWindowGroup));
            this.tbMessage = new MetroFramework.Controls.MetroTextBox();
            this.cbAlwaysOnTop = new MetroFramework.Controls.MetroCheckBox();
            this.btnSend = new MetroFramework.Controls.MetroButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
            this.usersPanel = new MetroFramework.Controls.MetroPanel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMessage
            // 
            // 
            // 
            // 
            this.tbMessage.CustomButton.Image = null;
            this.tbMessage.CustomButton.Location = new System.Drawing.Point(410, 1);
            this.tbMessage.CustomButton.Name = "";
            this.tbMessage.CustomButton.Size = new System.Drawing.Size(49, 49);
            this.tbMessage.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.tbMessage.CustomButton.TabIndex = 1;
            this.tbMessage.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tbMessage.CustomButton.UseSelectable = true;
            this.tbMessage.CustomButton.Visible = false;
            this.tbMessage.Lines = new string[0];
            this.tbMessage.Location = new System.Drawing.Point(23, 323);
            this.tbMessage.MaxLength = 32767;
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.PasswordChar = '\0';
            this.tbMessage.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.tbMessage.SelectedText = "";
            this.tbMessage.SelectionLength = 0;
            this.tbMessage.SelectionStart = 0;
            this.tbMessage.Size = new System.Drawing.Size(460, 51);
            this.tbMessage.Style = MetroFramework.MetroColorStyle.Lime;
            this.tbMessage.TabIndex = 0;
            this.tbMessage.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tbMessage.UseSelectable = true;
            this.tbMessage.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.tbMessage.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.tbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyDown);
            this.tbMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbMessage_KeyUp);
            // 
            // cbAlwaysOnTop
            // 
            this.cbAlwaysOnTop.AutoSize = true;
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
            this.btnSend.Location = new System.Drawing.Point(489, 323);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(109, 51);
            this.btnSend.Style = MetroFramework.MetroColorStyle.Lime;
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSend.UseSelectable = true;
            this.btnSend.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(1, 1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(572, 253);
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
            // metroLabel13
            // 
            this.metroLabel13.AutoSize = true;
            this.metroLabel13.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel13.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel13.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.metroLabel13.Location = new System.Drawing.Point(617, 22);
            this.metroLabel13.Name = "metroLabel13";
            this.metroLabel13.Size = new System.Drawing.Size(129, 25);
            this.metroLabel13.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel13.TabIndex = 7;
            this.metroLabel13.Text = "Seznam členů";
            this.metroLabel13.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel13.UseStyleColors = true;
            this.metroLabel13.Visible = false;
            // 
            // usersPanel
            // 
            this.usersPanel.HorizontalScrollbarBarColor = true;
            this.usersPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.usersPanel.HorizontalScrollbarSize = 10;
            this.usersPanel.Location = new System.Drawing.Point(617, 63);
            this.usersPanel.Name = "usersPanel";
            this.usersPanel.Size = new System.Drawing.Size(150, 300);
            this.usersPanel.TabIndex = 8;
            this.usersPanel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.usersPanel.VerticalScrollbarBarColor = true;
            this.usersPanel.VerticalScrollbarHighlightOnWheel = false;
            this.usersPanel.VerticalScrollbarSize = 10;
            // 
            // ChatWindowGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 408);
            this.Controls.Add(this.usersPanel);
            this.Controls.Add(this.metroLabel13);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cbAlwaysOnTop);
            this.Controls.Add(this.tbMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(829, 408);
            this.Name = "ChatWindowGroup";
            this.Style = MetroFramework.MetroColorStyle.Lime;
            this.Text = "Group chat: Rildas";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Activated += new System.EventHandler(this.ChatWindowGroup_Activated);
            this.Shown += new System.EventHandler(this.ChatWindowGroup_Shown);
            this.Resize += new System.EventHandler(this.ChatWindow_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroTextBox tbMessage;
        private MetroFramework.Controls.MetroCheckBox cbAlwaysOnTop;
        private MetroFramework.Controls.MetroButton btnSend;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private MetroFramework.Controls.MetroLabel metroLabel13;
        private MetroFramework.Controls.MetroPanel usersPanel;
    }
}