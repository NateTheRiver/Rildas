namespace RildasApp.Forms
{
    partial class UpdateNotification
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
            this.button_yes = new MetroFramework.Controls.MetroButton();
            this.button_no = new MetroFramework.Controls.MetroButton();
            this.label_update = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // button_yes
            // 
            this.button_yes.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.button_yes.Location = new System.Drawing.Point(30, 108);
            this.button_yes.Name = "button_yes";
            this.button_yes.Size = new System.Drawing.Size(104, 41);
            this.button_yes.TabIndex = 1;
            this.button_yes.Text = "Stáhnout";
            this.button_yes.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.button_yes.UseSelectable = true;
            this.button_yes.Click += new System.EventHandler(this.button_yes_Click);
            // 
            // button_no
            // 
            this.button_no.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.button_no.Location = new System.Drawing.Point(254, 108);
            this.button_no.Name = "button_no";
            this.button_no.Size = new System.Drawing.Size(104, 41);
            this.button_no.TabIndex = 2;
            this.button_no.Text = "Pokračovat";
            this.button_no.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.button_no.UseSelectable = true;
            this.button_no.Click += new System.EventHandler(this.button_no_Click);
            // 
            // label_update
            // 
            this.label_update.Location = new System.Drawing.Point(30, 23);
            this.label_update.Margin = new System.Windows.Forms.Padding(0);
            this.label_update.Name = "label_update";
            this.label_update.Size = new System.Drawing.Size(328, 72);
            this.label_update.TabIndex = 0;
            this.label_update.Text = "metroLabel1";
            this.label_update.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.label_update.WrapToLine = true;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(370, 2);
            this.metroButton1.Margin = new System.Windows.Forms.Padding(0);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(14, 13);
            this.metroButton1.TabIndex = 3;
            this.metroButton1.Text = "X";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // UpdateNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.label_update);
            this.Controls.Add(this.button_no);
            this.Controls.Add(this.button_yes);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpdateNotification";
            this.ShowIcon = false;
            this.Text = "New Version";
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton button_yes;
        private MetroFramework.Controls.MetroButton button_no;
        private MetroFramework.Controls.MetroLabel label_update;
        private MetroFramework.Controls.MetroButton metroButton1;
    }
}