using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace EdgeDetectionLib.Histogram
{
    /// <summary>
    /// Factory responsible for creating histograms.
    /// </summary>
    public class HistogramFactory : IHistogramFactory
    {
        /// <inheritdoc />
        public IHistogram Create(Bitmap bitmap)
        {
            bool isGrayscale = bitmap.PixelFormat is PixelFormat.Format8bppIndexed;
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
