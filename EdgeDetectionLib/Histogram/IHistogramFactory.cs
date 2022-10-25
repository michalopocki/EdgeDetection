using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Histogram
{
    public interface IHistogramFactory
    {
        IHistogram Create(Bitmap bitmap);
    }
}
