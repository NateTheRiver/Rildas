using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RildasApp
{
    public static class ConnectionManager
    {
        private static Socket sck;
        private static StringBuilder actualBuffer = new StringBuilder();
        
        public static bool Connect()
        {
            if (IsConnected) return true;
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                sck.Connect(IPAddress.Parse("127.0.0.1"), 51200);
                sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;

        }
        public static void callback(IAsyncResult ar)
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
                        actualBuffer.Append(decodedString);
                        //string decodedString = Encoding.UTF8.GetString(buf);
                        //decodedString = actualBuffer + decodedString;
                        if (decodedString.Contains("_||_"))
                        {
                            string[] parts = actualBuffer.ToString().Split(new string[] { "_||_" }, StringSplitOptions.None);
                            for (int i = 0; i < parts.Length - 1; i++)
                            {
                                Recieved(parts[i]);
                            }
                            if (parts[parts.Length - 1] != "") actualBuffer = new StringBuilder(parts[parts.Length - 1]);
                            else actualBuffer.Clear();
                        }
                        
                    }
                    catch (Exception)
                    {

                    }
                }
                if (IsConnected)
                {
                    sck.BeginReceive(new byte[] { 0 }, 0, 0, 0, callback, null);
                }
                else
                {
                    if(Disconnected != null) Disconnected();
                }
            }
            catch (Exception ex)
            {
                Close();

            }
        }

        public static bool IsConnected
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
        public static void Send(string text)
        {
            byte[] buf = Encoding.UTF8.GetBytes(text + "_||_");
            sck.BeginSend(buf, 0, buf.Length, SocketFlags.None,
            new AsyncCallback(SendCallback), sck);
        }
        private static void SendCallback(IAsyncResult ar)
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
            }
        }
        public static void Close()
        {
            sck.Close();
            if (Disconnected != null)
            {
                Disconnected();
            }
            sck.Disconnect(true);
        }
        public delegate void ServerRecievedHandler(string data);
        public delegate void ServerDisconnectedHandler();

        public static event ServerRecievedHandler Recieved;
        public static event ServerDisconnectedHandler Disconnected;


    }
}
