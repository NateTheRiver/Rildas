using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Models
{
    public class ApplicationVersion
    {
        public int id;
        public string version;
        public string downloadLocation;
        public bool isStable;
        public string description;
        public DateTime releaseDate;
        public bool isVisible;
    }
}
