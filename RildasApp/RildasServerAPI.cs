using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RildasApp.Models;

namespace RildasApp
{
    public static class RildasServerAPI
    {
        /// <summary>
        /// Pokusí se přihlásit uživatele
        /// </summary>
        /// <param name="username">Jméno</param>
        /// <param name="password">Heslo</param>
        /// <returns>Údaje o lognutém uživateli. V případě neúspěchu vrací null.</returns>
        /// 
       
        internal static void Login(string username, string password)
        {
            ConnectionManager.Send(String.Format("CLIENT_LOGIN_{0}_{1}_{2}", username.Count(f => f == '_'), username, password));
        }
        internal static void AddVersion(EpisodeVersion version)
        {
            ConnectionManager.Send(String.Format("CHANGEDATA_ADD_EPISODEVERSION_{0}", Serializer.Serialize(version)));
        }

        internal static void UpdateEpisodeVersion(EpisodeVersion version)
        {
            ConnectionManager.Send(String.Format("CHANGEDATA_UPDATE_EPISODEVERSION_{0}", Serializer.Serialize(version)));
        }
        internal static void GetAllAnimes()
        {
            ConnectionManager.Send("GETDATA_ANIME_ALL");
            while (Global.GetAnimes().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }
        internal static void SendMessage(int recipientId, string message)
        {
            ConnectionManager.Send(String.Format("CHAT_SENDMESSAGE_{0}_{1}", recipientId, message));
        }
        internal static void SendGroupMessage(int groupId, string message)
        {
            ConnectionManager.Send(String.Format("CHAT_SENDGROUPMESSAGE_{0}_{1}", groupId, message));
        }
        internal static void SendNoticeRequest(int recipientId)
        {
            ConnectionManager.Send(String.Format("CHAT_SENDALERT_{0}", recipientId));
        }

        internal static void GetAllEpisodes()
        {
            ConnectionManager.Send("GETDATA_EPISODE_ALL");

            while (Global.GetEpisodes().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
            
        }
        internal static void UploadFile(int numberOfUnderscores, string hash, string file)
        {
            ConnectionManager.Send(String.Format("FILE_UPLOAD_EPISODEVERSION_{0}_{1}_{2}", numberOfUnderscores, hash, file));
        }
        internal static void DownloadFile(EpisodeVersion version, string localPath, bool englishVersion = false)
        {
            ConnectionManager.Send(String.Format("FILE_DOWNLOAD_EPISODEVERSION_{0}_{1}_{2}", version.id, englishVersion?"1":"0", localPath));
        }
        internal static void GetAllEpisodeVersions()
        {
            ConnectionManager.Send("GETDATA_EPISODEVERSION_ALL");
            while (Global.GetEpisodeVersions().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

      internal static void GetAllChatGroups()
        {
            ConnectionManager.Send("GETDATA_CHATGROUP_ALL");
            while (Global.GetChatGroups().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }
        internal static void GetAllUsers()
        {
            ConnectionManager.Send("GETDATA_USER_ALL");
            while (Global.GetUsers().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        internal static void GetAllXDCCVersions()
        {
            GetFilteredXDCCVersions("", false);
            ConnectionManager.Send("GETDATA_IRCXDCCDATA_CHANNELSTOJOIN");
            while (Global.GetXDCCChannels().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }
        internal static void GetFilteredXDCCVersions(string filter, bool dirty)
        {
            ConnectionManager.Send(String.Format("GETDATA_IRCXDCCDATA_PACKAGES_{0}_{1}", dirty?"DIRTY": "NONDIRTY", filter));
        }
        internal static void GetLoggedUsers()
        {
            ConnectionManager.Send("GETDATA_LOGGEDUSERS");
            while (Global.GetLoggedUsers().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        internal static void GetTeamMembers()
        {
            ConnectionManager.Send("GETDATA_TEAMMEMBER_ALL");
            while (Global.GetUsers().Count == 0)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        internal static void GetNotifications()
        {
            ConnectionManager.Send("GETDATA_NOTIFICATIONS_ALL");
        }

        internal static void PublishEpisodeVersion(EpisodeVersion selectedVersion)
        {
            ConnectionManager.Send("CHANGEDATA_PUBLISH_EPISODEVERSION_  " + selectedVersion.id);
        }

        internal static void PublishEpisodeVersion(EpisodeVersion selectedVersion, DateTime date, int hours, int minutes)
        {
            ConnectionManager.Send(String.Format("CHANGEDATA_QUEUEPUBLISH_EPISODEVERSION_{0}_{1}", selectedVersion.id, String.Format("{0}.{1}.{2} {3}:{4}", date.Day, date.Month, date.Year, hours, minutes)));
        }
    }
}
