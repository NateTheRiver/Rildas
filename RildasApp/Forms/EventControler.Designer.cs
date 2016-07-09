namespace RildasApp.Forms
{
    partial class EventControler
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroDateTime1 = new MetroFramework.Controls.MetroDateTime();
            this.metroAddButton = new MetroFramework.Controls.MetroButton();
            this.metroMoveButton = new MetroFramework.Controls.MetroButton();
            this.metroDelButton = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(18, 74);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(55, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Datum: ";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroDateTime1
            // 
            this.metroDateTime1.CustomFormat = "dd.MM.yyyy";
            this.metroDateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.metroDateTime1.Location = new System.Drawing.Point(101, 64);
            this.metroDateTime1.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTime1.Name = "metroDateTime1";
            this.metroDateTime1.Size = new System.Drawing.Size(160, 29);
            this.metroDateTime1.TabIndex = 1;
            this.metroDateTime1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroDateTime1.ValueChanged += new System.EventHandler(this.metroDateTime1_ValueChanged);
            // 
            // metroAddButton
            // 
            this.metroAddButton.Location = new System.Drawing.Point(18, 401);
            this.metroAddButton.Name = "metroAddButton";
            this.metroAddButton.Size = new System.Drawing.Size(80, 39);
            this.metroAddButton.TabIndex = 3;
            this.metroAddButton.Text = "Přidej";
            this.metroAddButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroAddButton.UseSelectable = true;
            this.metroAddButton.Click += new System.EventHandler(this.metroAddButton_Click);
            // 
            // metroMoveButton
            // 
            this.metroMoveButton.Location = new System.Drawing.Point(104, 401);
            this.metroMoveButton.Name = "metroMoveButton";
            this.metroMoveButton.Size = new System.Drawing.Size(80, 39);
            this.metroMoveButton.TabIndex = 3;
            this.metroMoveButton.Text = "Přesuň";
            this.metroMoveButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroMoveButton.UseSelectable = true;
            this.metroMoveButton.Click += new System.EventHandler(this.metroMoveButton_Click);
            // 
            // metroDelButton
            // 
            this.metroDelButton.Location = new System.Drawing.Point(190, 401);
            this.metroDelButton.Name = "metroDelButton";
            this.metroDelButton.Size = new System.Drawing.Size(80, 39);
            this.metroDelButton.TabIndex = 3;
            this.metroDelButton.Text = "Smaž";
            this.metroDelButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroDelButton.UseSelectable = true;
            this.metroDelButton.Click += new System.EventHandler(this.metroDelButton_Click);
            // 
            // EventControler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 453);
            this.Controls.Add(this.metroDelButton);
            this.Controls.Add(this.metroMoveButton);
            this.Controls.Add(this.metroAddButton);
            this.Controls.Add(this.metroDateTime1);
            this.Controls.Add(this.metroLabel1);
            this.Name = "EventControler";
            this.ShowIcon = false;
            this.Text = "Události";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroDateTime metroDateTime1;
        private MetroFramework.Controls.MetroButton metroAddButton;
        private MetroFramework.Controls.MetroButton metroMoveButton;
        private MetroFramework.Controls.MetroButton metroDelButton;
    }
}