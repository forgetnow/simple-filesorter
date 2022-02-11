#define debug
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    internal static FileInfo info;
    internal static Dictionary<string, List<string>> types = new Dictionary<string, List<string>>();

    static string FilesDirectory = @"C:\Users\Admin\Desktop\Files";

    private static void Main(string[] args)
    {
        if (!Directory.Exists(FilesDirectory))
        {
            Directory.CreateDirectory(FilesDirectory);
        }

        types.Add("Images", new List<string>() { ".png", ".jpeg", ".bmp", ".gif", ".jpg", ".mpo", ".png", ".arw4" });
        types.Add("Music", new List<string>() { ".mp3", ".wav", ".m4a", ".flac", ".mp3", ".wma", ".aac" });
        types.Add("Video", new List<string>() { ".mp4", ".avi", ".flv", ".wmv", ".mkv", ".mpg", ".mpe", ".mpeg", ".m2ts", ".mts", ".divx", ".asf", ".wmv", ".divx", ".ogv", ".3gp", ".3g2", ".rm", ".rmvb", ".mov", ".flv", ".mvc" });
        types.Add("Document", new List<string>() { ".doc", ".ppt", ".xls", ".txt", ".pdf" });
        types.Add("Archive", new List<string>() { ".zip", ".rar", ".7z", ".tar", ".gzip", ".bzip2", ".apk", ".ipa", ".nrg" });
        types.Add("Executable", new List<string>() { ".exe", ".bat", ".com", ".jar", ".iso" });
        types.Add("Trash", new List<string>() { ".torrent", ".lnk" });

        foreach (string folder in types.Keys)
        {
            if (!Directory.Exists(FilesDirectory + @"\" + folder))
            {
                Directory.CreateDirectory(FilesDirectory + @"\" + folder);
            }
        }

        if (!Directory.Exists(FilesDirectory + "//Other"))
        {
            Directory.CreateDirectory(FilesDirectory + "//Other");
        }

        FileSystemWatcher watcher = new FileSystemWatcher(FilesDirectory, "*.*");
        watcher.IncludeSubdirectories = false;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.CreationTime;
        watcher.Created += new FileSystemEventHandler(SortFile);
        watcher.EnableRaisingEvents = true;

        while (true) { };
    }

    private static void SortFile(object sender, FileSystemEventArgs CurrentFile)
    {
#if debug
        Console.WriteLine("Sort Process");
#endif
        info = new FileInfo(CurrentFile.FullPath);

        foreach (var type in types)
        {
#if debug
            Console.WriteLine("1 step");
#endif
            foreach (var name in type.Value)
            {
#if debug
                Console.WriteLine(name);
#endif
                if (name == info.Extension)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
#if debug
                            Console.WriteLine(FilesDirectory + "\\" + type.Key.ToString() + "\\");
                            info.MoveTo(FilesDirectory + "\\" + type.Key.ToString() + "\\" + info.Name);
                            break;
#endif
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(1000);
                            continue;
                        }
                    }
                    return;
                }
            }

            info.MoveTo(FilesDirectory + "\\Other\\" + info.Name);
        }
    }
}