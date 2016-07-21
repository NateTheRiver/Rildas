using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Models
{
    class Event
    {
        public int id;
        public int addTime;
        public int deadline;
        public int anime_id;
        public int user_id;
        public int file_id;
        public string description;
        public string title;
        public int lateDeadline;
        public int creator_id;
        public state actualState;
        public enum state { NEW, UNFINISHED, ABORTED, DONE }
    }
}
