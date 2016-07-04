using System;
using System.Windows.Forms;
using System.Drawing;

namespace RildasApp.Timetable
{
    public enum Type { Translate, Correrction, Encode, Publish, Other, Empty };
    public class Event
    {
        public int id;
        public Type type;
        public DateTime date;
        public Event nextDate;
        public int userID;
        public string username;
        public string animeName;
        public int animeID;
        public string text;

        public Event(DateTime date, int userID, int animeID, string text, Type type)
        {
            this.date = date;
            this.userID = userID;
            this.animeID = animeID;
            this.text = text;
            this.type = type;
        }
    }
}
