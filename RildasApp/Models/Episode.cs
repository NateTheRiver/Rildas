using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RildasApp.Models
{
    public enum state { Not_ready, Korekce, Potvrzeni, Final, Done };

    public class Episode
    {
        public int id;
        public int ep_number;
        public bool special;
        public string name;
        public string link_mega;
        public string link_ulozto;
        public string link_online;
        public bool email_notification;
        public bool visible;
        public int animeid;
        public DateTime lastEditTime;
        public state epState;
        public User[] translators;
        public User[] correctors;
    }
}
