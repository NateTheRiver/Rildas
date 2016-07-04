using MetroFramework.Forms;
using RildasApp.HelpObject;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RildasApp
{
    public partial class EventControler : MetroForm
    {
        Timetable.Day day;
        EventEdit edit;
        MyPanel panel;
        int selected = 0;
        Timetable.Timetable table;
        int x, y;

        const int eventHeight = 30;
        public EventControler(ref Timetable.Day day, Timetable.Timetable table)
        {
            InitializeComponent();
            this.day = day;
            this.table = table;
            //if(day.date <= metroDateTime1.MaxDate && day.date >= metroDateTime1.MinDate)
            metroDateTime1.Value = day.date;
            panel = new MyPanel();
            panel.Location = new Point(5, 100);
            panel.Size = new Size(this.Width - 10, this.Height - 160);
            panel.Paint += new PaintEventHandler(this.metroPanel1_Paint);
            panel.MouseClick += new MouseEventHandler(this.metroPanel_Click);
            panel.MouseDoubleClick += new MouseEventHandler(this.metroPanel_DoubleClick);
            panel.MouseMove += new MouseEventHandler(this.metroPanel_MouseMove);
            panel.MouseLeave += new EventHandler(metroPanel_MouseLeave);
            this.Controls.Add(panel);
        }

        private void edit_Closed(object sender, FormClosedEventArgs e)
        {
            table.LoadEvents();
            this.day = table.GetDay(metroDateTime1.Value);
        }

        private void metroPanel_MouseLeave(object sender, EventArgs e)
        {
            x = 0;
            y = selected * eventHeight;
        }

        private void metroPanel_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            panel.Refresh();
        }

        private void metroPanel_DoubleClick(object sender, MouseEventArgs e)
        {
            /*if (day.events == null || day.events.Count == 0) return;
            MessageBox.Show(day.events[selected].text);*/
            if (edit == null || edit.IsDisposed)
            {
                edit = new EventEdit(day.events[selected], true);
                edit.FormClosed += new FormClosedEventHandler(edit_Closed);
                edit.Show();
            }
        }

        private void metroPanel_Click(object sender, MouseEventArgs e)
        {
            if (day.events == null || day.events.Count == 0) return;
            selected = y / eventHeight;
            if (selected >= day.events.Count)
                selected = day.events.Count - 1;
            if (Global.loggedUser.access < 5 && Global.loggedUser.id != day.events[selected].userID)
            {
                metroMoveButton.Enabled = false;
                metroDelButton.Enabled = false;
            }
            else
            {
                metroMoveButton.Enabled = true;
                metroDelButton.Enabled = true;
            }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {
            if (day.events == null || day.events.Count == 0) return;
            Color WaitToTranslate = Color.FromArgb(150, 0, 0);
            Color WaitToCorrect = Color.FromArgb(150, 90, 0);
            Color WaitToEncode = Color.FromArgb(150, 150, 0);
            Color WaitToPublish = Color.FromArgb(0, 104, 0);
            Color Other = Color.FromArgb(0, 94, 200);

            for (int i = 0; i < day.events.Count; ++i)
            {
                SolidBrush useColor;
                if (y / eventHeight == i || i == selected)
                {
                    switch (day.events[i].type)
                    {
                        case Timetable.Type.Correrction: useColor = new SolidBrush(Color.Orange); break;
                        case Timetable.Type.Translate: useColor = new SolidBrush(Color.Red); break;
                        case Timetable.Type.Encode: useColor = new SolidBrush(Color.Yellow); break;
                        case Timetable.Type.Publish: useColor = new SolidBrush(Color.Green); break;
                        default: useColor = new SolidBrush(Color.Blue); break;
                    }
                }
                else
                {
                    switch (day.events[i].type)
                    {
                        case Timetable.Type.Correrction: useColor = new SolidBrush(WaitToCorrect); break;
                        case Timetable.Type.Translate: useColor = new SolidBrush(WaitToTranslate); break;
                        case Timetable.Type.Encode: useColor = new SolidBrush(WaitToEncode); break;
                        case Timetable.Type.Publish: useColor = new SolidBrush(WaitToPublish); break;
                        default: useColor = new SolidBrush(Other); break;
                    }
                }
                e.Graphics.FillRectangle(useColor, 0, eventHeight * i, panel.Width, eventHeight);
                e.Graphics.DrawString(day.events[i].username + ": " + day.events[i].text, new Font("Arial", 12, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 15, 15, 15)), new Point(0, eventHeight * i + 5));
            }
        }

        private void metroAddButton_Click(object sender, EventArgs e)
        {
            if (edit == null || edit.IsDisposed)
            {
                edit = new EventEdit(day.date);
                edit.FormClosed += new FormClosedEventHandler(edit_Closed);
                edit.Show();
            }
        }

        private void metroMoveButton_Click(object sender, EventArgs e)
        {
            if (edit == null || edit.IsDisposed)
            {
                edit = new EventEdit(day.events[selected]);
                edit.FormClosed += new FormClosedEventHandler(edit_Closed);
                edit.Show();
            }
        }

        private void metroDelButton_Click(object sender, EventArgs e)
        {
            if (day.events.Count > 0 && selected < day.events.Count)
            {
                // TODO: Opravit Events
                //Global.Query("UPDATE app_events SET state='ABORTED' WHERE id='" + day.events[selected].id + "'");
                MessageBox.Show("Událost byla zrušena");
                table.LoadEvents();
                this.day = table.GetDay(metroDateTime1.Value);
            }
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            this.day = table.GetDay(metroDateTime1.Value);
        }
        private bool CheckEvents()
        {
            if ((edit == null || edit.IsDisposed)) selected = -1;
            return (edit == null || edit.IsDisposed);
        }
    }
}
