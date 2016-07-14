using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace RildasAppUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Updating to the new version.");
                if (args.Length == 0 || string.IsNullOrEmpty(args[0])) throw new ArgumentNullException("Missing argument: download location");
                using (var client = new WebClient())
                {
                    client.DownloadFile(args[0], "client.zip");
                }
                // System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath);
                if (Directory.Exists("newVersion")) Directory.Delete("newVersion", true);
                Process.GetProcessesByName("RildasApp")[0].Kill();
                DirectoryInfo directoryInfo = Directory.CreateDirectory("newVersion");
                System.IO.Compression.ZipFile.ExtractToDirectory("client.zip", "newVersion");
                MoveDirectory(directoryInfo, ".");
                var files = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    if (File.Exists(Path.Combine(Environment.CurrentDirectory, file.Name))) File.Delete(Path.Combine(Environment.CurrentDirectory, file.Name));
                    File.Move(Path.Combine(Environment.CurrentDirectory, file.Name), Path.Combine(Environment.CurrentDirectory, "newVersion", file.Name));
                }
                Directory.Delete("newVersion", true);

                Process p = new Process();
                p.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RildasApp.exe");
                p.Start();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Data);
                // Get stack trace for the exception with source file information
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                Console.WriteLine(line);
                Console.Read();
            }
        }
        static void MoveDirectory(DirectoryInfo directoryInfo, string recursivePath)
        {
            var files = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
            if (!Directory.Exists(recursivePath)) Directory.CreateDirectory(recursivePath);
            foreach (var file in files)
            {
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, recursivePath, file.Name))) File.Delete(Path.Combine(Environment.CurrentDirectory, recursivePath, file.Name));
                File.Move(Path.Combine(Environment.CurrentDirectory, "newVersion", recursivePath, file.Name), Path.Combine(Environment.CurrentDirectory, recursivePath, file.Name));
            }
            var directories = directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
            foreach(var directory in directories)
            {
                MoveDirectory(directory, Path.Combine(recursivePath, directory.Name));
            }
           
        }
       
    }
}
