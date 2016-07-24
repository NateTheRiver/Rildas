using Host.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    public static class GlobalData
    {
        private static object AnimeLock = new object();
        private static object EpisodeLock = new object();
        private static object EpisodeVersionLock = new object();
        private static object UsersLock = new object();


        private static List<Anime> _animes = new List<Anime>();
        private static List<Episode> _episodes = new List<Episode>();
        private static List<EpisodeVersion> _episodeVersions = new List<EpisodeVersion>();
        private static List<User> _users = new List<User>();
        private static List<ApplicationVersion> _versions = new List<ApplicationVersion>();
        private static List<ChatGroup> chatGroups = new List<ChatGroup>();
        public static void Init()
        {
            Logger.Log("GlobalData downloading started.", Logger.SEVERITY.MESSAGE);
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            DownloadAnimes();
            DownloadEpisodes();
            DownloadEpisodeVersions();
            DownloadUsers();
            CalculateMissingEpisodes();
            DownloadClientVersionsData();
            DownloadChatGroupData();
            sw.Stop();
            Logger.Log("GlobalData initialized. Time elapsed: " + sw.ElapsedMilliseconds + "ms");

        }

        public static void SendNotification(User user, string header, string text)
        {

        }
        public static User GetUser(int id)
        {
            return GetUsers().FirstOrDefault(x => x.id == id);
        }
        public static Anime GetAnime(int id)
        {
            return GetAnimes().FirstOrDefault(x => x.id == id);
        }
        public static void AddEpisodeVersion(EpisodeVersion episodeVersion)
        {

            SendNotification(GetUser(GetAnime(episodeVersion.animeId).translatorid), "Nová verze", "Byla přidána nová verze anime, které překládáte.");
            lock (EpisodeVersionLock)   
            {
                _episodeVersions.Add(episodeVersion);
                ConnectionManager.SendToAll("CHANGEDATA_ADD_EPISODEVERSION_" + Serializer.Serialize(episodeVersion));
                return;
            }
        }
        public static void UpdateEpisodeVersion(EpisodeVersion episodeVersion)
        {

            lock(EpisodeVersionLock)
            {
                for(int i = 0; i < _episodeVersions.Count; i++)
                {
                    if (_episodeVersions[i].id == episodeVersion.id)
                    {
                        if (_episodeVersions[i].reservedBy != episodeVersion.reservedBy)
                        {

                        }
                        if (_episodeVersions[i].special != episodeVersion.special)
                        {

                        }
                        if (_episodeVersions[i].episode != episodeVersion.episode)
                        {

                        }
                        if (_episodeVersions[i].state != episodeVersion.state)
                        {

                        }
                        if (_episodeVersions[i].timeOn != episodeVersion.timeOn)
                        {

                        }
                        _episodeVersions[i] = episodeVersion;
                        ConnectionManager.SendToAll("CHANGEDATA_UPDATE_EPISODEVERSION_" + Serializer.Serialize(episodeVersion));
                        //TODO: Update database
                        return;
                    } 

                }
            }
        }
        public static void UpdateEpisode(Episode episode)
        {

            lock (EpisodeLock)
            {
                for (int i = 0; i < _episodes.Count; i++)
                {
                    if (_episodes[i].id == episode.id)
                    {
                       
                        _episodes[i] = episode;
                        ConnectionManager.SendToAll("CHANGEDATA_UPDATE_EPISODE_" + Serializer.Serialize(episode));
                        Database.Instance.UpdateQuery("episodes", String.Format("special='{0}', name='{1}', downlink1='{2}', downlink2='{3}', online='{4}'", episode.special, episode.name, episode.link_mega, episode.link_ulozto, episode.link_online), String.Format("id='{0}'", episode.id));
                        return;
                    }

                }
            }
        }
        public static Anime[] GetAnimes()
        {
            lock(AnimeLock)
            {
                return _animes.ToArray();
            }
        }
        public static ApplicationVersion[] GetApplicationVersions()
        {
            return _versions.ToArray();
        }
        public static ApplicationVersion GetCurrentVersion(bool stable, bool visible)
        {
            return _versions.Find(x=> x.id == _versions.FindAll(y => y.isStable == stable && y.isVisible == visible).Max(y => y.id));
        }
        public static ChatGroup GetChatGroup(string name)
        {
            return chatGroups.First(x => x.name == name);
        }
        public static ChatGroup GetChatGroup(int id)
        {
            return chatGroups.First(x => x.id == id);
        }
        public static ChatGroup[] GetChatGroups()
        {
            return chatGroups.ToArray();
        }
        public static Episode[] GetEpisodes()
        {
            lock (EpisodeLock)
            {
                return _episodes.ToArray();
            }
        }
        public static EpisodeVersion[] GetEpisodeVersions()
        {
            lock (EpisodeVersionLock)
            {
                return _episodeVersions.ToArray();
            }
        }
        public static User[] GetUsers()
        {
            lock (UsersLock)
            {
                return _users.ToArray();
            }
        }
        private static void CalculateMissingEpisodes()
        {
            foreach (Anime anime in _animes)
            {
                for (int i = 1; i <= anime.ep_count; i++)
                {
                    IEnumerable<EpisodeVersion> epvers = _episodeVersions.Where(x => x.animeId == anime.id && x.episode == i && !x.special);
                    EpisodeVersion episodeVersion = null;
                    if (epvers.Count() > 0)
                    {
                        int maxId = epvers.Max(x => x.id);
                        episodeVersion = _episodeVersions.FirstOrDefault(x => x.id == maxId);
                    }
                    if (_episodes.FirstOrDefault(x => x.animeid == anime.id && x.ep_number == i && !x.special) == null)
                    {

                        state epState = state.Not_ready;
                        if (episodeVersion != null)
                        {
                            switch (episodeVersion.state)
                            {
                                case -4:
                                case -3: epState = state.Potvrzeni; break;
                                case -1:
                                case 0: epState = state.Korekce; break;
                                case 1: epState = state.Final; break;
                                case 2:
                                case 3: epState = state.Done; break;
                            }
                        }
                        _episodes.Add(new Episode()
                        {
                            animeid = anime.id,
                            epState = epState,
                            id = -1,
                            special = false,
                            ep_number = i

                        });
                    }
                }
                for (int i = 1; i <= anime.special_ep_count; i++)
                {
                    IEnumerable<EpisodeVersion> epvers = _episodeVersions.Where(x => x.animeId == anime.id && x.episode == i && x.special);
                    EpisodeVersion episodeVersion = null;
                    if (epvers.Count() > 0)
                    {
                        int maxId = epvers.Max(x => x.id);
                        episodeVersion = _episodeVersions.FirstOrDefault(x => x.id == maxId);
                    }
                    if (_episodes.FirstOrDefault(x => x.animeid == anime.id && x.ep_number == i && x.special) == null)
                    {
                        state epState = state.Not_ready;
                        if (episodeVersion != null)
                        {
                            switch (episodeVersion.state)
                            {
                                case -3: epState = state.Potvrzeni; break;
                                case -1:
                                case 0: epState = state.Korekce; break;
                                case 1: epState = state.Final; break;
                                case 2:
                                case 3: epState = state.Done; break;
                            }
                        }
                        _episodes.Add(new Episode()
                        {
                            animeid = anime.id,
                            epState = epState,
                            id = -1,
                            special = true,
                            ep_number = i

                        });

                    }
                }
            }
        }
        private static void DownloadUsers()
        {
            List<string> result = Database.Instance.SelectQuery("*", "users");
            for(int i = 0; i < result.Count; )
            {
                User user = new User();
                user.id = int.Parse(result[i++]);
                user.username = result[i++];
                user.password = result[i++]; // password
                user.email = result[i++];
                i++; // bday
                user.access = int.Parse(result[i++]);
                user.userState = User.ParseState(result[i++]);
                user.registrationTime = UnixTimeStampToDateTime(int.Parse(result[i++]));
                user.registrationIP = result[i++];
                user.lastLoginTime = UnixTimeStampToDateTime(int.Parse(result[i++]));
                user.lastLoginIP = result[i++];
                user.hash = result[i++];
                i++; // gender
                i++; // whoiam
                i++; // interest
                i++; // fb
                i++; // twitter
                i++; // skype
                i++; // denominatioin
                user.passwordHash = result[i++];
                _users.Add(user);
            }
            Logger.Log(String.Format("Loaded {0} users.", _users.Count()), Logger.SEVERITY.INFO);
        }
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
        private static void DownloadEpisodeVersions()
        {
            List<string> result = Database.Instance.SelectQuery("*", "app_files");
            for (int i = 0; i < result.Count; )
            {
                EpisodeVersion epVersion = new EpisodeVersion();
                epVersion.id = int.Parse(result[i++]);
                epVersion.type = EpisodeVersion.ParseType(result[i++]);
                epVersion.animeId = int.Parse(result[i++]);
                epVersion.episode = int.Parse(result[i++]);
                epVersion.special = !result[i++].Equals("0");
                epVersion.state = int.Parse(result[i++]); 
                epVersion.title = result[i++];
                epVersion.titleEN = result[i++];
                epVersion.name = result[i++];
                epVersion.nameEN = result[i++];
                epVersion.comment = result[i++];
                epVersion.timeOn = result[i++];
                epVersion.addedBy = int.Parse(result[i++]);
                epVersion.added = DateTime.Parse(result[i++]);
                epVersion.reservedBy = int.Parse(result[i++]); // whoiam
                _episodeVersions.Add(epVersion);
            }
            Logger.Log(String.Format("Loaded {0} EpisodeVersions.", _episodeVersions.Count()), Logger.SEVERITY.INFO);
        }
        private static void DownloadClientVersionsData()
        {
            List<string> result = Database.Instance.SelectQuery("*", "app_launcher");
            for (int i = 0; i < result.Count;)
            {
                ApplicationVersion version = new ApplicationVersion();
                version.id = int.Parse(result[i++]);
                version.downloadLocation = result[i++];
                version.version = result[i++];
                version.isStable = !result[i++].Equals("0");
                version.description = result[i++];
                version.releaseDate = DateTime.Parse(result[i++]); 
                version.isVisible = !result[i++].Equals("0");
                _versions.Add(version);
            }

        }
        private static void DownloadEpisodes()
        {
            List<string> result = Database.Instance.SelectQuery("*", "episodes");
            for (int i = 0; i < result.Count;)
            {
                Episode episode = new Episode();
                episode.id = int.Parse(result[i++]);
                episode.animeid = int.Parse(result[i++]);
                episode.ep_number = int.Parse(result[i++]);
                episode.special = !result[i++].Equals("0");
                episode.name = result[i++];
                episode.link_mega = result[i++];
                episode.link_ulozto = result[i++];
                episode.link_online = Escape(result[i++]);
                episode.email_notification = !result[i++].Equals("0");
                i++; // 1080 ???
                episode.visible = bool.Parse(result[i++]);
                episode.lastEditTime = UnixTimeStampToDateTime(int.Parse(result[i++]));
                episode.epState = state.Done;
                _episodes.Add(episode);
            }
            Logger.Log(String.Format("Loaded {0} episodes.", _episodes.Count()), Logger.SEVERITY.INFO);
        }

        private static void DownloadAnimes()
        {
            List<string> result = Database.Instance.SelectQuery("*", "anime");
            for (int i = 0; i < result.Count;)
            {
                Anime anime = new Anime();
                anime.id = int.Parse(result[i++]);
                anime.name = result[i++];
                anime.filename = result[i++];
                anime.animelist_img = result[i++];
                anime.banner_img = result[i++];
                anime.post_img = result[i++];
                anime.status = Anime.ParseStatus(result[i++]);
                anime.release_year = int.Parse(result[i++]);
                anime.ep_count = int.Parse(result[i++]);
                anime.special_ep_count = int.Parse(result[i++]);
                anime.banner_show = !result[i++].Equals("0");
                anime.plot = Escape(result[i++]);
                anime.age = int.Parse(result[i++]);
                anime.banner_text = Escape(result[i++]);
                anime.translatorid = int.Parse(result[i++]);
                anime.correctorid = int.Parse(result[i++]);
                _animes.Add(anime);
            }
            Logger.Log(String.Format("Loaded {0} animes.", _animes.Count()), Logger.SEVERITY.INFO);
        }
        private static void DownloadChatGroupData()
        {
            List<string> result = Database.Instance.SelectQuery("*", "app_chatgroup");
            for (int i = 0; i < result.Count;)
            {
                ChatGroup chatGroup = new ChatGroup();
                chatGroup.id = int.Parse(result[i++]);
                chatGroup.name = result[i++];
                chatGroup.members = new List<User>();
                chatGroups.Add(chatGroup);
            }
            result = Database.Instance.SelectQuery("*", "app_usersToChatGroups");
            for (int i = 0; i < result.Count;)
            {
                int id = i++;
                User user = GetUsers().First(x => x.id == int.Parse(result[id]));
                ChatGroup chatGroup = GetChatGroup(int.Parse(result[i++]));
                chatGroup.members.Add(user);
            }
            Logger.Log(String.Format("Loaded {0} chat groups.", chatGroups.Count()), Logger.SEVERITY.INFO);
        }
        private static string Escape(string v)
        {
            v = v.Replace("&", "&amp");
            v = v.Replace("\"", "&quot");
            v = v.Replace("'", "&apos;");
            v = v.Replace("<", "&lt;");
            v = v.Replace(">", "&gt;");
            return v;
        }
    }
}
