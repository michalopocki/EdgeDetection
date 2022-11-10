using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Histogram
{
    public class RGBHistogram : IHistogram
    {
        private readonly PixelMatrix _pixelMatrix;

        public RGBHistogram(Bitmap bitmap)
        {
            _pixelMatrix = new PixelMatrix(bitmap);
        }
        public HistogramResults Calculate()
        {
            var results = new HistogramResults(HistogramType.Colorscale);
            int width = _pixelMatrix.Width;
            int height = _pixelMatrix.Height;

            //Parallel.For(0, width, x =>
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        switch (d)
                        {
                            case 0:
                                results.B_Series![(int)_pixelMatrix[x, y, d]]++;
                                break;
                            case 1:
                                results.G_Series![(int)_pixelMatrix[x, y, d]]++;
                                break;
                            case 2:
                                results.R_Series![(int)_pixelMatrix[x, y, d]]++;
                                break;

                        }
                    }
                }
            }
            return results;
        }
    }
}
