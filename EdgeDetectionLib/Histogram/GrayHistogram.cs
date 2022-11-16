using System;
using System.Drawing;

namespace EdgeDetectionLib.Histogram
{
    /// <summary>
    /// Class that calculates image histogram gray series.
    /// </summary>
    public class GrayHistogram : IHistogram
    {
        private readonly PixelMatrix _pixelMatrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHistogram"/> class.
        /// </summary>
        /// <param name="bitmap"></param>
        public GrayHistogram(Bitmap bitmap)
        {
            _pixelMatrix = new PixelMatrix(bitmap);
        }

        /// <inheritdoc />
        public HistogramResults Calculate()
        {
            var results = new HistogramResults(HistogramType.Grayscale);
            int width = _pixelMatrix.Width;
            int height = _pixelMatrix.Height;

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = (int)_pixelMatrix[x, y, 0];
                    results.Gray_Series![index]++;
                }
            }
            return results;
        }
    }
}
