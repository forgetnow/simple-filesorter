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

        types.Add("Images", new List<string>() { "bmp", "ecw", "gif", "ico", "ilbm", "jpeg", "jpeg 2000", "Mrsid", "pcx", "png", "psd", "tga", "tiff", "jfif", "hd photo", "webp", "xbm", "xps", "rla", "rpf", "pnm"});
        types.Add("Music", new List<string>() { "3gp", "aa", "aac", "aax", "act", "aiff", "alac", "amr", "ape", "au", "awb", "dss", "dvf", "flac", "gsm", "iklax", "ivs", "m4a", "m4b", "m4p", "mmf", "mp3", "mpc", "msv", "nmf", "ogg", "oga", "mogg", "opus", "ra", "rm", "raw", "rf64", "sln", "tta", "voc", "wav", "wma", "wv", "webm", "8svx", "cda"});
        types.Add("Video", new List<string>() { "webm", "mkv", "flv", "vob", "ogv", "drc", "gif", "gifv", "mng", "avi", "mts", "m2ts", "ts", "mov", "qt", "wmv", "yuv", "rmvb", "viv", "asf", "amv", "mp4", "m4p", "m4v", "mpg", "mp2", "mpeg", "mpe", "mpv", "m2v", "m4v", "svi", "3gp", "3g2", "3g2", "roq", "msv", "flv", "f4v", "f4p", "f4a", "f4b"});
        types.Add("Document", new List<string>() { "asp", "cdd", "cpp", "doc", "docm", "docx", "dot", "dotm", "dotx", "epub", "fb2", "gpx", "ibooks", "indd", "kdc", "key", "kml", "mdb", "mdf", "mobi", "mso", "ods", "odt", "one", "oxps", "pages", "pdf", "pkg", "pl", "pot", "potm", "potx", "pps", "ppsm", "ppsx", "ppt", "pptx", "ps", "pub", "rtf", "sdf", "sgm1", "sldm", "snb", "wpd", "wps", "xar", "xlr", "xls", "xlsb", "xlsm", "slsx", "xlt", "xltm", "xltx", "xps" });
        types.Add("Archive", new List<string>() { "a", "ar", "cpio", "shar", "lbr", "iso", "lbr", "mar", "sbx", "tar", "bz2", "f", "?xf", "gz", "lz", "lz4", "lzma", "lzo", "lz", "sfark", "sz", "?q?", "?z", "xz", "z", "zst", "??_", "7z", "s7z", "ace", "afa", "alz", "apk", "arc", "ark", "cdx", "arj", "b1", "b6z", "ba", "bh", "cab", "car", "cfs", "cpt", "dar", "dd", "dgc", "dmg", "ear", "gca", "genozip", "ha", "hki", "ice", "jar", "kgb", "lzh", "lha", "lzx", "pak", "partimg", "paq6", "paq7", "paq8", "pea", "phar", "pim", "pit", "qda", "rar", "rk", "sda", "sea", "sen", "sfx", "shk", "sit", "sitx", "sqx", "tar.gz", "tgz", "tar.z", "tar.bz2", "tbz2", "tar.lz", "tlz", "tar.xz", "txz", "tar.zst", "uc", "uc0", "uc2", "ucn", "ur2", "ue2", "uca", "uha", "war", "wim", "xar", "xp3", "yz1", "zip", "zipx", "zoo", "zpaq", "zz", "ecc", "ecsbx", "par", "par2", "rev" });
        types.Add("Executable", new List<string>() {"exe", "msi", "bat", "sh", "cmd", "com"});
        types.Add("Trash", new List<string>() { "torrent" });

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

        return currentName;
    }
}