using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Models
{
    public class Notification
    {
        public int id;
        public int userId;
        public string header;
        public string text;
        public DateTime time;
    }
}
