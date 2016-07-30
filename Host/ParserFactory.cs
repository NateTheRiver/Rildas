using Host.DataParsers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    static class ParserFactory
    {
        static private IEnumerable<IParser> parsers
        {
            get; set;
        }
        static ParserFactory()
        {
            var registrationBuilder = new RegistrationBuilder();
            registrationBuilder.ForTypesDerivedFrom<IParser>().ExportInterfaces();
            var assemblyCatalog = new AssemblyCatalog(typeof(IParser).Assembly, registrationBuilder);
            var compositionContainer = new CompositionContainer(assemblyCatalog);
            parsers = compositionContainer.GetExportedValues<IParser>();
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
