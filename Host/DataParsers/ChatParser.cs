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
    class ChatParser : IParser
    {
        private static ChatParser instance;
        public static ChatParser Instance
        {
            get { return instance = instance ?? new ChatParser(); }
        }

        public void ParseData(Client sender, string[] data)
        {
            try
            {
                switch (data[0])
                {
                    case "SENDMESSAGE": SendText(sender, int.Parse(data[1]), data.Skip(2).ToArray()); break;
                    case "SENDGROUPMESSAGE": SendGroupText(sender, int.Parse(data[1]), data.Skip(2).ToArray()); break;
                    case "SENDALERT": SendAlert(sender, int.Parse(data[1])); break;
                }
            }
            catch (Exception e)
            {
                Logger.Log(String.Format("Failed to parse Chat data. Command: {0}. User: {1}. Exception: {2}", String.Join("_", data), sender.loggedUser == null ? "" : sender.loggedUser.username, e.Message), Logger.SEVERITY.ERROR);
            }
        }

        private void SendGroupText(Client sender, int groupId, string[] textMessage)
        {
            string text = String.Join("_", textMessage);
            ChatGroup chatGroup = GlobalData.GetChatGroup(groupId);

            List<Client> clients = ConnectionManager.connections;
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].loggedUser != null && chatGroup.members.Exists(x => x.id == clients[i].loggedUser.id))
                {
                    clients[i].Send(String.Format("CHAT_RECEIVE_GROUPMESSAGE_{0}_{1}_{2}_{3}", groupId, sender.loggedUser.id , GlobalData.DateTimeToUnixTimestamp(DateTime.Now), text));
                }
            }
        }

        private void SendAlert(Client sender, int recipient)
        {
            List<Client> clients = ConnectionManager.connections;
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].loggedUser != null && clients[i].loggedUser.id == recipient)
                {
                    clients[i].Send(String.Format("CHAT_ALERT_REQUEST_{0}", sender.loggedUser.id));
                }
            }
        }

        private static void SendText(Client sender, int recipient, string[] data)
        {
            string text = String.Join("_", data);
            List<Client> clients = ConnectionManager.connections;
            bool sent = false;
            for (int i = 0; i < clients.Count; i++)
            {
                if(clients[i].loggedUser != null && clients[i].loggedUser.id == recipient)
                {
                    sent = true;
                    clients[i].Send(String.Format("CHAT_RECEIVE_MESSAGE_{0}_{1}_{2}", sender.loggedUser.id, GlobalData.DateTimeToUnixTimestamp(DateTime.Now), text));
                }
            }
            if(!sent)
            {
                string[] columns = { "sender", "recipient", "text", "time" };
                string[] values = { sender.loggedUser.id.ToString(), recipient.ToString(), text, GlobalData.DateTimeToUnixTimestamp(DateTime.Now).ToString() };
                Database.Instance.InsertQuery("app_messages", columns, values);
            }
        }

    }
}
