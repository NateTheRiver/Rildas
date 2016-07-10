namespace RildasApp.Forms
{
    partial class ApplicationUpdateInformationForm
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
            this._lbCurr = new MetroFramework.Controls.MetroLabel();
            this._lbServerVersion = new MetroFramework.Controls.MetroLabel();
            this._lbDownloading = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this._tbPatchNotes = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // _lbCurr
            // 
            this._lbCurr.AutoSize = true;
            this._lbCurr.Location = new System.Drawing.Point(23, 85);
            this._lbCurr.Name = "_lbCurr";
            this._lbCurr.Size = new System.Drawing.Size(106, 19);
            this._lbCurr.TabIndex = 0;
            this._lbCurr.Text = "Current Version: ";
            this._lbCurr.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _lbServerVersion
            // 
            this._lbServerVersion.AutoSize = true;
            this._lbServerVersion.Location = new System.Drawing.Point(23, 104);
            this._lbServerVersion.Name = "_lbServerVersion";
            this._lbServerVersion.Size = new System.Drawing.Size(104, 19);
            this._lbServerVersion.TabIndex = 1;
            this._lbServerVersion.Text = "Server Version:  ";
            this._lbServerVersion.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _lbDownloading
            // 
            this._lbDownloading.AutoSize = true;
            this._lbDownloading.FontSize = MetroFramework.MetroLabelSize.Tall;
            this._lbDownloading.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this._lbDownloading.ForeColor = System.Drawing.SystemColors.HotTrack;
            this._lbDownloading.Location = new System.Drawing.Point(95, 378);
            this._lbDownloading.Name = "_lbDownloading";
            this._lbDownloading.Size = new System.Drawing.Size(689, 25);
            this._lbDownloading.Style = MetroFramework.MetroColorStyle.Blue;
            this._lbDownloading.TabIndex = 2;
            this._lbDownloading.Text = "Po dokončení updatu se aplikace sama restartuje. Sit back and get some coffee.";
            this._lbDownloading.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._lbDownloading.UseStyleColors = true;
            this._lbDownloading.Visible = false;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 153);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(81, 19);
            this.metroLabel2.TabIndex = 3;
            this.metroLabel2.Text = "Patch Notes:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // _tbPatchNotes
            // 
            // 
            // 
            // 
            this._tbPatchNotes.CustomButton.Image = null;
            this._tbPatchNotes.CustomButton.Location = new System.Drawing.Point(454, 2);
            this._tbPatchNotes.CustomButton.Name = "";
            this._tbPatchNotes.CustomButton.Size = new System.Drawing.Size(217, 217);
            this._tbPatchNotes.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this._tbPatchNotes.CustomButton.TabIndex = 1;
            this._tbPatchNotes.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this._tbPatchNotes.CustomButton.UseSelectable = true;
            this._tbPatchNotes.CustomButton.Visible = false;
            this._tbPatchNotes.Lines = new string[0];
            this._tbPatchNotes.Location = new System.Drawing.Point(110, 153);
            this._tbPatchNotes.MaxLength = 32767;
            this._tbPatchNotes.Multiline = true;
            this._tbPatchNotes.Name = "_tbPatchNotes";
            this._tbPatchNotes.PasswordChar = '\0';
            this._tbPatchNotes.ReadOnly = true;
            this._tbPatchNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._tbPatchNotes.SelectedText = "";
            this._tbPatchNotes.SelectionLength = 0;
            this._tbPatchNotes.SelectionStart = 0;
            this._tbPatchNotes.Size = new System.Drawing.Size(674, 222);
            this._tbPatchNotes.TabIndex = 4;
            this._tbPatchNotes.Theme = MetroFramework.MetroThemeStyle.Dark;
            this._tbPatchNotes.UseSelectable = true;
            this._tbPatchNotes.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this._tbPatchNotes.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // ApplicationUpdateInformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 423);
            this.Controls.Add(this._tbPatchNotes);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this._lbDownloading);
            this.Controls.Add(this._lbServerVersion);
            this.Controls.Add(this._lbCurr);
            this.Name = "ApplicationUpdateInformationForm";
            this.Text = "Updating to new version";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel _lbCurr;
        private MetroFramework.Controls.MetroLabel _lbServerVersion;
        private MetroFramework.Controls.MetroLabel _lbDownloading;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox _tbPatchNotes;
    }
}