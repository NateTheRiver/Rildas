using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Logger
    {
        public static void Log(string message)
        {
            Log(message, SEVERITY.MESSAGE);
        }
        public enum SEVERITY { INFO, MESSAGE, SUCCESS, ERROR, FATAL};
        public static void Log(string message, SEVERITY sever)
        {
            Console.WriteLine(message);
            try
            {
                string severity = "[";
                switch(sever)
                {
                    case SEVERITY.ERROR: severity += "ERROR"; break;
                    case SEVERITY.FATAL: severity += "FATAL"; break;
                    case SEVERITY.INFO: severity += "INFO"; break;
                    case SEVERITY.MESSAGE: severity += "MESSAGE"; break;
                    case SEVERITY.SUCCESS: severity += "SUCCESS"; break;
                }
                severity += "] ";
                File.AppendAllText("/var/www/anime/service/app_log.txt", severity + message + Environment.NewLine);
            }
            catch (Exception e)
            {
                Console.WriteLine("INTERNAL LOGGING ERROR: " + e.Message);
            }
        }
    }
}
