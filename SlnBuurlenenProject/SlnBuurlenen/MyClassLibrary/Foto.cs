using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MyClassLibrary
{
    public class Foto
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public int VoertuigId { get; set; }
        public Foto()
        {
        }

        public static BitmapImage GetBitmapImageFromByteArray(byte[] imageData) // chatgpt voor images dynamisch toevoegen
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (MemoryStream stream = new MemoryStream(imageData))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }
    }
}
