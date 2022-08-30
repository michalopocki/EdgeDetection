using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace EdgeDetection.Model
{
    public class ImageEdgeDetection
    {
        public Bitmap OriginalImage { get; set; }
        public Bitmap ProcessedImage { get; set; }
        public FrameworkElement ChartView { get; set; }
        public ImageEdgeDetection(Bitmap originalImage)
        {
            OriginalImage = originalImage;
            ProcessedImage = originalImage;
        }
    }
}
