using Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Host.DataParsers
{
    class ClientParser : IParser
    {
        private static ClientParser instance;
        public static ClientParser Instance
        {
            get { return instance = instance ?? new ClientParser(); }
        }
        public void ParseData(Client sender, string[] data)
        {
            try
            {
                switch (data[0])
                {

                    case "LOGIN": Login(sender, data.Skip(1).ToArray()); break;
                }
            }
            catch(Exception e)
            {
                Logger.Log(String.Format("Failed to parse Client data. Command: {0}. User: {1}. Exception: {2}", String.Join("_", data), sender.loggedUser == null ? "": sender.loggedUser.username, e.Message), Logger.SEVERITY.ERROR);
            }
        }
        public void Login(Client sender, string[] data)
        {
            if(data.Length < 3)
            {
                sender.Send("CLIENT_LOGIN_FAILED");
                return;
            }
            int numberOfBrackets = int.Parse(data[0]);
            string username = data[1];
            for (int i = 2; i <= (numberOfBrackets + 1); i++)
            {
                username += "_" + data[i];
            }
            string password = String.Join("_", data.Skip(2 + numberOfBrackets).ToArray());
            foreach(User user in GlobalData.GetUsers())
            {
                if (user.username == username)
                {
                    if (user.password == CreateMD5(password))
                    {
                        sender.loggedUser = user;
                        sender.Send("CLIENT_LOGIN_SUCCESS_" + Serializer.Serialize(user));
                        foreach(Client client in ConnectionManager.Connections)
                        {
                            if (client != sender) client.Send("CHAT_USER_CONNECTED_" + user.id);
                        }
                    }
                    else sender.Send("CLIENT_LOGIN_FAILED");
                    return;
                }
            }
            sender.Send("CLIENT_LOGIN_FAILED");
        }
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            byte[] asciiBytes = ASCIIEncoding.UTF8.GetBytes(input);
            byte[] hashedBytes = MD5CryptoServiceProvider.Create().ComputeHash(asciiBytes);
            string hashedString = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hashedString;
        }

        public string GetParserName()
        {
            return "CLIENT";
        }
    }
}
