using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
