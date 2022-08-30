using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EdgeDetection.Converters
{
    public class BitmapToBitmapImageConverter : OneWayConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Bitmap To Image source:" + watch.ElapsedMilliseconds + " ms");

            return image;
        }
    }


}
