using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

class Program
{
    internal static int ActiveThreadsCount = 0;
    // Словарь для сортировки.
    internal static Dictionary<string, List<string>> types = new();
    // Путь к папке с программой.
    static readonly string mainPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    // Папка, откуда мы копируем файлы.
    static readonly string SourcePath = mainPath + "\\Source";
    // Папка, куда мы копируем файлы.
    static readonly string DestPath = mainPath + "\\Dest";
    // Папка, куда складываются логи.
    static readonly string LogsPath = mainPath + "\\Logs";

    private static void Main()
    {
        // Создаём папку из которой берём файлы сортировки.
        if (!Directory.Exists(SourcePath))
        {
            Directory.CreateDirectory(SourcePath);
        }

        // Создаём папку, куда перемещаем.
        if (!Directory.Exists(DestPath))
        {
            Directory.CreateDirectory(DestPath);
        }

        types.Add("Images", new List<string>() { "bmp", "ecw", "gif", "ico", "ilbm", "jpeg", "jpeg 2000", "Mrsid", "pcx", "png", "psd", "tga", "tiff", "jfif", "hd photo", "webp", "xbm", "xps", "rla", "rpf", "pnm" });
        types.Add("Music", new List<string>() { "3gp", "aa", "aac", "aax", "act", "aiff", "alac", "amr", "ape", "au", "awb", "dss", "dvf", "flac", "gsm", "iklax", "ivs", "m4a", "m4b", "m4p", "mmf", "mp3", "mpc", "msv", "nmf", "ogg", "oga", "mogg", "opus", "ra", "rm", "raw", "rf64", "sln", "tta", "voc", "wav", "wma", "wv", "webm", "8svx", "cda" });
        types.Add("Video", new List<string>() { "webm", "mkv", "flv", "vob", "ogv", "drc", "gif", "gifv", "mng", "avi", "mts", "m2ts", "ts", "mov", "qt", "wmv", "yuv", "rmvb", "viv", "asf", "amv", "mp4", "m4p", "m4v", "mpg", "mp2", "mpeg", "mpe", "mpv", "m2v", "m4v", "svi", "3gp", "3g2", "3g2", "roq", "msv", "flv", "f4v", "f4p", "f4a", "f4b" });
        types.Add("Document", new List<string>() { "asp", "cdd", "cpp", "doc", "docm", "docx", "dot", "dotm", "dotx", "epub", "fb2", "gpx", "ibooks", "indd", "kdc", "key", "kml", "mdb", "mdf", "mobi", "mso", "ods", "odt", "one", "oxps", "pages", "pdf", "pkg", "pl", "pot", "potm", "potx", "pps", "ppsm", "ppsx", "ppt", "pptx", "ps", "pub", "rtf", "sdf", "sgm1", "sldm", "snb", "wpd", "wps", "xar", "xlr", "xls", "xlsb", "xlsm", "slsx", "accdb", "xlt", "xltm", "xltx", "xps" });
        types.Add("Archive", new List<string>() { "a", "ar", "cpio", "shar", "lbr", "iso", "lbr", "mar", "sbx", "tar", "bz2", "f", "?xf", "gz", "lz", "lz4", "lzma", "lzo", "lz", "sfark", "sz", "?q?", "?z", "xz", "z", "zst", "??_", "7z", "s7z", "ace", "afa", "alz", "apk", "arc", "ark", "cdx", "arj", "b1", "b6z", "ba", "bh", "cab", "car", "cfs", "cpt", "dar", "dd", "dgc", "dmg", "ear", "gca", "genozip", "ha", "hki", "ice", "jar", "kgb", "lzh", "lha", "lzx", "pak", "partimg", "paq6", "paq7", "paq8", "pea", "phar", "pim", "pit", "qda", "rar", "rk", "sda", "sea", "sen", "sfx", "shk", "sit", "sitx", "sqx", "tar.gz", "tgz", "tar.z", "tar.bz2", "tbz2", "tar.lz", "tlz", "tar.xz", "txz", "tar.zst", "uc", "uc0", "uc2", "ucn", "ur2", "ue2", "uca", "uha", "war", "wim", "xar", "xp3", "yz1", "zip", "zipx", "zoo", "zpaq", "zz", "ecc", "ecsbx", "par", "par2", "rev" });
        types.Add("Executable", new List<string>() { "exe", "msi", "bat", "sh", "cmd", "com" });
        types.Add("Trash", new List<string>() { "torrent" });

        foreach (string folder in types.Keys)
        {
            // Создаём папки из файлов.
            if (!Directory.Exists(DestPath + "\\" + folder))
            {
                Directory.CreateDirectory(DestPath + "\\" + folder);
            }
        }

        // Создаём папку Other
        if (!Directory.Exists(DestPath + "\\Other"))
        {
            Directory.CreateDirectory(DestPath + "\\Other");
        }


        while (true)
        {
            System.Console.WriteLine(ActiveThreadsCount);
            // Если есть файл, то запускаем процесс сортировки первого попавшегося файла (по хорошему лучше заменить на FileSystemWatcher).
            while (ActiveThreadsCount < 10)
            {
                if (Directory.GetFiles(SourcePath).Length > 0)
                {
                    string[] files = Directory.GetFiles(SourcePath);

                    while ((ActiveThreadsCount < files.Length) && (ActiveThreadsCount < 10))
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(SortFile));
                        thread.Start(files[ActiveThreadsCount]);
                        ActiveThreadsCount = ActiveThreadsCount + 1;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Сортировка файла в определённую категорию.
    /// </summary>
    /// <param name="filePath">Полный путь к файлу, который сортируется.</param>
    private static void SortFile(object filePath)
    {
        string ObjectToPath = filePath.ToString();
        // Информация о файле, которая вышла сортирока.
        FileInfo fileInfo = new(ObjectToPath);

        StreamReader streamReader;

        // Заглушка от цикла. Так, как может произойти множественный вызов => IOException.
        try
        {
            streamReader = new(fileInfo.FullName);
        }
        catch
        {
            return;
        }
        
        // Отчёт количества символов от начала файла.
        int count = 0;
        // Текущий символ, получаемый при чтении файла.
        int charact;

        // Есть файл лога с таким же именем > Удаляем.
        if (File.Exists(LogsPath + "\\" + fileInfo.Name + ".log"))
        {
            File.Delete(LogsPath + "\\" + fileInfo.Name + ".log");
        }

        // Самый простой варинт посимвольного чтения файла.
        while (streamReader.Peek() >= 0)
        {
            charact = streamReader.Read();

            count++;
 
            if (charact == '\r')
            {
                charact = streamReader.Read();

                count++;

                if (charact == '\n')
                {
                    // Запись в лог, если найдена последовательность строк.
                    StreamWriter writer = new(LogsPath + "\\" + fileInfo.Name + ".log", true);
                    writer.Write(count.ToString() + ", ");
                    writer.Close();
                }
            }
        }

        streamReader.Close();

        // Сортировка и перенос в соответсвующую папку.
        foreach (KeyValuePair<string, List<string>> type in types)
        {
            foreach (string name in type.Value)
            {
                if ("." + name == fileInfo.Extension.ToLower())
                {
                    MoveFile(fileInfo, type.Key.ToString());
                    return;
                }
            }
        }

        // Перенос в папку Other, если не нашли, куда сортировать.
        MoveFile(fileInfo, "Other");
    }

    
    /// <summary>
    /// Перемещает файл
    /// </summary>
    /// <param name="fileInfo">Текущая информация о файле.</param>
    /// <param name="type">Папка в которую перемещается файл.</param>
    private static void MoveFile(FileInfo fileInfo, string type)
    {
        try
        {
            // Получаем файл без формата, перед возможным переименованием.
            string newName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            // Папка, в которую перемещается файл. 
            string newPath = DestPath + "\\" + type + "\\";
            // Если файл есть, то запускаем процесс получения свободного имени.
            if (File.Exists(newPath + fileInfo.Name))
            {
                // Получаем имя из метода.
                newName = GetRenamedFile(fileInfo, newPath);
            }

            // Перемещаем файл
            fileInfo.MoveTo(newPath + newName + fileInfo.Extension);
            return;
        }
        catch (IOException)
        {
            Thread.Sleep(1000);

            // Рекурсивно запускаем перемещение файла.
            MoveFile(fileInfo, type);
        }
        catch
        {
            return;
        }
        ActiveThreadsCount = ActiveThreadsCount - 1;
    }

    /// <summary>
    /// Общий итератор для выдачи свободного имени файлу.
    /// </summary>
    /// <param name="fileInfo">Информация о файле.</param>
    /// <param name="path">Путь в котором проверяется наличие файла.</param>
    /// <returns>Новое имя файла.</returns>
    private static string GetRenamedFile(FileInfo fileInfo, string path)
    {
        // Текущее имя.
        string currentName;
        // Итератор файлов.
        int filecount = 1;

        do
        {
            // Файл без расширения.
            currentName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            // Выбираем свободное имя с помощью итератора.
            currentName = currentName + " (" + filecount + ") ";
            filecount++;
        }
        while (File.Exists(path + currentName + fileInfo.Extension));

        return currentName;
    }
}