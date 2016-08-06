using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RildasApp.Models;
using System.IO;
using System.Windows.Forms;
using RildasApp.Forms;
using System.Runtime.InteropServices;
using MetroFramework.Forms;
using System.Configuration;

namespace RildasApp
{
    public static class Global
    {
        public class PrivateMessage
        {
            public int sender;
            public DateTime time;
            public string text;
        }
        public class GroupMessage
        {
            public int groupId;
            public int senderId;
            public string text;
            public DateTime time;
        }
        static List<ChatWindowPrivate> chatWindows = new List<ChatWindowPrivate>();
        static List<ChatWindowGroup> groupChatWindows = new List<ChatWindowGroup>();
        public static User loggedUser;
        public static string password;
        private static List<Anime> animes = new List<Anime>();
        private static List<Episode> episodes = new List<Episode>();
        private static List<EpisodeVersion> episodeVersions = new List<EpisodeVersion>();
        private static List<Notification> notifications = new List<Notification>();
        private static List<ChatGroup> chatGroups = new List<ChatGroup>();
        private static List<User> users = new List<User>();
        private static List<User> loggedUsers = new List<User>();
        private static List<XDCCPackageDetails> xdccPackages = new List<XDCCPackageDetails>();
        private static List<string> xdccChannels = new List<string>();
        private static List<PrivateMessage> unseenPrivateMessages = new List<PrivateMessage>();
        private static List<GroupMessage> unseenGroupMessages = new List<GroupMessage>();
        static Global()
        {

        }
        public static void Init()
        {
            ConnectionManager.Recieved += ConnectionManager_Recieved;
            ConnectionManager.Disconnected += ConnectionManager_Disconnected;
        }
        private static void ConnectionManager_Disconnected()
        {
            if (Dashboard.instance != null) Dashboard.instance.DisableForm();
            while (!ConnectionManager.Connect()) ;
            if (Dashboard.instance != null) Dashboard.instance.EnableForm();
        }

        private static void ConnectionManager_Recieved(string data)
        {
            string[] split = data.Split('_');
            string determinator = String.Join("_", split[0], split[1], split[2]);
            string rest;
            if (determinator.Length == data.Length) rest = "";
            else rest = data.Substring(determinator.Length + 1, data.Length - determinator.Length - 1);
            split = split.Skip(3).ToArray();
            try
            {
                switch (determinator)
                {
                    // CLIENT
                    case "CLIENT_CONNECTION_READY": { if (Dashboard.instance != null) { RildasServerAPI.Login(Global.loggedUser.username, Global.password); } }; break;
                    case "CLIENT_LOGIN_SUCCESS": { if (Dashboard.instance != null) Dashboard.instance.EnableForm(); }; break;
                    // DATA
                    case "DATA_ANIME_FULL": { animes = new List<Anime>(Serializer.Deserialize<Anime[]>(rest)); if (AnimeListUpdated != null) AnimeListUpdated(); } break;
                    case "DATA_CHATGROUPS_FULL": { chatGroups = new List<ChatGroup>(Serializer.Deserialize<ChatGroup[]>(rest)); if (ChatGroupListUpdated != null) ChatGroupListUpdated(); }; break;
                    case "DATA_EPISODE_FULL": { episodes = new List<Episode>(Serializer.Deserialize<Episode[]>(rest)); if (EpisodeListUpdated != null) EpisodeListUpdated(); } break;
                    case "DATA_EPISODEVERSION_FULL": { episodeVersions = new List<EpisodeVersion>(Serializer.Deserialize<EpisodeVersion[]>(rest)); if (EpisodeVersionListUpdated != null) EpisodeVersionListUpdated(); } break;
                    case "DATA_USER_FULL": { users = new List<User>(Serializer.Deserialize<User[]>(rest)); if (UsersListUpdated != null) UsersListUpdated(); } break;
                    case "DATA_IRCXDCCDATA_VERSIONS": { xdccPackages = Serializer.Deserialize<List<XDCCPackageDetails>>(rest); if (XDCCPackagesListUpdated != null) XDCCPackagesListUpdated(); } break;
                    case "DATA_IRCXDCCDATA_CHANNELS": { xdccChannels = Serializer.Deserialize<List<string>>(rest); if (XDCCChannelsListUpdated != null) XDCCChannelsListUpdated(); } break;
                    case "DATA_TEAMMEMBER_FULL": AddMembers(Serializer.Deserialize<User[]>(rest)); break;
                    case "DATA_NOTIFICATION_FULL": { notifications = new List<Notification>(Serializer.Deserialize<Notification[]>(rest)); if (NotificationListUpdated != null) NotificationListUpdated(); } break;

                    // CHANGEDATA
                    case "CHANGEDATA_UPDATE_EPISODE": UpdateEpisode(Serializer.Deserialize<Episode>(rest)); break;
                    case "CHANGEDATA_UPDATE_EPISODEVERSION": UpdateEpisodeVersion(Serializer.Deserialize<EpisodeVersion>(rest)); break;
                    case "CHANGEDATA_ADD_EPISODEVERSION": AddEpisodeVersion(Serializer.Deserialize<EpisodeVersion>(rest)); break;
                    case "CHANGEDATA_ADD_IRCXDCCPACKAGE": AddXDCCPackage(Serializer.Deserialize<XDCCPackageDetails>(rest)); break;
                    case "CHANGEDATA_ADD_NOTIFICATION": AddNotification(Serializer.Deserialize<Notification>(rest)); break;
                    //FILE
                    case "FILE_DOWNLOAD_FILEVERSION": DownloadVersion(rest); break;
                    // CHAT
                    case "CHAT_RECEIVE_MESSAGE": ChatReceiveMessage(rest); break;
                    case "CHAT_RECEIVE_GROUPMESSAGE": ChatReceiveGroupMessage(int.Parse(split[0]), int.Parse(split[1]), Global.UnixTimeStampToDateTime(double.Parse(split[2])), String.Join("_", split.Skip(3).ToArray())); break;
                    case "CHAT_ALERT_REQUEST": ChatRequestAlert(rest); break;

                    case "CHAT_USER_DISCONNECTED": ChatUserDisconnected(int.Parse(rest)); break;
                    case "CHAT_USER_CONNECTED": ChatUserConnected(int.Parse(rest)); break;
                    case "CHAT_USER_ONLINELIST": { loggedUsers = new List<User>(Serializer.Deserialize<User[]>(rest)); if (OnlineUsersListUpdated != null) OnlineUsersListUpdated(); }; break;
                }
            }
            catch (Exception e)
            {
                ;
            }

        }
        public static bool SetApplicationSettings(string pstrKey, string pstrValue)
        {
            Configuration objConfigFile =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool blnKeyExists = false;

            foreach (string strKey in objConfigFile.AppSettings.Settings.AllKeys)
            {
                if (strKey == pstrKey)
                {
                    blnKeyExists = true;
                    objConfigFile.AppSettings.Settings[pstrKey].Value = pstrValue;
                    break;
                }
            }
            if (!blnKeyExists)
            {
                objConfigFile.AppSettings.Settings.Add(pstrKey, pstrValue);
            }
            objConfigFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            return true;
        }
        public static PrivateMessage[] GetUnseenMessages(User user, bool deleteThem = false)
        {
            var messages = unseenPrivateMessages.Where(x => x.sender == user.id).ToArray();
            if (deleteThem && messages.Count() > 0)
            {
                foreach (var message in messages) unseenPrivateMessages.Remove(message);
                if (UnseenPrivateMessagesUpdated != null) UnseenPrivateMessagesUpdated();
            }
            return messages;  
        }
        public static GroupMessage[] GetUnseenMessages(ChatGroup group, bool deleteThem = false)
        {
            var messages = unseenGroupMessages.Where(x => x.groupId == group.id).ToArray();
            if (deleteThem && messages.Count() > 0)
            {
                foreach (var message in messages) unseenGroupMessages.Remove(message);
                if (UnseenGroupMessagesUpdated != null) UnseenGroupMessagesUpdated();
            }
            return messages;
        }
        public static string GetApplicationSettings(string pstrKey)
        {
            return ConfigurationManager.AppSettings[pstrKey];
        }
        private static void AddNotification(Notification notification, int time = 10)
        {
            notifications.Add(notification);
            NotificationListUpdated?.Invoke();
            if (!ConfigApp.silentNotifications)
            {
                Dashboard.instance.StyleManager = new MetroFramework.Components.MetroStyleManager
                {
                    Theme = MetroFramework.MetroThemeStyle.Dark,
                    Style = MetroFramework.MetroColorStyle.Green
                };
                MetroTaskWindow.ShowTaskWindow(Dashboard.instance, "Upozornění", new NewNotification(notification.header, notification.text), time);
            }
        }

        
        internal static void SetProgramAsStartUp()
        {
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t);
            try
            {
                var lnk = shell.CreateShortcut(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "RildasApp.lnk"));
                try
                {
                    lnk.TargetPath = Application.ExecutablePath;
                    lnk.IconLocation = "shell32.dll, 1";
                    lnk.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }
        internal static bool IsProgramSetAsStartup()
        {
            return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "RildasApp.lnk"));
        }
        internal static void DeleteProgramFromStartUp()
        {
            File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "RildasApp.lnk"));
        }
        internal static List<Notification> GetNotifications()
        {
            return notifications;
        }
        private static void AddMembers(User[] newUsers)
        {
            foreach (User user in newUsers)
            {
                if (!users.Exists(x => x.id == user.id)) users.Add(user);
            }
        }

        private static void ChatUserConnected(int id)
        {
            if (Dashboard.instance == null) return;
            User user = Global.GetUser(id);
            UserConnected?.Invoke(user);
            loggedUsers.Add(user);
            //TODO: if silent login 
            Dashboard.instance.StyleManager = new MetroFramework.Components.MetroStyleManager
            {
                Theme = MetroFramework.MetroThemeStyle.Dark,
                Style = MetroFramework.MetroColorStyle.Green
            };
            MetroTaskWindow.ShowTaskWindow(Dashboard.instance, "Upozornění", new NewNotification(user.username + " je nyní online.", user.username + " je nyní online."), 5);
        }

        private static void ChatUserDisconnected(int id)
        {
            if (Dashboard.instance == null) return;
            User user = Global.GetUser(id);
            UserDisconnected?.Invoke(user);
            loggedUsers.Remove(user);
            //TODO: if silent login
            Dashboard.instance.StyleManager = new MetroFramework.Components.MetroStyleManager
            {
                Theme = MetroFramework.MetroThemeStyle.Dark,
                Style = MetroFramework.MetroColorStyle.Green
            };
            MetroTaskWindow.ShowTaskWindow(Dashboard.instance, "Upozornění", new NewNotification(user.username + " se odhlásil.", user.username + " tě opustil. Nyní se můžeš dále utápět v beznaději."), 5);
        }

        internal static List<ChatGroup> GetChatGroups()
        {
            return chatGroups;
        }

        internal static List<User> GetLoggedUsers()
        {
            return loggedUsers;
        }

        private static void AddXDCCPackage(XDCCPackageDetails xDCCPackageDetails)
        {
            xdccPackages.Add(xDCCPackageDetails);
            if (XDCCPackagesListUpdated != null) XDCCPackagesListUpdated();
        }

        private static void ChatRequestAlert(string rest)
        {
            int id = int.Parse(rest);
            RequestAlert(id);
        }
        private static void ChatReceiveGroupMessage(int groupId, int senderId, DateTime sendTime, string text)
        {
            GroupMessage message = new GroupMessage()
            {
                groupId = groupId,
                senderId = senderId,
                time = sendTime,
                text = text
            };
            ChatGroup chatGroup = Global.GetChatGroup(groupId);
            ChatWindowGroup wind = OpenIfNeeded(chatGroup, message);
            if (wind == null) return;
            wind.AppendMessage(GetUser(senderId).username, text, sendTime);
            wind.FlashWindowEx();
        }
        private static void ChatReceiveMessage(string rest)
        {
            string[] split = rest.Split('_');
            PrivateMessage message = new PrivateMessage()
            {
                sender = int.Parse(split[0]),
                time = Global.UnixTimeStampToDateTime(double.Parse(split[1])),
                text = String.Join("_", split.Skip(2).ToArray())

            };
            if (Dashboard.instance == null)
            {
                unseenPrivateMessages.Add(message);
            }
            else GetMessage(message);

        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }



        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
        private static void DownloadVersion(string rest)
        {
            string[] split = rest.Split('_');
            int id = int.Parse(split[0]);
            int numberOfUnderscoresInPath = int.Parse(split[1]);
            string filepath = split[2];
            for (int i = 0; i < numberOfUnderscoresInPath; i++)
            {
                filepath += "_" + split[3 + i];
            }
            string fileData = String.Join("_", split.Skip(3 + numberOfUnderscoresInPath).ToArray());
            FileStream fs = new FileStream(filepath, FileMode.Create);
            // Create the writer for data.
            BinaryWriter w = new BinaryWriter(fs, Encoding.UTF8);
            // Write data to Test.data.
            w.Write(Encoding.UTF8.GetBytes(fileData));
            w.Close();
            fs.Close();

            EpisodeVersion version = GetEpisodeVersion(id);
            MessageBox.Show(String.Format("File {0} - {1} was successfully downloaded.", GetAnime(version.animeId).name, version.episode));
        }

        private static void AddEpisodeVersion(EpisodeVersion epVer)
        {
            episodeVersions.Add(epVer);
            if (EpisodeVersionListUpdated != null) EpisodeVersionListUpdated();
        }
        private static void UpdateEpisodeVersion(EpisodeVersion epVer)
        {
            for (int i = 0; i < episodeVersions.Count(); i++)
            {
                if (episodeVersions[i].id == epVer.id)
                {
                    episodeVersions[i] = epVer;
                    if (EpisodeVersionListUpdated != null) EpisodeVersionListUpdated();
                    return;
                }
            }
        }
        private static void UpdateEpisode(Episode ep)
        {
            for (int i = 0; i < episodes.Count(); i++)
            {
                if (episodes[i].id == ep.id)
                {
                    episodes[i] = ep;
                    if (EpisodeListUpdated != null) EpisodeListUpdated();
                    return;
                }
            }
        }
        public static List<Anime> GetAnimes()
        {
            return animes;
        }
        internal static EpisodeVersion[] GetLastEpisodeVersions(int start = 0, int count = 20)
        {
            List<EpisodeVersion> epVersions = GetEpisodeVersions().ToList();
            epVersions.Sort(delegate (EpisodeVersion x, EpisodeVersion y) { return y.id - x.id; });
            List<EpisodeVersion> result = new List<EpisodeVersion>();
            int max = epVersions.Count < start + count ? epVersions.Count : start + count;
            for (int i = start; i < max; i++)
            {
                result.Add(epVersions[i]);
            }
            return result.ToArray();
        }
        internal static List<XDCCPackageDetails> GetXDCCPackages()
        {
            return xdccPackages;
        }
        internal static List<string> GetXDCCChannels()
        {
            return xdccChannels;
        }
        public static List<Episode> GetEpisodes()
        {
            return episodes;
        }
        public static List<EpisodeVersion> GetEpisodeVersions()
        {
            return episodeVersions;
        }
        public static EpisodeVersion GetEpisodeVersion(int id)
        {
            return episodeVersions.FirstOrDefault(x => x.id == id);

        }
        public static ChatGroup GetChatGroup(int id)
        {
            return chatGroups.FirstOrDefault(x => x.id == id);
        }
        public static ChatGroup GetChatGroup(string name)
        {
            return chatGroups.FirstOrDefault(x => x.name == name);
        }


        public static List<User> GetUsers()
        {
            return users;
        }
        public static Anime GetAnime(int id)
        {
            return animes.FirstOrDefault(x => x.id == id);
        }
        public static Anime GetAnime(string name)
        {
            return animes.FirstOrDefault(x => x.name == name);
        }
        internal static Episode[] GetEpisodes(int animeId)
        {
            List<Episode> eps = new List<Episode>();
            foreach (Episode ep in episodes)
            {
                if (ep.animeid == animeId) eps.Add(ep);
            }
            return eps.ToArray();
        }
        internal static Anime[] GetAnimesOfUser(User user)
        {
            List<Anime> anims = new List<Anime>();
            foreach (Anime anime in animes)
            {
                if (anime.translatorid == user.id) anims.Add(anime);
            }
            return anims.ToArray();
        }
        internal static User GetUser(int id)
        {
            foreach (User user in users)
            {
                if (user.id == id) return user;
            }
            return null;
        }
        internal static EpisodeVersion[] GetEpisodeVersions(int animeId, int epNumber, bool special = false, bool reverse = false)
        {
            List<EpisodeVersion> epVers = new List<EpisodeVersion>();
            foreach (EpisodeVersion epver in episodeVersions)
            {
                if (epver.episode == epNumber && epver.animeId == animeId && epver.special == special) epVers.Add(epver);
            }
            if (reverse) epVers.Reverse();
            return epVers.ToArray();
        }
        public static ChatWindowPrivate OpenIfNeeded(User user, PrivateMessage message = null, bool userTriggeredAction = false)
        {

            foreach (ChatWindowPrivate chat in chatWindows)
            {
                if ((chat.Tag as User).id == user.id)
                {
                    if(ConfigApp.silentPrivateMessages && !userTriggeredAction) chat.Activate();
                    return chat;
                }

            }
            ChatWindowPrivate window = null;
            if (ConfigApp.silentPrivateMessages && !userTriggeredAction)
            {
                unseenPrivateMessages.Add(message);
                if (UnseenPrivateMessagesUpdated != null) UnseenPrivateMessagesUpdated();
                return window;
            }
            Dashboard.instance.Invoke(new MethodInvoker(delegate
            {
                window = new ChatWindowPrivate();
                window.Tag = user;
                window.Text = "Private Chat - " + user.username;
                window.FormClosed += Window_FormClosed;
                window.ShowInTaskbar = true;
                window.Show();
                window.Activate();
                var unseenMessages = GetUnseenMessages(user, true);
                foreach (var mess in unseenMessages)
                {
                    window.AppendMessage(mess.text, mess.time);
                }
                window.FlashWindowEx();
                chatWindows.Add(window);

            }));
            return window;
        }

        public static ChatWindowGroup OpenIfNeeded(ChatGroup chatGroup, GroupMessage message = null,
            bool userTriggeredAction = false)
        {
            foreach (ChatWindowGroup chat in groupChatWindows)
            {
                if ((chat.Tag as ChatGroup).id == chatGroup.id)
                {
                    if (ConfigApp.silentPrivateMessages && !userTriggeredAction) chat.Activate();
                    return chat;
                }
            }
            ChatWindowGroup window = null;
            if (ConfigApp.silentGroupMessages && !userTriggeredAction)
            {
                unseenGroupMessages.Add(message);
                UnseenGroupMessagesUpdated?.Invoke();
                return null;
            }
            Dashboard.instance.Invoke(new MethodInvoker(delegate
            {
                window = new ChatWindowGroup(chatGroup, loggedUsers)
                {
                    Tag = chatGroup,
                    Text = string.Format("Group Chat - {0}", chatGroup.name),
                    ShowInTaskbar = true
                };
                window.FormClosed += GroupWindow_FormClosed;
                window.Show();
                window.Activate();
                var unseenMessages = GetUnseenMessages(chatGroup, true);
                foreach (var mess in unseenMessages)
                {
                    window.AppendMessage(GetUser(mess.senderId).username, mess.text, mess.time);
                }
                groupChatWindows.Add(window);
            }));
            return window;

        }

        private static void GroupWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < groupChatWindows.Count; i++)
            {
                if (groupChatWindows[i] == (sender as ChatWindowGroup))
                {
                    groupChatWindows.Remove(groupChatWindows[i]);
                    return;
                }
            }
        }

        public static void GetMessage(PrivateMessage message)
        {
            User user = Global.GetUser(message.sender);
            ChatWindowPrivate wind = OpenIfNeeded(user, message);
            if (wind == null) return;
            wind.AppendMessage(message.text, message.time);
            wind.FlashWindowEx();
        }
        
        private static void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < chatWindows.Count; i++)
            {
                if (chatWindows[i] == (sender as ChatWindowPrivate))
                {
                    chatWindows.Remove(chatWindows[i]);
                    return;
                }
            }
        }
        internal static void RequestAlert(int id)
        {
            User user = Global.GetUser(id);
            ChatWindowPrivate wind = OpenIfNeeded(user);
            wind.NoticeRequest();
            

        }

        public delegate void OnlineUsersListUpdatedHandler();
        public static event OnlineUsersListUpdatedHandler OnlineUsersListUpdated;

        public delegate void UserConnectedHandler(User user);
        public static event UserConnectedHandler UserConnected;

        public delegate void UserDisconnectedHandler(User user);
        public static event UserDisconnectedHandler UserDisconnected;


        public delegate void UnseenMessagesUpdatedHandler();
        public static event UnseenMessagesUpdatedHandler UnseenGroupMessagesUpdated;
        public static event UnseenMessagesUpdatedHandler UnseenPrivateMessagesUpdated;
        public delegate void NotificationListUpdatedHandler();
        public static event NotificationListUpdatedHandler NotificationListUpdated;
        public delegate void ChatGroupListUpdatedHandler();
        public static event ChatGroupListUpdatedHandler ChatGroupListUpdated;
        public delegate void AnimeListUpdatedHandler();
        public static event AnimeListUpdatedHandler AnimeListUpdated;
        public delegate void EpisodeListUpdatedHandler();
        public static event EpisodeListUpdatedHandler EpisodeListUpdated;
        public delegate void EpisodeVersionListUpdatedHandler();
        public static event EpisodeVersionListUpdatedHandler EpisodeVersionListUpdated;
        public delegate void UsersListUpdatedHandler();
        public static event UsersListUpdatedHandler UsersListUpdated;
        public delegate void XDCCChannelsUpdatedHandler();
        public static event XDCCChannelsUpdatedHandler XDCCChannelsListUpdated;
        public delegate void XDCCPackagessUpdatedHandler();
        public static event XDCCPackagessUpdatedHandler XDCCPackagesListUpdated;


    }
}
