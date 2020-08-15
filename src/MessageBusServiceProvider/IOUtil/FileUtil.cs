using System.IO;
using System.Runtime.InteropServices;

namespace Service.MessageBusServiceProvider.IOUtil
{
    public class FileUtil
    {
        public static bool CreateDirectoryFromFilePath(string fileNameAndPath)
        {
            CreateLocalDirectoriesInPath(Path.GetDirectoryName(fileNameAndPath));

            if (!File.Exists(fileNameAndPath))
            {
                File.Create(fileNameAndPath);
                return true;
            }
            return false; 
        }

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


        public bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
    }
}
