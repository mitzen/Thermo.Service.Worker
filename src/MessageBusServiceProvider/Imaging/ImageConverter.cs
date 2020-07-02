using System;
using System.Drawing;
using System.IO;

namespace Service.MessageBusServiceProvider.Imaging
{
    public class ImageConverter
    {
        public void SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            image.Save(fullOutputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        
        public string ExractBase64(string image)
        {
            return image.Replace("data:image/jpeg;base64,", string.Empty);
        }
    }
}
