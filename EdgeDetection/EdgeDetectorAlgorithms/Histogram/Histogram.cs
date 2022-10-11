using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram
{
    public class Histogram : IHistogram
    {
        private readonly PixelArray _PixelArray;

        public Histogram(Bitmap bitmap)
        {
            _PixelArray = new PixelArray(bitmap);
        }
        public HistogramResults Calculate()
        {
            HistogramResults results = new();
            int width = _PixelArray.Width;
            int height = _PixelArray.Height;

            for (int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        if (d == 0)
                        {
                            results.R_Series[(int)_PixelArray[x, y, d]]++;
                        }
                        if (d == 1)
                        {
                            results.G_Series[(int)_PixelArray[x, y, d]]++;
                        }
                        if (d == 2)
                        {
                            results.B_Series[(int)_PixelArray[x, y, d]]++;
                        }
                    }
                }
            }
            return results;
        }
    }
}
