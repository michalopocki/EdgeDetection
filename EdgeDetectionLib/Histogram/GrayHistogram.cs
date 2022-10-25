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
        private readonly PixelMatrix _pixelMatrix;

        public GrayHistogram(Bitmap bitmap)
        {
            _pixelMatrix = new PixelMatrix(bitmap);
        }
        public HistogramResults Calculate()
        {
            var results = new HistogramResults();
            int width = _pixelMatrix.Width;
            int height = _pixelMatrix.Height;

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    results.Gray_Series[(int)_pixelMatrix[x, y, 0]]++;
                }
            });
            return results;
        }
    }
}
