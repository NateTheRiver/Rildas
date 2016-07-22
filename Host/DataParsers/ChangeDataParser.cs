using Host.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Host.DataParsers
{
    
    class ChangeDataParser : IParser
    {
        private static ChangeDataParser instance;
        public static ChangeDataParser Instance
        {
            get { return instance = instance ?? new ChangeDataParser(); }
        }
        public void ParseData(Client sender, string[] data)
        {
            try
            {
                switch (data[0])
                {
                    case "ADD":
                        AddObject(sender, data);
                        break;
                    case "UPDATE":
                        ChangeObject(sender, data);
                        break;
                    case "PUBLISH":
                        PublishObject(sender, data);
                        break;
                    case "QUEUEPUBLISH":
                        QueuePublishObject(sender, data);
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Log(String.Format("Failed to parse ChangeData data. Command: {0}. User: {1}. Exception: {2}", String.Join("_", data), sender.loggedUser == null ? "" : sender.loggedUser.username, e.Message), Logger.SEVERITY.ERROR);
            }
        }

        private void QueuePublishObject(Client sender, string[] data)
        {
            if (data[1] == "EPISODEVERSION")
            {
                int id = int.Parse(data[2]);
                DateTime dateTime = DateTime.ParseExact(data[3], "dd MM yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture);

            }
        }
        private void PublishObject(Client sender, string[] data)
        {
            if (data[1] == "EPISODEVERSION")
            {
                int id = int.Parse(data[2]);
                EpisodeVersion episodeVersion = GlobalData.GetEpisodeVersions().First(version => version.id == id);
                Database.Instance.UpdateQuery("episodes", "visible='1', email_notif='1'", String.Format("anime_id='{0}' AND ep_number='{1}' AND special='{2}'", episodeVersion.animeId, episodeVersion.episode, episodeVersion.special));
            }
        }
        private void ChangeObject(Client sender, string[] data)
        {
            if(data[1] == "EPISODEVERSION")
            {
                EpisodeVersion epVer = Serializer.Deserialize<EpisodeVersion>(String.Join("_", data.Skip(2).ToArray()));
                GlobalData.UpdateEpisodeVersion(epVer);
            }
        }

        private void AddObject(Client sender, string[] data)
        {
            if (data[1] == "EPISODEVERSION")
            {
                EpisodeVersion epVer = Serializer.Deserialize<EpisodeVersion>(String.Join("_", data.Skip(2).ToArray()));
                epVer.added = DateTime.Now;
                string type = epVer.type == EpisodeVersion.Type.PŘEKLAD? "PŘEKLAD": "KOREKCE"; 
                string[] columns = new string[] { "type", "anime_id", "episode", "special" , "ready", "comment", "timeOn", "addedBy", "added", "reservedBy" };
                string[] values = new string[] { type, epVer.animeId.ToString(), epVer.episode.ToString(), epVer.special?"1":"0", epVer.state.ToString(), epVer.comment, epVer.timeOn, epVer.addedBy.ToString(), epVer.added.Date.ToString("yyyy-MM-dd HH:mm:ss"), epVer.reservedBy.ToString() };
                epVer.id = Database.Instance.InsertQuery("app_files", columns, values);
                string path1 = Path.GetFullPath("./EpVersionsTmp/" + epVer._hash);
                string path2 = Path.GetFullPath("./EpVersionsTmp/en_" + epVer._hash);
                if (File.Exists(path1)) File.Move(path1, Path.GetFullPath("./EpVersions/" + epVer.id));
                if (File.Exists(path2)) File.Move(path2, Path.GetFullPath("./EpVersions/en_" + epVer.id));

                GlobalData.AddEpisodeVersion(epVer);
            }
        }

        public string GetParserName()
        {
            return "CHANGEDATA";
        }
    }
}
