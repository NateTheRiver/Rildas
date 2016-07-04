using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RildasApp.Models
{
    class EventFile
    {
        public int id;
        public int anime_id;
        public int episode_id;
        public int ready;
        public string title;
        public string title_aj;
        public string name;
        public string name_aj;
        public int addTime;
        public int creator_id;
        public int reservedBy;
        public int timeOn;

        Tuple<int, string>[] comments;
    }
}
