using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace EdgeDetectionLib.Histogram
{
    public class HistogramFactory : IHistogramFactory
    {
        public IHistogram Create(Bitmap bitmap)
        {
            bool isGrayscale = bitmap.PixelFormat == PixelFormat.Format8bppIndexed;
            if (isGrayscale)
            {
                return new GrayHistogram(bitmap);
            }
            else
            {
                return new RGBHistogram(bitmap);
            }
        }
    }
}
