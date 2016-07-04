using RildasApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RildasApp
{
    public static class Serializer
    {
        static public string Serialize(object ch)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(ch);
        }
        public static T Deserialize<T>(string text)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text);

        }

    }
}
