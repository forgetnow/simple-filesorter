

using System;
using System.IO;
using System.Threading;

class Program
{
    public static FileInfo? info;

    public static string[] folders = { "Images", "Music", "Video", "Documents", "Archives", "Executable", "Other" };

    public static string[] ImageTypes = { ".png", ".jpeg", ".bmp", ".gif", ".jpg", ".mpo", ".png", ".arw4" };
    public static string[] MusicTypes = { ".mp3", ".wav", ".m4a", ".flac", ".mp3", ".wma", ".aac"};
    public static string[] VideoTypes = { ".mp4", ".avi", ".flv", ".wmv", ".mkv", ".mpg", ".mpe", ".mpeg", ".m2ts", ".mts", ".divx", ".asf", ".wmv", ".divx", ".ogv", ".3gp", ".3g2", ".rm", ".rmvb", ".mov", ".flv", ".mvc"};
    public static string[] DocumentTypes = { ".doc", ".ppt", ".xls", ".txt", ".pdf" };
    public static string[] ArchiveTypes = { ".zip", ".rar", ".7z", ".tar", ".gzip", ".bzip2", ".apk", ".ipa", ".nrg" };
    public static string[] ExecutableTypes = { ".exe", ".bat", ".com", ".jar", ".iso" };
    public static string[] Trash = { ".torrent", ".lnk" };

    private static void Main(string[] args)
    {
        string? path;

        try
        {
            path = args[0];
        }
        catch
        {
            Console.WriteLine("No path specified");
            return;
        }

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileSystemWatcher watcher = new FileSystemWatcher(path, "*.*");
        watcher.IncludeSubdirectories = false;
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.CreationTime;
        watcher.Created += new FileSystemEventHandler(SortFile);
        watcher.EnableRaisingEvents = true;

        foreach (string folder in folders)
        {
            if (!Directory.Exists(path + @"\" + folder))
            {
                Directory.CreateDirectory(path + @"\" + folder);
            }
        }


        Console.WriteLine("filesorter started correctly.");


        Console.ReadKey();
    }

    private static void SortFile(object sender, FileSystemEventArgs CurrentFile)
    {
        for(int i = 0; i < 10; i++)
        {
            info = new FileInfo(CurrentFile.FullPath);
            try
            {
                foreach (var type in ImageTypes)
                {
                    if (info.Extension == type)
                    {
                        info.MoveTo(info.Directory + "\\Images\\" + info.Name, true);
                        return;
                    }
                }

                foreach (var type in MusicTypes)
                {
                    if (info.Extension == type)
                    {
                        info.MoveTo(info.Directory + "\\Music\\" + info.Name, true);
                        return;
                    }
                }

                foreach (var type in VideoTypes)
                {
                    if (info.Extension == type)
                    {
                        info.MoveTo(info.Directory + "\\Video\\" + info.Name, true);
                        return;
                    }
                }

                foreach (var type in DocumentTypes)
                {
                    if (info.Extension == type)
                    {
                        info.MoveTo(info.Directory + "\\Documents\\" + info.Name, true);
                        return;
                    }
                }

                foreach (var type in ArchiveTypes)
                {
                    if (info.Extension == type)
                    {
                        info.MoveTo(info.Directory + "\\Archives\\" + info.Name, true);
                        return;
                    }
                }

                foreach (var type in ExecutableTypes)
                {
                    if (info.Extension == type)
                    {
                        info.MoveTo(info.Directory + "\\Executable\\" + info.Name, true);
                        return;
                    }
                }

                foreach (var type in Trash)
                {
                    if (info.Extension == type)
                    {
                        System.Threading.Thread.Sleep(100);
                        info.Delete();
                        return;
                    }
                }

                info.MoveTo(info.Directory + "\\Other\\" + info.Name, true);

                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("The file could not be moved: " + info.Name + info.Extension + " " + ex.Message);
            }

            Thread.Sleep(1000);
        }
    }
}

