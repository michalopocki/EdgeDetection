using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram
{
    public class HistogramFactory : IHistogramFactory
    {
        public IHistogram Create(Bitmap bitmap, bool isGrayscale)
        {
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
