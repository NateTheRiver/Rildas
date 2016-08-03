using Host.DataParsers;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;

namespace Host
{
    static class ParserFactory
    {
        static private List<IParser> parsers
        {
            get; set;
        }
        static ParserFactory()
        {
            try
            {
                parsers = new List<IParser>();
                Assembly mscorlib = typeof(IParser).Assembly;
                foreach (Type type in mscorlib.GetTypes())
                {
                    if(type.GetInterfaces().FirstOrDefault(x => x.FullName == typeof(IParser).FullName) != null) parsers.Add(Activator.CreateInstance(type) as IParser);
                }

                
            }
            catch (Exception e)
            {

                Logger.Log("Failed to compose. Ex: " + e.Message);
            }
        }
            
        public static IParser GetParser(string id)
        {
            foreach (IParser parser in parsers)
            {
                if (parser.GetParserName() == id)
                    return parser;
            }
            
            throw new Exception(String.Format("Parser {0} not found.", id));
        }
    }
}
