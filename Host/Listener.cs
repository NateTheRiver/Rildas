using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    public class Listener
    {
        Socket s;
        public bool Listening
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }
        public Listener(int port)
        {
            this.Port = port;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Start()
        {
            if (Listening) return;
            try
            {
                s.Bind(new IPEndPoint(0, Port));
                s.Listen(0);

                s.BeginAccept(callback, null);
                Listening = true;
                Logger.Log("Server started successfully", Logger.SEVERITY.MESSAGE);
            }
            catch (Exception e)
            {
                 Logger.Log(e.Message, Logger.SEVERITY.ERROR);
            }
        }
        public void Stop()
        {
            if (!Listening) return;

            s.Close();
            s.Dispose();
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Logger.Log("Server stopped successfully", Logger.SEVERITY.MESSAGE);
        }

        void callback(IAsyncResult ar)
        {
            try
            {
                Logger.Log("socket accepted", Logger.SEVERITY.INFO);
                Socket s = this.s.EndAccept(ar);
                if (SocketAccepted != null)
                {
                    SocketAccepted(s);
                }
                this.s.BeginAccept(callback, null);
            }
            catch (Exception e)
            {
                Logger.Log(e.Message, Logger.SEVERITY.ERROR);
            }

        }
        public delegate void SocketAcceptedHandler(Socket e);
        public event SocketAcceptedHandler SocketAccepted;

    }
}
