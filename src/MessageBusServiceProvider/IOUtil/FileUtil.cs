using System.IO;

namespace Service.MessageBusServiceProvider.IOUtil
{
    public class FileUtil
    {
        public static bool CreateLocalDirectoriesInPath(string path)
        {
            var targetPath = Path.GetDirectoryName(path);

            if (!Directory.Exists(targetPath))
            {
                 Directory.CreateDirectory(targetPath);
                return true; 
            }

            return false;
        }
    }
}
