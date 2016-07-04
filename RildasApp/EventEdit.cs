using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RildasApp
{
    public partial class EventEdit : MetroForm
    {
        public enum Type { New, Edit };
        Type type;
        Timetable.Event ev;
        List<Models.Anime> anime;
        List<Models.User> users;
        public EventEdit(DateTime day)
        {
            InitializeComponent();
            this.type = Type.New;
            users = new List<Models.User>();
            anime = new List<Models.Anime>();
            metroDateTime1.Value = day;
            // Není potřeba přepisovat datum u akce, která ještě neexistuje
            metroLabelNewDate.Visible = false;
            metroLabelNewTime.Visible = false;
            metroDateTimeNewDate.Visible = false;
            metroDateTimeNewTime.Visible = false;
            metroDateTime1.Enabled = true;
            metroDateTime2.Enabled = true;
            metroTextBoxDescription.Location = new Point(metroTextBoxDescription.Location.X, metroDateTime1.Location.Y);
            GetAnimeList();
            GetUserList();
            metroComboBoxAction.SelectedIndex = 0;
        }
        public EventEdit(Timetable.Event ev, bool readon = false)
        {
            InitializeComponent();
            this.type = Type.Edit;
            this.ev = ev;
            users = new List<Models.User>();
            anime = new List<Models.Anime>();
            GetAnimeList();
            GetUserList();
            metroTextBoxDescription.Text = ev.text;
            switch (ev.type)
            {
                case Timetable.Type.Correrction: metroComboBoxAction.SelectedIndex = 1; break;
                case Timetable.Type.Translate: metroComboBoxAction.SelectedIndex = 0; break;
                case Timetable.Type.Encode: metroComboBoxAction.SelectedIndex = 2; break;
                case Timetable.Type.Publish: metroComboBoxAction.SelectedIndex = 3; break;
                default: metroComboBoxAction.SelectedIndex = 4; break;
            }
            metroComboBoxUsers.SelectedIndex = 0;
            for (int i = 0; i < users.Count; ++i)
            {
                if (users[i].id == ev.userID)
                    metroComboBoxUsers.SelectedIndex = i + 1;
            }
            for (int i = 0; i < anime.Count; ++i)
            {
                if (anime[i].id == ev.animeID)
                    metroComboBoxAnime.SelectedIndex = i;
            }
            metroDateTime1.Value = ev.date;
            if (readon)
            {
                DisableAll();
                this.Text += "-pouze pro čtení";
            }
        }

        private void DisableAll()
        {
            metroButton1.Enabled = false;
            metroButton2.Enabled = false;
            metroComboBoxAction.Enabled = false;
            metroComboBoxAnime.Enabled = false;
            metroComboBoxUsers.Enabled = false;
            metroDateTime1.Enabled = false;
            metroDateTime2.Enabled = false;
            metroDateTimeNewDate.Enabled = false;
            metroDateTimeNewTime.Enabled = false;
            metroTextBoxDescription.Enabled = false;
        }
        public void GetAnimeList()
        {
            anime.Clear();
            metroComboBoxAnime.Items.Clear();
            var result = Global.GetAnimes();//Global.Query("SELECT id, name FROM anime ORDER BY name ASC");
            for (int i = 0; i < result.Count; ++i)
            {
                Models.Anime anim = new Models.Anime();
                anim.id = result[i].id;
                anim.name = result[i].name;
                anime.Add(anim);
                metroComboBoxAnime.Items.Add(anim.name);
            }
        }
        public void GetUserList()
        {
            users.Clear();
            metroComboBoxUsers.Items.Clear();
            metroComboBoxUsers.Items.Add("Všichni");
          // TODO: Opravit events
            /*  var result = Global.Query("SELECT id, username FROM users WHERE access > 1");
            for (int i = 0; i < result.GetLength(0); ++i)
            {
                Models.User user = new Models.User();
                user.id = Int16.Parse(result[i, 0]);
                user.username = result[i, 1];
                users.Add(user);
                metroComboBoxUsers.Items.Add(user.username);
            }*/
            metroComboBoxUsers.SelectedIndex = 0;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            int animeID, userID;
            string action = "";
            Int32 time = (Int32)metroDateTime1.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            switch (metroComboBoxAction.SelectedIndex)
            {
                case 0: action = "Translate"; break;
                case 1: action = "Correction"; break;
                case 2: action = "Encode"; break;
                case 3: action = "Publish"; break;
                case 4: action = "Other"; break;
            }
            userID = (metroComboBoxUsers.SelectedIndex == 0) ? 0 : users[metroComboBoxUsers.SelectedIndex - 1].id;
            animeID = (metroComboBoxAnime.SelectedIndex == -1) ? (0) : anime[metroComboBoxAnime.SelectedIndex].id;
            if (type == Type.New)
            {
                // TODO: Opravit events
               // Global.Query("INSERT INTO app_events (`type`, `added`, `deadline`, `anime_id`, `user_id`, `title`, `state`, `creatorID`) VALUES ('" + action + "', UNIX_TIMESTAMP(), '" + time + "', '" + animeID + "', '" + userID + "', '" + metroTextBoxDescription.Text + "', 'NEW', '" + Global.loggedUser.id + "')");
            }
            else
            {
                // TODO: Opravit events
                //Global.Query("UPDATE app_events SET `type`='" + action + "', `added`= UNIX_TIMESTAMP(), `deadline`='" + time + "', `anime_id`='" + animeID + "', `user_id`='" + userID + "', `title`='" + metroTextBoxDescription.Text + "', `state`='NEW' WHERE id='" + ev.id + "'");
            }
            this.Close();
        }

        private void metroComboBoxAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroComboBoxAction.SelectedIndex == 4)
            {
                metroComboBoxAnime.Visible = false;
                metroLabelAnime.Visible = false;
                metroComboBoxUsers.Enabled = false;
                metroComboBoxUsers.SelectedIndex = 0;
            }
            else
            {
                metroComboBoxAnime.Visible = true;
                metroLabelAnime.Visible = true;
                metroComboBoxUsers.Enabled = true;
            }
        }
    }
}
