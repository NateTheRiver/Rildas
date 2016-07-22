using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Host.Models;
using System.Text.RegularExpressions;

namespace Host
{
    public class Client
    {
        public User loggedUser;
        private string actualBuffer;
        public IPEndPoint Endpoint
        {
            get;
            private set;
        }
        public Socket sck;
        public Client(Socket accepted)
        {
            Logger.Log("Socket accepted. Endpoint:" + accepted.RemoteEndPoint, Logger.SEVERITY.MESSAGE);
            sck = accepted;
            Endpoint = (IPEndPoint)sck.RemoteEndPoint;
            this.Send("CLIENT_CONNECTION_READY_" + Serializer.Serialize(GlobalData.GetCurrentVersion(true, true)));
            sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
        }

        void callback(IAsyncResult ar)
        {
            try
            {
                sck.EndReceive(ar);

                byte[] buf = new byte[8192];

                int rec = sck.Receive(buf, buf.Length, 0);

                if (rec < buf.Length)
                {

                    Array.Resize<byte>(ref buf, rec);

                }
                if (Recieved != null)
                {
                    try
                    {
                        string decodedString = Encoding.UTF8.GetString(buf);
                        decodedString = actualBuffer + decodedString;
                        if (decodedString.Contains("_||_"))
                        {
                            string[] parts = decodedString.Split(new string[] { "_||_"  }, StringSplitOptions.None); 
                            for (int i = 0; i < parts.Length - 1; i++)
                            {
                                Recieved(this, parts[i]);
                            }
                            if (parts[parts.Length - 1] != "") actualBuffer = parts[parts.Length - 1];
                            else actualBuffer = "";
                        }
                        else actualBuffer = decodedString;
                    }
                    catch(Exception e)
                    {

                    }
                }
                if (IsConnected)
                {
                    sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
                }
                else
                {
                    Disconnected?.Invoke(this);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Connecting to client failed. Ex: " + ex.Message, Logger.SEVERITY.ERROR);
                Close();

            }
        }

        public bool IsConnected
        {
            get
            {
                try
                {
                    if (sck != null && sck.Connected)
                    {
                        // Detect if client disconnected
                        if (sck.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (sck.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                // Client disconnected
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }



        public void Send(string text)
        {
            byte[] buf = Encoding.UTF8.GetBytes(text + "_||_");
            this.sck.BeginSend(buf, 0, buf.Length, SocketFlags.None,
            new AsyncCallback(SendCallback), this.sck);
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);


            }
            catch (Exception e)
            {
                Logger.Log("Sending message to client failed with message: " + e.Message, Logger.SEVERITY.ERROR);
            }
        }
        public void Close()
        {
            Logger.Log(String.Format("Socket {0} disconnected.", sck.RemoteEndPoint), Logger.SEVERITY.MESSAGE);

            Disconnected?.Invoke(this);
            sck.Disconnect(true);
        }
        public delegate void ClientRecievedHandler(Client sender, string data);
        public delegate void ClientDisconnectedHandler(Client sender);

        public event ClientRecievedHandler Recieved;
        public event ClientDisconnectedHandler Disconnected;
    }
}
