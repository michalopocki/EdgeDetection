using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Histogram
{
    public class GrayHistogram : IHistogram
    {
        private readonly PixelArray _PixelArray;

        public GrayHistogram(Bitmap bitmap)
        {
            _PixelArray = new PixelArray(bitmap);
        }
        public HistogramResults Calculate()
        {
            var results = new HistogramResults();
            int width = _PixelArray.Width;
            int height = _PixelArray.Height;

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    results.Gray_Series[(int)_PixelArray[x, y, 0]]++;
                }
            });
            return results;
        }
    }
}
