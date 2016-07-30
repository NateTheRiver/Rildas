namespace RildasApp.Forms
{
    partial class EventEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventEdit));
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.metroComboBoxAction = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabelNewDate = new MetroFramework.Controls.MetroLabel();
            this.metroDateTime1 = new MetroFramework.Controls.MetroDateTime();
            this.metroDateTimeNewDate = new MetroFramework.Controls.MetroDateTime();
            this.metroComboBoxUsers = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxDescription = new MetroFramework.Controls.MetroTextBox();
            this.metroLabelNewTime = new MetroFramework.Controls.MetroLabel();
            this.metroDateTime2 = new MetroFramework.Controls.MetroDateTime();
            this.metroDateTimeNewTime = new MetroFramework.Controls.MetroDateTime();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroComboBoxAnime = new MetroFramework.Controls.MetroComboBox();
            this.metroLabelAnime = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(336, 282);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(130, 41);
            this.metroButton1.TabIndex = 0;
            this.metroButton1.Text = "Proveď";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // metroComboBoxAction
            // 
            this.metroComboBoxAction.FormattingEnabled = true;
            this.metroComboBoxAction.ItemHeight = 23;
            this.metroComboBoxAction.Items.AddRange(new object[] {
            "Překlad",
            "Korekce",
            "Encód",
            "Zveřejnění",
            "Rildas akce"});
            this.metroComboBoxAction.Location = new System.Drawing.Point(80, 150);
            this.metroComboBoxAction.Name = "metroComboBoxAction";
            this.metroComboBoxAction.Size = new System.Drawing.Size(130, 29);
            this.metroComboBoxAction.TabIndex = 1;
            this.metroComboBoxAction.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroComboBoxAction.UseSelectable = true;
            this.metroComboBoxAction.SelectedIndexChanged += new System.EventHandler(this.metroComboBoxAction_SelectedIndexChanged);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 82);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(51, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Datum:";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabelNewDate
            // 
            this.metroLabelNewDate.AutoSize = true;
            this.metroLabelNewDate.Location = new System.Drawing.Point(245, 82);
            this.metroLabelNewDate.Name = "metroLabelNewDate";
            this.metroLabelNewDate.Size = new System.Drawing.Size(85, 19);
            this.metroLabelNewDate.TabIndex = 2;
            this.metroLabelNewDate.Text = "Nové datum:";
            this.metroLabelNewDate.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroDateTime1
            // 
            this.metroDateTime1.CustomFormat = "dd.MM.yyyy";
            this.metroDateTime1.Enabled = false;
            this.metroDateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.metroDateTime1.Location = new System.Drawing.Point(80, 80);
            this.metroDateTime1.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTime1.Name = "metroDateTime1";
            this.metroDateTime1.Size = new System.Drawing.Size(130, 29);
            this.metroDateTime1.TabIndex = 3;
            this.metroDateTime1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroDateTimeNewDate
            // 
            this.metroDateTimeNewDate.CustomFormat = "dd.MM.yyyy";
            this.metroDateTimeNewDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.metroDateTimeNewDate.Location = new System.Drawing.Point(336, 80);
            this.metroDateTimeNewDate.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTimeNewDate.Name = "metroDateTimeNewDate";
            this.metroDateTimeNewDate.Size = new System.Drawing.Size(130, 29);
            this.metroDateTimeNewDate.TabIndex = 3;
            this.metroDateTimeNewDate.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroComboBoxUsers
            // 
            this.metroComboBoxUsers.FormattingEnabled = true;
            this.metroComboBoxUsers.ItemHeight = 23;
            this.metroComboBoxUsers.Items.AddRange(new object[] {
            "Všechny"});
            this.metroComboBoxUsers.Location = new System.Drawing.Point(80, 185);
            this.metroComboBoxUsers.Name = "metroComboBoxUsers";
            this.metroComboBoxUsers.Size = new System.Drawing.Size(130, 29);
            this.metroComboBoxUsers.TabIndex = 4;
            this.metroComboBoxUsers.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroComboBoxUsers.UseSelectable = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(23, 185);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(37, 19);
            this.metroLabel3.TabIndex = 5;
            this.metroLabel3.Text = "Pro: ";
            this.metroLabel3.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroTextBoxDescription
            // 
            // 
            // 
            // 
            this.metroTextBoxDescription.CustomButton.Image = null;
            this.metroTextBoxDescription.CustomButton.Location = new System.Drawing.Point(122, 2);
            this.metroTextBoxDescription.CustomButton.Name = "";
            this.metroTextBoxDescription.CustomButton.Size = new System.Drawing.Size(95, 95);
            this.metroTextBoxDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxDescription.CustomButton.TabIndex = 1;
            this.metroTextBoxDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxDescription.CustomButton.UseSelectable = true;
            this.metroTextBoxDescription.CustomButton.Visible = false;
            this.metroTextBoxDescription.Lines = new string[] {
        "Popis:"};
            this.metroTextBoxDescription.Location = new System.Drawing.Point(246, 150);
            this.metroTextBoxDescription.MaxLength = 32767;
            this.metroTextBoxDescription.Multiline = true;
            this.metroTextBoxDescription.Name = "metroTextBoxDescription";
            this.metroTextBoxDescription.PasswordChar = '\0';
            this.metroTextBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDescription.SelectedText = "";
            this.metroTextBoxDescription.SelectionLength = 0;
            this.metroTextBoxDescription.SelectionStart = 0;
            this.metroTextBoxDescription.Size = new System.Drawing.Size(220, 100);
            this.metroTextBoxDescription.TabIndex = 6;
            this.metroTextBoxDescription.Text = "Popis:";
            this.metroTextBoxDescription.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTextBoxDescription.UseSelectable = true;
            this.metroTextBoxDescription.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxDescription.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabelNewTime
            // 
            this.metroLabelNewTime.AutoSize = true;
            this.metroLabelNewTime.Location = new System.Drawing.Point(245, 117);
            this.metroLabelNewTime.Name = "metroLabelNewTime";
            this.metroLabelNewTime.Size = new System.Drawing.Size(64, 19);
            this.metroLabelNewTime.TabIndex = 2;
            this.metroLabelNewTime.Text = "Nový čas:";
            this.metroLabelNewTime.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroDateTime2
            // 
            this.metroDateTime2.CustomFormat = "dd.MM.yyyy";
            this.metroDateTime2.Enabled = false;
            this.metroDateTime2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.metroDateTime2.Location = new System.Drawing.Point(80, 115);
            this.metroDateTime2.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTime2.Name = "metroDateTime2";
            this.metroDateTime2.Size = new System.Drawing.Size(130, 29);
            this.metroDateTime2.TabIndex = 3;
            this.metroDateTime2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroDateTime2.Value = new System.DateTime(2016, 2, 24, 0, 0, 0, 0);
            // 
            // metroDateTimeNewTime
            // 
            this.metroDateTimeNewTime.CustomFormat = "dd.MM.yyyy";
            this.metroDateTimeNewTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.metroDateTimeNewTime.Location = new System.Drawing.Point(336, 115);
            this.metroDateTimeNewTime.MinimumSize = new System.Drawing.Size(0, 29);
            this.metroDateTimeNewTime.Name = "metroDateTimeNewTime";
            this.metroDateTimeNewTime.Size = new System.Drawing.Size(130, 29);
            this.metroDateTimeNewTime.TabIndex = 3;
            this.metroDateTimeNewTime.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(23, 150);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(40, 19);
            this.metroLabel6.TabIndex = 7;
            this.metroLabel6.Text = "Akce:";
            this.metroLabel6.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroButton2
            // 
            this.metroButton2.Location = new System.Drawing.Point(80, 282);
            this.metroButton2.Name = "metroButton2";
            this.metroButton2.Size = new System.Drawing.Size(130, 41);
            this.metroButton2.TabIndex = 8;
            this.metroButton2.Text = "Přiložit soubor";
            this.metroButton2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroButton2.UseSelectable = true;
            // 
            // metroComboBoxAnime
            // 
            this.metroComboBoxAnime.FormattingEnabled = true;
            this.metroComboBoxAnime.ItemHeight = 23;
            this.metroComboBoxAnime.Location = new System.Drawing.Point(80, 221);
            this.metroComboBoxAnime.Name = "metroComboBoxAnime";
            this.metroComboBoxAnime.Size = new System.Drawing.Size(130, 29);
            this.metroComboBoxAnime.TabIndex = 9;
            this.metroComboBoxAnime.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroComboBoxAnime.UseSelectable = true;
            // 
            // metroLabelAnime
            // 
            this.metroLabelAnime.AutoSize = true;
            this.metroLabelAnime.Location = new System.Drawing.Point(24, 221);
            this.metroLabelAnime.Name = "metroLabelAnime";
            this.metroLabelAnime.Size = new System.Drawing.Size(50, 19);
            this.metroLabelAnime.TabIndex = 10;
            this.metroLabelAnime.Text = "Anime:";
            this.metroLabelAnime.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // EventEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 346);
            this.Controls.Add(this.metroLabelAnime);
            this.Controls.Add(this.metroComboBoxAnime);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroLabel6);
            this.Controls.Add(this.metroTextBoxDescription);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroComboBoxUsers);
            this.Controls.Add(this.metroDateTimeNewTime);
            this.Controls.Add(this.metroDateTime2);
            this.Controls.Add(this.metroDateTimeNewDate);
            this.Controls.Add(this.metroLabelNewTime);
            this.Controls.Add(this.metroDateTime1);
            this.Controls.Add(this.metroLabelNewDate);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.metroComboBoxAction);
            this.Controls.Add(this.metroButton1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EventEdit";
            this.Text = "Událost";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroComboBox metroComboBoxAction;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabelNewDate;
        private MetroFramework.Controls.MetroDateTime metroDateTime1;
        private MetroFramework.Controls.MetroDateTime metroDateTimeNewDate;
        private MetroFramework.Controls.MetroComboBox metroComboBoxUsers;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDescription;
        private MetroFramework.Controls.MetroLabel metroLabelNewTime;
        private MetroFramework.Controls.MetroDateTime metroDateTime2;
        private MetroFramework.Controls.MetroDateTime metroDateTimeNewTime;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroComboBox metroComboBoxAnime;
        private MetroFramework.Controls.MetroLabel metroLabelAnime;
    }
}