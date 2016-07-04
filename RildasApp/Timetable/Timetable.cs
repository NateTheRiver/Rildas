using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RildasApp.Timetable
{
    public class Timetable
    {
        List<Day> ev; //Více událostí na jeden den
        MetroFramework.Controls.MetroButton leftButton;
        MetroFramework.Controls.MetroButton rightButton;
        Panel sender;
        int tableWidth;
        int tableHeight;
        ToolTip tip;
        DateTime firstDay;
        int year;
        int month;

        const int visibleDays = 42;
        const int rowCount = 7;
        const int padding = 50;
        const int controlPanelHeight = 50;

        public Timetable(Panel sender, ToolTip tip, int width = 800, int height = 600)
        {
            //this.events = events;
            tableWidth = width;
            tableHeight = height;
            this.tip = tip;
            this.year = DateTime.Today.Year;
            this.month = DateTime.Today.Month;
            this.sender = sender;
            ev = new List<Day>();
            LoadEvents();
            //Tlačítka na posun měsíce
            leftButton = new MetroFramework.Controls.MetroButton();
            rightButton = new MetroFramework.Controls.MetroButton();

            leftButton.Location = new System.Drawing.Point(tableWidth - 80, tableHeight - 35);
            leftButton.Name = "metroButton6";
            leftButton.Size = new System.Drawing.Size(30, 30);
            leftButton.Text = "<";
            leftButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            leftButton.UseSelectable = true;
            leftButton.Click += new System.EventHandler(this.left_Click);

            rightButton.Location = new System.Drawing.Point(tableWidth - 40, tableHeight - 35);
            rightButton.Name = "metroButton7";
            rightButton.Size = new System.Drawing.Size(30, 30);
            rightButton.Text = ">";
            rightButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            rightButton.UseSelectable = true;
            rightButton.Click += new System.EventHandler(this.right_Click);

            sender.Controls.Add(leftButton);
            sender.Controls.Add(rightButton);
        }

        public void LoadEvents()
        {
            ev.Clear();
            List<Event> events = GetEvents();
            foreach (Event e in events)
            {
                AddEvent(e);
            }
        }

        private List<Event> GetEvents()
        {
            List<Event> events = new List<Event>();
            // TODO: Opravit Events
            /*var result = Global.Query("SELECT app_events.id,`type`,`added`,`deadline`,`anime_id`,`user_id`,`file_id`,`title`,`description`,`lateDeadline`, app_events.state, `creatorID`, anime.name, users.username FROM `app_events` LEFT OUTER JOIN anime ON (anime.id = app_events.anime_id) LEFT OUTER JOIN users ON (app_events.user_id = users.id) WHERE app_events.state='NEW' OR app_events.state='UNFINISHED'");
            Type type;
            for (int i = 0; i < result.GetLength(0); ++i)
            {
                switch (result[i, 1])
                {
                    case "Translate": type = Type.Translate; break;
                    case "Correction": type = Type.Correrction; break;
                    case "Encode": type = Type.Encode; break;
                    case "Publish": type = Type.Publish; break;
                    default: type = Type.Other; break;
                }
                Event e = new Event(UnixTimeStampToDateTime(Double.Parse(result[i, 3])), Int16.Parse(result[i, 5]), Int16.Parse(result[i, 4]), result[i, 7], type);
                e.animeName = (result[i, 12]);
                e.username = result[i, 13];
                e.id = Int16.Parse(result[i, 0]);
                events.Add(e);
            }*/
            return events;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void right_Click(object sender, EventArgs e)
        {
            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }
            this.sender.Refresh();
        }

        private void left_Click(object sender, EventArgs e)
        {
            month--;
            if (month < 1)
            {
                month = 12;
                year--;
            }
            this.sender.Refresh();
        }

        public void AddEvent(Event e1)
        {
            for (int i = ev.Count - 1; i >= 0; --i)
            {
                if (CompareDays(ev[i].date, e1.date))
                {
                    ev[i].events.Add(e1);
                    return;
                }
            }
            ev.Add(new Day(e1.date));
            AddEvent(e1);
        }

        public void MakeTable(Graphics g, int mouseX, int mouseY)
        {
            //SortEvents();                     Je potřeba nejdříve sesortovat eventy podle data od nejstaršího
            //Více událostí na jeden den?
            ev.Sort(SortEvent);
            DateTime today = new DateTime(year, month, 1);
            while (today.Month == month || today.DayOfWeek != DayOfWeek.Monday)
            {
                today = today.AddDays(-1);
            }
            firstDay = today;
            List<Day> tmp = new List<Day>();
            tmp.AddRange(ev.ToArray());
            double tmpe = 1.0 * (tableHeight - padding - controlPanelHeight) / (visibleDays / rowCount);
            int cellSize = (int)Math.Round(tmpe, MidpointRounding.AwayFromZero);

            Font font = new Font(new FontFamily("Arial"), 16, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush brush = new SolidBrush(Color.LightBlue);

            for (int i = 0; i < visibleDays; ++i)
            {
                if (i < rowCount)
                {
                    Rectangle rec = new Rectangle(i * cellSize, padding / 2, cellSize, (int)g.MeasureString(String.Format("{0: ddd}", today), font).Height);
                    StringFormat formatter = new StringFormat();
                    formatter.LineAlignment = StringAlignment.Center;
                    formatter.Alignment = StringAlignment.Center;
                    string nameDay = String.Format("{0:ddd}", today);

                    //Nevím, jak to mají systémy s nastavenou primární češtinou
                    switch (nameDay)
                    {
                        case "Mon": nameDay = "Po"; break;
                        case "Tue": nameDay = "Út"; break;
                        case "Wed": nameDay = "St"; break;
                        case "Thu": nameDay = "Čt"; break;
                        case "Fri": nameDay = "Pá"; break;
                        case "Sat": nameDay = "So"; break;
                        case "Sun": nameDay = "Ne"; break;
                    }

                    g.DrawString(nameDay, font, brush, rec, formatter);
                }
                if (i == visibleDays - 15)
                {
                    string monthName = String.Format("{0:MMMM}", today);

                    switch (monthName)
                    {
                        case "January": monthName = "Leden"; break;
                        case "February": monthName = "Únor"; break;
                        case "March": monthName = "Březen"; break;
                        case "April": monthName = "Duben"; break;
                        case "May": monthName = "Květen"; break;
                        case "June": monthName = "Červen"; break;
                        case "July": monthName = "Červenec"; break;
                        case "August": monthName = "Srpen"; break;
                        case "September": monthName = "Září"; break;
                        case "October": monthName = "Říjen"; break;
                        case "November": monthName = "Listopad"; break;
                        case "December": monthName = "Prosinec"; break;
                    }

                    string stmp = String.Format("{0:yyyy}" + " " + monthName, today);

                    g.DrawString(stmp, font, brush, 0, padding + 10 + (((i + 14) / rowCount) + 1) * cellSize);
                }
                //TODO: Výpis několika akcí na jeden den
                //Předělat, pokud bude dost eventů, tak se začně sekat + nevypisuje, pokud eventy dojdou.
                if (tmp.Count > 0 && CompareDays(today, tmp[0].date))
                {
                    tmp[0].DrawCell((i % rowCount) * cellSize, padding + (i / rowCount) * cellSize, mouseX, mouseY, g, cellSize, cellSize);
                    tmp.RemoveAt(0);
                }
                else
                {
                    Day tmpDay = new Day(today, new List<Event> { new Event(today, 0, 0, "—", Type.Empty) });
                    tmpDay.DrawCell((i % rowCount) * cellSize, padding + (i / rowCount) * cellSize, mouseX, mouseY, g, cellSize, cellSize);
                }
                today = today.AddDays(1);
            }
        }

        /*public void MoveEvent(Event e1, Event e2)
        {
            Event tmp = null;
            if (e1 != null && e2 != null && e1.date.CompareTo(e2.date) < 0)
                for (int i = 0; i < ev.Count; ++i)
                {
                    if (ev[i].date.CompareTo(e1.date) == 0)
                    {
                        tmp = new Event(ev[i].events[0].date, ev[i].events[0].user, ev[i].events[0].text);
                        tmp.date = e2.date;
                        AddEvent(tmp);
                        ev[i].events[0].nextDate = e2;
                        break;
                    }
                }
        }*/

        private int SortEvent(Day d1, Day d2)
        {
            return d1.date.CompareTo(d2.date);
        }
        private bool CompareDays(DateTime d1, DateTime d2)
        {
            return (d1.Year == d2.Year && d1.Month == d2.Month && d1.Day == d2.Day);
        }

        public List<Event> GetEvent(int x, int y)
        {
            y -= padding;
            double tmpe = 1.0 * (tableHeight - padding - controlPanelHeight) / (visibleDays / rowCount);
            int cellSize = (int)Math.Round(tmpe, MidpointRounding.AwayFromZero);
            int dayFromToday = (y / cellSize) * rowCount + (x / cellSize);

            DateTime t = firstDay;
            t = t.AddDays(dayFromToday);
            List<Event> ret = new List<Event>();

            for (int i = 0; i < ev.Count; ++i)
            {
                if (ev[i].date.CompareTo(t) == 0)
                {
                    for (int z = 0; z < ev[i].events.Count; ++z)
                    {
                        ret.Add(ev[i].events[z]);
                    }
                    break;
                }
            }
            if (ret.Count > 0)
                return ret;
            else
                return new List<Event>() { new Event(t, 0, 0, "", Type.Other) };
        }
        public Day GetDay(int x, int y)
        {
            y -= padding;
            double tmpe = 1.0 * (tableHeight - padding - controlPanelHeight) / (visibleDays / rowCount);
            int cellSize = (int)Math.Round(tmpe, MidpointRounding.AwayFromZero);
            int dayFromToday = (y / cellSize) * rowCount + (x / cellSize);

            DateTime t = firstDay;
            t = t.AddDays(dayFromToday);
            List<Event> ret = new List<Event>();

            for (int i = 0; i < ev.Count; ++i)
            {
                if (CompareDays(ev[i].date, t))
                {
                    /*EventControler controle = new EventControler();
                    controle.Show();*/
                    return ev[i];
                }
            }
            return new Day(new DateTime(t.Year, t.Month, t.Day));
        }
        public Day GetDay(DateTime date)
        {
            for (int i = 0; i < ev.Count; ++i)
            {
                if (ev[i].date.Year == date.Year && ev[i].date.Month == date.Month && ev[i].date.Day == date.Day)
                    return ev[i];
            }
            return new Day(date);
        }
    }
}
