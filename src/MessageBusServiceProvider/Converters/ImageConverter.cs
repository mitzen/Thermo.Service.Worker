using System;
using System.Drawing;
using System.IO;

namespace Service.MessageBusServiceProvider.Converters
{
    public class ImageConverter
    {
        // https://stackoverflow.com/questions/5400173/converting-a-base-64-string-to-an-image-and-saving-it
        public static Image LoadImage(string image64Encoded)
        {
            byte[] bytes = Convert.FromBase64String(image64Encoded);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
    }
}
