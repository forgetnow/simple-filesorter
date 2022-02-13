#define debug
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    internal static Dictionary<string, List<string>> types = new Dictionary<string, List<string>>();
    static string mainPath = "V:\\Files";

    private static void Main()
    {
        if (!Directory.Exists(mainPath))
        {
            Directory.CreateDirectory(mainPath);
        }

        types.Add("Images", new List<string>() { "png", "jpeg", "bmp", "gif", "jpg", "mpo", "png", "arw4" });
        types.Add("Music", new List<string>() { "mp3", "wav", "m4a", "flac", "mp3", "wma", "aac" });
        types.Add("Video", new List<string>() { "mp4", "avi", "flv", "wmv", "mkv", "mpg", "mpe", "mpeg", "m2ts", "mts", "divx", "asf", "wmv", "divx", "ogv", "3gp", "3g2", "rm", "rmvb", "mov", "flv", "mvc" });
        types.Add("Document", new List<string>() { "doc", "ppt", "xls", "txt", "pdf" });
        types.Add("Archive", new List<string>() { "zip", "rar", "7z", "tar", "gzip", "bzip2", "apk", "ipa", "nrg" });
        types.Add("Executable", new List<string>() { "exe", "bat", "com", "jar", "iso" });
        types.Add("Trash", new List<string>() { "torrent", "lnk" });

        foreach (string folder in types.Keys)
        {
            if (!Directory.Exists(mainPath + "\\" +  folder))
            {
                Directory.CreateDirectory(mainPath + "\\" + folder);
            }
        }

        if (!Directory.Exists(mainPath + "\\Other"))
        {
            Directory.CreateDirectory(mainPath + "\\Other");
        }

        FileSystemWatcher watcher = new FileSystemWatcher(mainPath, "*.*");
        watcher.IncludeSubdirectories = false;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.CreationTime;
        watcher.Created += sortFile;
        watcher.EnableRaisingEvents = true;

        while (true) { }
    }

    private static void sortFile(object sender, FileSystemEventArgs CurrentFile)
    {
        FileInfo fileInfo = new FileInfo(CurrentFile.FullPath);
         
        foreach (KeyValuePair<string, List<string>> type in types)
        {
            foreach (string name in type.Value)
            {
                Console.WriteLine("." + name);
                if ("." + name == fileInfo.Extension)
                {
                    moveFile(fileInfo, type.Key.ToString());
                    return;
                }
            }
        }

        moveFile(fileInfo, "Other");
    }

    private static void moveFile(FileInfo fileInfo, string type)
    {
        try
        {
            string newName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            string newPath = mainPath + "\\" + type + "\\";

            if (File.Exists(newPath + fileInfo.Name))
            {
                newName = getRenamedFile(fileInfo, newPath);
            }

            fileInfo.MoveTo(newPath + newName + fileInfo.Extension);
            return;
        }
        catch (FileNotFoundException)
        {
            return;
        }
        catch (IOException)
        {
            Thread.Sleep(1000);

            moveFile(fileInfo, type);
        }
        catch
        {
            return;
        }
    }

    private static string getRenamedFile(FileInfo fileInfo, string path)
    {
        string currentName;
        int filecount = 1;

        do
        {
            currentName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            currentName = currentName + "(" + filecount + ")";
            filecount++;
        }
        while (File.Exists(path + currentName + fileInfo.Extension));

        Console.WriteLine(currentName);
        return currentName;
    }
}