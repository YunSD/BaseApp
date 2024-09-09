using System.IO;

namespace BaseApp.Core.Utils
{
    public class BaseFileUtil
    {
        private static readonly string BaseFilePath = AppDomain.CurrentDomain.BaseDirectory
            + "Cache" + Path.DirectorySeparatorChar.ToString();

        private static readonly SnowflakeIdWorker IdWorker = SnowflakeIdWorker.Singleton;

        static BaseFileUtil()
        {
            Directory.CreateDirectory(BaseFilePath);
        }

        public static string? UpdateFile(string? sourceFilePath)
        {
            if (!File.Exists(sourceFilePath)) return null;

            string extension = Path.GetExtension(sourceFilePath);
            string destinationFilePath = GenNextFileName(extension);

            File.Copy(sourceFilePath, destinationFilePath, true);

            return Path.GetFileName(destinationFilePath);
        }

        public static string? GetOriFilePath(string? fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return null;
            string oriFileAddress = Path.Combine(BaseFilePath, fileName);
            if (File.Exists(oriFileAddress)) return oriFileAddress;
            return "pack://application:,,,/Assets/icon.png";
        }

        private static string GenNextFileName(string extension)
        {
            return Path.Combine(BaseFilePath, IdWorker.nextId().ToString() + extension);
        }
    }
}
