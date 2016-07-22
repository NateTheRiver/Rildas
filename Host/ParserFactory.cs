using Host.DataParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class ParserFactory
    {
        public static IParser GetParser(string id)
        {
            switch(id)
            {
                case "CLIENT": return ClientParser.Instance;
                case "GETDATA": return GetDataParser.Instance;
                case "CHANGEDATA": return ChangeDataParser.Instance;
                case "FILE": return FileParser.Instance;
                case "CHAT": return ChatParser.Instance;
            }
            
            throw new Exception(String.Format("Parser {0} not found.", id));
        }
    }
}
