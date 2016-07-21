using Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Host.DataParsers
{
    class GetDataParser : IParser
    {
        private static GetDataParser instance;
        public static GetDataParser Instance
        {
            get { return instance = instance ?? new GetDataParser(); }
        }
        public void ParseData(Client sender, string[] data)
        {
            try
            {
                switch (data[0])
                {
                    case "ANIME": GetAnimeData(sender, data[1]); break;
                    case "EPISODE": GetEpisodeData(sender, data[1]); break;
                    case "EPISODEVERSION": GetEpisodeVersionData(sender, data[1]); break;
                    case "CHATGROUP": GetChatGroup(sender, data[1]); break;
                    case "USER": GetUserData(sender, data[1]); break;
                    case "TEAMMEMBER": GetTeamMemberData(sender, data[1]); break;
                    case "IRCXDCCDATA": GetIRCXDCCData(sender, data.Skip(1).ToArray()); break;
                    case "LOGGEDUSERS": GetLoggedUsers(sender); break;
                }
            }
            catch (Exception e)
            {
                Logger.Log(String.Format("Failed to parse GetData data. Command: {0}. User: {1}. Exception: {2}", String.Join("_", data), sender.loggedUser == null ? "" : sender.loggedUser.username, e.Message), Logger.SEVERITY.ERROR);
            }
        }

        private void GetTeamMemberData(Client sender, string data)
        {
            if (data == "ALL") { sender.Send("DATA_TEAMMEMBER_FULL_" + Serializer.Serialize(GlobalData.GetUsers().Where(x => x.access > 1).ToArray())); }
        }

        private void GetLoggedUsers(Client sender)
        {
            User[] loggedUsers = ConnectionManager.connections.Where(x=>x.loggedUser != null).Select(x => x.loggedUser).ToArray();
            sender.Send("CHAT_USER_ONLINELIST_" + Serializer.Serialize(loggedUsers));
        }

        private void GetChatGroup(Client sender, string data)
        {
            if (data == "ALL") { sender.Send("DATA_CHATGROUPS_FULL_" + Serializer.Serialize(GlobalData.GetChatGroups())); }
        }

        private void GetIRCXDCCData(Client sender, string[] data)
        {
            if (data[0] == "PACKAGES")
            {
                bool dirty = data[1] == "DIRTY";
                string[] filters = data.Skip(2).ToArray();
                
                List<XDCCPackageDetails> filteredDetails = new List<XDCCPackageDetails>();
                XDCCPackageDetails[] allDetails = AutoXDCCUpdater.GetPackageDetails().ToArray();
                allDetails=  allDetails.Reverse().ToArray();
                foreach (XDCCPackageDetails package in allDetails) 
                {
                    bool everyFilterMatch = true;
                    foreach (string filter in filters)
                    {
                        if (package.quality.Contains(filter) || package.botName.Contains(filter) || package.fansubGroup.Contains(filter)) continue;
                        if (package.filename.Contains(filter))
                        {
                            if (dirty) continue;
                            int position = package.filename.IndexOf(filter);
                            int firstClosingBrack = package.filename.IndexOf("]", position);
                            if (firstClosingBrack == -1) continue; // Pokud za textem není žádná "]", tak text nemůže být uzavřen v závorkách
                            int firstOpeningBrack = package.filename.IndexOf("[", position);
                            if(firstOpeningBrack == -1) // Pokud se za textem nachází "]", ale nikoliv "[", tak text musí být v závorkách
                            {
                                everyFilterMatch = false;
                                break;
                            }
                            if(firstOpeningBrack > firstClosingBrack)
                            {
                                everyFilterMatch = false;
                                break;
                            }
                        }
                        else
                        {
                            everyFilterMatch = false;
                            break;
                        }
                    }
                    if (everyFilterMatch)
                    {
                        filteredDetails.Add(package);
                        if (filteredDetails.Count >= 200) break;
                    }
                }
                sender.Send("DATA_IRCXDCCDATA_VERSIONS_" + Serializer.Serialize(filteredDetails));
            }
            if (data[0] == "CHANNELSTOJOIN") sender.Send("DATA_IRCXDCCDATA_CHANNELS_" + Serializer.Serialize(AutoXDCCUpdater.channelsToJoin));
        }

        private void GetUserData(Client sender, string data)
        {
            if (data == "ALL") { sender.Send("DATA_USER_FULL_" + Serializer.Serialize(GlobalData.GetUsers())); }
            else
            {
                sender.Send(String.Format("DATA_USER_{0}_{1}", data, Serializer.Serialize(GlobalData.GetUsers().FirstOrDefault(x=>x.id == int.Parse(data)))));
            }
        }

        private void GetEpisodeVersionData(Client sender, string data)
        {
            if (data == "ALL") { sender.Send("DATA_EPISODEVERSION_FULL_" + Serializer.Serialize(GlobalData.GetEpisodeVersions())); }
        }

        private void GetEpisodeData(Client sender, string data)
        {
            if (data == "ALL") { sender.Send("DATA_EPISODE_FULL_" + Serializer.Serialize(GlobalData.GetEpisodes())); }
        }

        private void GetAnimeData(Client sender, string data)
        {
            if(data == "ALL") { sender.Send("DATA_ANIME_FULL_" + Serializer.Serialize(GlobalData.GetAnimes())); }
        }

        public void Login(string username, string password)
        {

        }
    }
}