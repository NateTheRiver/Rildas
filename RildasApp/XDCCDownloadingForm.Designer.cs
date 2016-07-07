namespace RildasApp
{
    partial class XDCCDownloadingForm
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
            this._lbFileName = new MetroFramework.Controls.MetroLabel();
            this._lbFileSize = new MetroFramework.Controls.MetroLabel();
            this._progressBar = new MetroFramework.Controls.MetroProgressBar();
            this._lbProgress = new MetroFramework.Controls.MetroLabel();
            this._lbSpeed = new MetroFramework.Controls.MetroLabel();
            this._lbState = new MetroFramework.Controls.MetroLabel();
            this._lbDownloaded = new MetroFramework.Controls.MetroLabel();
            this._lbETA = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // _lbFileName
            // 
            this._lbFileName.AutoSize = true;
            this._lbFileName.Location = new System.Drawing.Point(23, 82);
            this._lbFileName.Name = "_lbFileName";
            this._lbFileName.Size = new System.Drawing.Size(119, 19);
            this._lbFileName.TabIndex = 0;
            this._lbFileName.Text = "FileName: Loading";
            this._lbFileName.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _lbFileSize
            // 
            this._lbFileSize.AutoSize = true;
            this._lbFileSize.Location = new System.Drawing.Point(23, 113);
            this._lbFileSize.Name = "_lbFileSize";
            this._lbFileSize.Size = new System.Drawing.Size(106, 19);
            this._lbFileSize.TabIndex = 1;
            this._lbFileSize.Text = "FileSize: Loading";
            this._lbFileSize.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _progressBar
            // 
            this._progressBar.Location = new System.Drawing.Point(23, 156);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(531, 23);
            this._progressBar.TabIndex = 2;
            this._progressBar.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _lbProgress
            // 
            this._lbProgress.AutoSize = true;
            this._lbProgress.FontSize = MetroFramework.MetroLabelSize.Tall;
            this._lbProgress.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this._lbProgress.ForeColor = System.Drawing.SystemColors.HotTrack;
            this._lbProgress.Location = new System.Drawing.Point(560, 156);
            this._lbProgress.Name = "_lbProgress";
            this._lbProgress.Size = new System.Drawing.Size(38, 25);
            this._lbProgress.Style = MetroFramework.MetroColorStyle.Blue;
            this._lbProgress.TabIndex = 5;
            this._lbProgress.Text = "0%";
            this._lbProgress.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._lbProgress.UseStyleColors = true;
            // 
            // _lbSpeed
            // 
            this._lbSpeed.AutoSize = true;
            this._lbSpeed.Location = new System.Drawing.Point(23, 182);
            this._lbSpeed.Name = "_lbSpeed";
            this._lbSpeed.Size = new System.Drawing.Size(152, 19);
            this._lbSpeed.TabIndex = 7;
            this._lbSpeed.Text = "Download speed: 0 kbps";
            this._lbSpeed.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _lbState
            // 
            this._lbState.AutoSize = true;
            this._lbState.FontSize = MetroFramework.MetroLabelSize.Tall;
            this._lbState.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this._lbState.ForeColor = System.Drawing.SystemColors.HotTrack;
            this._lbState.Location = new System.Drawing.Point(23, 270);
            this._lbState.Name = "_lbState";
            this._lbState.Size = new System.Drawing.Size(245, 25);
            this._lbState.Style = MetroFramework.MetroColorStyle.Blue;
            this._lbState.TabIndex = 8;
            this._lbState.Text = "State: Preparing to connect";
            this._lbState.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._lbState.UseStyleColors = true;
            // 
            // _lbDownloaded
            // 
            this._lbDownloaded.AutoSize = true;
            this._lbDownloaded.Location = new System.Drawing.Point(407, 182);
            this._lbDownloaded.Name = "_lbDownloaded";
            this._lbDownloaded.Size = new System.Drawing.Size(121, 19);
            this._lbDownloaded.TabIndex = 9;
            this._lbDownloaded.Text = "Downloaded: 0 MB";
            this._lbDownloaded.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _lbETA
            // 
            this._lbETA.AutoSize = true;
            this._lbETA.Location = new System.Drawing.Point(259, 182);
            this._lbETA.Name = "_lbETA";
            this._lbETA.Size = new System.Drawing.Size(65, 19);
            this._lbETA.TabIndex = 10;
            this._lbETA.Text = "ETA:  150s";
            this._lbETA.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(466, 281);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(132, 36);
            this.metroButton1.TabIndex = 11;
            this.metroButton1.Text = "CANCEL DOWNLOAD";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(466, 239);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(131, 36);
            this.metroButton2.TabIndex = 12;
            this.metroButton2.Text = "View download folder";
            this.metroButton2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton2.UseSelectable = true;
            this.metroButton2.Click += new System.EventHandler(this.metroButton2_Click);
            // 
            // XDCCDownloadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 343);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this._lbETA);
            this.Controls.Add(this._lbDownloaded);
            this.Controls.Add(this._lbState);
            this.Controls.Add(this._lbSpeed);
            this.Controls.Add(this._lbProgress);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._lbFileSize);
            this.Controls.Add(this._lbFileName);
            this.Name = "XDCCDownloadingForm";
            this.Resizable = false;
            this.Text = "Downloading #11674 from HelloKittyBot";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.XDCCDownloadingForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel _lbFileName;
        private MetroFramework.Controls.MetroLabel _lbFileSize;
        private MetroFramework.Controls.MetroProgressBar _progressBar;
        private MetroFramework.Controls.MetroLabel _lbProgress;
        private MetroFramework.Controls.MetroLabel _lbSpeed;
        private MetroFramework.Controls.MetroLabel _lbState;
        private MetroFramework.Controls.MetroLabel _lbDownloaded;
        private MetroFramework.Controls.MetroLabel _lbETA;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroButton metroButton2;
    }
}