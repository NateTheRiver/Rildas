using Host.DataParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class ConnectionManager
    {
        public static List<Client> Connections = new List<Client>();
        public static void AddClient(Client e)
        {
            Connections.Add(e);
            e.Recieved += Client_Recieved;
        }
        public static void SendToAll(string message)
        {
            foreach(Client client in Connections)
            {
                client.Send(message);
            }
        }

        public static void RemoveClient(Client e)
        {
            try
            {
                Connections.Remove(e);
                foreach (Client client in Connections)
                {
                    client.Send("CHAT_USER_DISCONNECTED_" + e.loggedUser.id);
                }
            }
            catch(Exception ex)
            {
                Logger.Log("An error occured while disconnecting a client. Ex: " + ex.Message, Logger.SEVERITY.ERROR);
            }
        }
        private static void Client_Recieved(Client sender, string command)
        {

            if(command.Contains("CLIENT_LOGIN")) Logger.Log(String.Format("Received login command from {1}.", command, sender.sck.RemoteEndPoint), Logger.SEVERITY.MESSAGE);
            else Logger.Log(String.Format("Received {0} from {1}.", command, sender.sck.RemoteEndPoint), Logger.SEVERITY.MESSAGE);

            if (command == "") return;
            string[] splitData = command.Split('_');
            try
            {
                IParser parser = ParserFactory.GetParser(splitData[0]);
                parser.ParseData(sender, splitData.Skip(1).ToArray());
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Data parsing failed. Data: {0} Ex: {1}.", command, ex.Message), Logger.SEVERITY.ERROR);

            }

        }
    }
}
