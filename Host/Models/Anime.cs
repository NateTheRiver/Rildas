using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Models
{
    public class Anime
    {
        public static Status ParseStatus(string status)
        {
            switch (status)
            {
                case "PŘELOŽENO": return Status.PŘELOŽENO;
                case "PŘEKLÁDÁ SE": return Status.PŘEKLÁDÁ_SE;
                case "POZASTAVENO": return Status.POZASTAVENO;
            }
            return Status.PŘELOŽENO;
        }
        public int id;
        public string name;
        public string filename;
        public string animelist_img;
        public string post_img;
        public string banner_img;
        public string banner_text;
        public bool banner_show;
        public int release_year;
        public int ep_count;
        public int special_ep_count;
        public string plot;
        public int age;
        public int translatorid;
        public string imagepath;
        public enum Status { PŘELOŽENO, PŘEKLÁDÁ_SE, POZASTAVENO }
        public Status status;
        internal int correctorid;
    }
}
