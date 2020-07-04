using System.IO;

namespace Service.MessageBusServiceProvider.IOUtil
{
    public class FileUtil
    {
        public static bool CreateLocalDirectoriesInPath(string path)
        {
            var targetPath = Path.GetFullPath(path);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                return true; 
            }

            return false;
        }
    }
}
