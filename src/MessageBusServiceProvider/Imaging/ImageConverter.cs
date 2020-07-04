using System;
using System.IO;

namespace Service.MessageBusServiceProvider.Imaging
{
    public class ImageConverter
    {
        public static void SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            File.WriteAllBytes(fullOutputPath, Convert.FromBase64String(base64String));
        }
        
        public static string ExractBase64(string base64StringImage)
        {
            return base64StringImage.Replace("data:image/jpeg;base64,", string.Empty);
        }
    }
}
