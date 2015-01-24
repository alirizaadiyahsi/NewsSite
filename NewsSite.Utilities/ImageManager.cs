using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace NewsSite.Utilities
{
    public static class ImageManager
    {
        /// <summary>
        /// resmi yeniden boyutlandır.
        /// </summary>
        /// <param name="imgToResize">boyutlandırılacak resim</param>
        /// <param name="size">boyutlar</param>
        /// <returns>Image titipnde bir resim</returns>
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            imgToResize.Dispose();

            return (Image)b;
        }

        /// <summary>
        /// Resmi farklı boyutlarda kaydeder.
        /// </summary>
        /// <param name="image">Boyutlandırılacak resim.</param>
        /// <param name="size">Resim boyutu.</param>
        /// <param name="directory">Resmin kaydedileceği dizin.</param>
        /// <param name="fileName">Resim adı.</param>
        public static void SaveResizedImage(Image image, Size size, string directory, string fileName)
        {
            Image _image = ResizeImage(image, size);
            _image.Save(Path.Combine(directory, fileName));
            _image.Dispose();
        }
    }
}
