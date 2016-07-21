using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

using System.ServiceModel.Description;
using System.Threading;

namespace Host
{
    class Program
    {
        public static void Main(string[] args)
        {
            GlobalData.Init();
            Listener list = new Listener(51200);
            list.SocketAccepted += List_SocketAccepted;
            list.Start();
            Thread packageAutoUpdate = new Thread(AutoXDCCUpdater.DownloadAndParse);
            packageAutoUpdate.Start();
            while (true) Console.ReadLine();
        }

        private static void List_SocketAccepted(System.Net.Sockets.Socket e)
        {
            Client client = new Client(e);
            client.Disconnected += Client_Disconnected;
            ConnectionManager.AddClient(client);
        }

        private static void Client_Disconnected(Client sender)
        {
            ConnectionManager.RemoveClient(sender);
        }


    }
}
