using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RildasApp.Models;
using System.IO;
using System.Windows.Forms;
using RildasApp.Forms;
namespace RildasApp
{
    public static class Global
    {
        static List<ChatWindow> chatWindows = new List<ChatWindow>();
        public static User loggedUser;
        public static string password;
        private static List<Anime> animes = new List<Anime>();
        private static List<Episode> episodes = new List<Episode>();
        private static List<EpisodeVersion> episodeVersions = new List<EpisodeVersion>();
        private static List<User> users = new List<User>();
        private static List<XDCCPackageDetails> xdccPackages = new List<XDCCPackageDetails>();
        private static List<string> xdccChannels = new List<string>();
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
            if (Dashboard.instance != null)  Dashboard.instance.DisableForm();
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
            switch (determinator)
            {
                // CLIENT
                case "CLIENT_CONNECTION_READY": { if (Dashboard.instance != null) { RildasServerAPI.Login(Global.loggedUser.username, Global.password); } }; break;
                case "CLIENT_LOGIN_SUCCESS": { if(Dashboard.instance != null) Dashboard.instance.EnableForm(); }; break;
                // DATA
                case "DATA_ANIME_FULL": { animes = new List<Anime>(Serializer.Deserialize<Anime[]>(rest)); if (AnimeListUpdated != null) AnimeListUpdated(); } break;
                case "DATA_EPISODE_FULL": { episodes = new List<Episode>(Serializer.Deserialize<Episode[]>(rest)); if (EpisodeListUpdated != null) EpisodeListUpdated(); }break;
                case "DATA_EPISODEVERSION_FULL": { episodeVersions = new List<EpisodeVersion>(Serializer.Deserialize<EpisodeVersion[]>(rest));if (EpisodeVersionListUpdated != null) EpisodeVersionListUpdated(); } break;
                case "DATA_USER_FULL": { users = new List<User>(Serializer.Deserialize<User[]>(rest)); if (UsersListUpdated != null) UsersListUpdated(); }break;
                case "DATA_IRCXDCCDATA_VERSIONS": { xdccPackages = Serializer.Deserialize<List<XDCCPackageDetails>>(rest); if (XDCCPackagesListUpdated != null) XDCCPackagesListUpdated(); } break;
                case "DATA_IRCXDCCDATA_CHANNELS": { xdccChannels = Serializer.Deserialize<List<string>>(rest); if (XDCCChannelsListUpdated != null) XDCCChannelsListUpdated(); } break;
                // CHANGEDATA
                case "CHANGEDATA_UPDATE_EPISODEVERSION": UpdateEpisodeVersion(Serializer.Deserialize<EpisodeVersion>(rest)); break;
                case "CHANGEDATA_ADD_EPISODEVERSION": AddEpisodeVersion(Serializer.Deserialize<EpisodeVersion>(rest)); break;
                case "CHANGEDATA_ADD_IRCXDCCPACKAGE": AddXDCCPackage(Serializer.Deserialize<XDCCPackageDetails>(rest)); break;
                //FILE
                case "FILE_DOWNLOAD_FILEVERSION": DownloadVersion(rest); break;
                // CHAT
                case "CHAT_RECEIVE_MESSAGE": ChatReceiveMessage(rest); break;
                case "CHAT_ALERT_REQUEST": ChatRequestAlert(rest); break;
            }

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

        private static void ChatReceiveMessage(string rest)
        {
            string[] split = rest.Split('_');
            int sender = int.Parse(split[0]);
            DateTime sendTime = Global.UnixTimeStampToDateTime(double.Parse(split[1]));
            
            string text = String.Join("_", split.Skip(2).ToArray());
            GetMessage(sender, text, sendTime);
            
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
            for(int i = 0; i < numberOfUnderscoresInPath; i++)
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
            for(int i = 0; i < episodeVersions.Count(); i++)
            {
                if(episodeVersions[i].id == epVer.id)
                {
                    episodeVersions[i] = epVer;
                    if (EpisodeVersionListUpdated != null) EpisodeVersionListUpdated();
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
            List<EpisodeVersion> epVersions = GetEpisodeVersions();
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
            foreach(Episode ep in episodes)
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
            foreach(User user in users)
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
        public static ChatWindow OpenIfNeeded(User user)
        {

            foreach (ChatWindow chat in chatWindows)
            {
                if ((chat.Tag as User).id == user.id)
                {
                    return chat;
                }

            }
            ChatWindow window = new ChatWindow();
            window.Tag = user;
            window.Text = "Private Chat - " + user.username;
            window.FormClosed += Window_FormClosed;
            window.Show();
            window.Activate();
            chatWindows.Add(window);
            return window;
        }
        public static void GetMessage(int from, string text, DateTime time)
        {
            User user = Global.GetUser(from);
            ChatWindow wind = OpenIfNeeded(user);
            wind.AppendMessage(text, time);
            wind.FlashWindowEx();
        }
        private static void Window_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < chatWindows.Count; i++)
            {
                if (chatWindows[i] == (sender as ChatWindow))
                {
                    chatWindows.Remove(chatWindows[i]);
                    return;
                }
            }
        }
        internal static void RequestAlert(int id)
        {
            User user = Global.GetUser(id);
            ChatWindow wind = OpenIfNeeded(user);
            wind.NoticeRequest();
            

        }

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
