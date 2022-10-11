using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram
{
    public interface IHistogramFactory
    {
        IHistogram Create(Bitmap bitmap, bool isGrayscale);
    }
}
