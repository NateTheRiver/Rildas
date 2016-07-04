using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RildasApp.Timetable
{
    public class Day
    {
        public List<Event> events;
        public DateTime date;

        public Day(DateTime date)
        {
            this.date = date;
            events = new List<Event>();
        }
        public Day(DateTime date, List<Event> events)
        {
            this.date = date;
            this.events = events;
        }

        public void DrawCell(int x, int y, int mouseX, int mouseY, Graphics g, int width = 100, int height = 100)
        {
            Pen pen = new Pen(Color.Gray);

            Font fontDate = new Font(new FontFamily("Arial"), (height > 70) ? 16 : 14, FontStyle.Regular, GraphicsUnit.Pixel);
            Font font = new Font(new FontFamily("Arial"), (height > 70) ? 12 : 10, FontStyle.Regular, GraphicsUnit.Pixel);

            SolidBrush metroBrush = new SolidBrush(Color.FromArgb(0, 174, 219));
            SolidBrush brush = new SolidBrush(Color.LightBlue);
            SolidBrush brushLate = new SolidBrush(Color.Red);

            StringFormat formatter = new StringFormat();
            formatter.LineAlignment = StringAlignment.Center;
            formatter.Alignment = StringAlignment.Center;

            Rectangle rec = new Rectangle(x + 2, y + 20, width - 2, height - 20);
            Rectangle cell = new Rectangle(x, y, width, height);

            if (cell.Contains(new Point(mouseX, mouseY)))
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(50, 50, 50)), cell);
            }
            else
            {
                if (date.Day == DateTime.Today.Day && date.Month == DateTime.Today.Month && date.Year == DateTime.Today.Year)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(35, 35, 40)), cell);
                }
                else
                    g.DrawRectangle(pen, cell);
            }
            g.DrawString(date.Day.ToString(), fontDate, metroBrush, x + 2, y + 2);
            for (int i = 0; i < events.Count; ++i)
            {
                SolidBrush hbrush = new SolidBrush(Color.Red);
                string text = "";
                switch (events[i].type)
                {
                    case Type.Correrction: hbrush = new SolidBrush(Color.Orange); text = "Korekce"; break;
                    case Type.Empty: hbrush = new SolidBrush(Color.LightBlue); text = "–"; break;
                    case Type.Encode: hbrush = new SolidBrush(Color.Yellow); text = "Encód"; break;
                    case Type.Other: hbrush = new SolidBrush(Color.Blue); text = "Rildas"; break;
                    case Type.Publish: hbrush = new SolidBrush(Color.Green); text = "Zveřejnění"; break;
                    case Type.Translate: hbrush = new SolidBrush(Color.Red); text = "Překlad"; break;
                }
                g.DrawString(text, font, hbrush, new Rectangle(rec.Location.X, rec.Location.Y - ((events.Count - 1) * 5) + i * 10, rec.Width, rec.Height), formatter);
            }
            /*if (events[0].nextDate == null)
                g.DrawString(text, font, brush, rec, formatter);
            else
                g.DrawString(text, font, brushLate, rec, formatter);*/
        }
    }
}
