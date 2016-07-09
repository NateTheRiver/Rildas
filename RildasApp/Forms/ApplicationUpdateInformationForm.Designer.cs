﻿namespace RildasApp.Forms
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this._tbPatchNotes = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // _lbCurr
            // 
            this._lbCurr.AutoSize = true;
            this._lbCurr.Location = new System.Drawing.Point(23, 85);
            this._lbCurr.Name = "_lbCurr";
            this._lbCurr.Size = new System.Drawing.Size(141, 19);
            this._lbCurr.TabIndex = 0;
            this._lbCurr.Text = "Current Version: 0.1.0.0";
            this._lbCurr.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 104);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(148, 19);
            this.metroLabel1.TabIndex = 1;
            this.metroLabel1.Text = "Server Version:  0.5.0.0a";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel13
            // 
            this.metroLabel13.AutoSize = true;
            this.metroLabel13.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel13.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel13.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.metroLabel13.Location = new System.Drawing.Point(95, 378);
            this.metroLabel13.Name = "metroLabel13";
            this.metroLabel13.Size = new System.Drawing.Size(689, 25);
            this.metroLabel13.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel13.TabIndex = 2;
            this.metroLabel13.Text = "Po dokončení updatu se aplikace sama restartuje. Sit back and get some coffee.";
            this.metroLabel13.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel13.UseStyleColors = true;
            this.metroLabel13.Visible = false;
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
            this.Controls.Add(this.metroLabel13);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this._lbCurr);
            this.Name = "ApplicationUpdateInformationForm";
            this.Text = "Updating to new version";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel _lbCurr;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel13;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox _tbPatchNotes;
    }
}