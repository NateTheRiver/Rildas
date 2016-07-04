using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RildasApp.Models
{
    public class EpisodeVersion
    {
        public enum Type { PŘEKLAD, KOREKCE };
        public Type type;
        public int id;
        public int animeId;
        public int episode;
        public bool special;
        public int state;
        public string title;
        public string titleEN;
        public string name;
        public string nameEN;
        public string comment;
        public string timeOn;
        public int addedBy;
        public DateTime added;
        public int reservedBy;
        public string _hash; // Internal purposes of file recognition

    }
}
