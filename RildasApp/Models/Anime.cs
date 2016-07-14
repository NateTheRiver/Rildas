using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RildasApp.Models
{
    public class Anime
    {
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
        internal int correctorid;
        public string imagepath;
        public enum Status { PŘELOŽENO, PŘEKLÁDÁ_SE, POZASTAVENO }
        public Status status;
    }
}
