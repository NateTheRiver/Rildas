using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.DataParsers
{
    class FileParser : IParser
    {
        private static FileParser instance;
        public static FileParser Instance
        {
            get { return instance = instance ?? new FileParser(); }
        }

        public string GetParserName()
        {
            return "FILE";
        }

        public void ParseData(Client sender, string[] data)
        {
            try
            {
                switch (data[0])
                {
                    case "UPLOAD": UploadFile(data[1], int.Parse(data[2]), data.Skip(3).ToArray()); break;
                    case "DOWNLOAD": SendFile(sender, data[1], int.Parse(data[2]), data[3] == "1", data.Skip(4).ToArray()); break;
                }
            }
            catch (Exception e)
            {
                Logger.Log(String.Format("Failed to parse File data. Command: {0}. User: {1}. Exception: {2}", String.Join("_", data), sender.loggedUser == null ? "" : sender.loggedUser.username, e.Message), Logger.SEVERITY.ERROR);
            }
        }

        private void SendFile(Client sender, string type, int id, bool englishVersion, string[] filepath)
        {
            if (type == "EPISODEVERSION")
            {
                string path = String.Join("_", filepath);
                string enVer = englishVersion ? "en_" : "";
                byte[] filebytes = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EpVersions", enVer + id));
                sender.Send(String.Format("FILE_DOWNLOAD_FILEVERSION_{0}_{1}_{2}_{3}", id, filepath.Length - 1, path, Encoding.UTF8.GetString(filebytes)));
            }
        }

        private void UploadFile(string type, int numberOfUnderscoresInName, string[] data)
        {
            bool first = true;
            string name = "";
            for (int i = 0; i < numberOfUnderscoresInName + 1; i++)
            {
                if (!first) name += "_";
                name += data[i];
                first = false;
            }
            data = data.Skip(numberOfUnderscoresInName + 1).ToArray();
            string fileName = "";
            byte[] fileData = Encoding.UTF8.GetBytes(String.Join("_", data));
            if(type == "EPISODEVERSION") fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EpVersionsTmp", name);

            if(fileName == "")
            {
                Logger.Log(String.Format("Failed to upload {0}. {1} is probably unknown type of file.", name, type));
                return;
            }
            if (File.Exists(fileName))
            {
                Logger.Log(String.Format("Upload of file {0} failed. It already exists!", name));
                return;
            }
            FileStream fs = new FileStream(fileName, FileMode.CreateNew);
            // Create the writer for data.
            BinaryWriter w = new BinaryWriter(fs, Encoding.UTF8);
            // Write data to Test.data.
            w.Write(fileData);
            w.Close();
            fs.Close();

        }
    }
}
