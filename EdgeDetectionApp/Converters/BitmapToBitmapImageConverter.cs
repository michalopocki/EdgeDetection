using EdgeDetectionLib;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EdgeDetectionApp.Converters
{
    public class BitmapToBitmapImageConverter : OneWayConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bitmap = (Bitmap)value;
            var bitmapImage = bitmap.ToBitmapImage();

            return bitmapImage;
        }
    }


}
