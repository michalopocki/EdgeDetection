using System.Drawing;

namespace EdgeDetectionLib.Histogram
{
    /// <summary>
    /// Class that calculates image histogram RGB series.
    /// </summary>
    public class RGBHistogram : IHistogram
    {
        private readonly PixelMatrix _pixelMatrix;

        /// <summary>
        /// Initializes a new instance of the <see cref="RGBHistogram"/> class.
        /// </summary>
        /// <param name="bitmap"></param>
        public RGBHistogram(Bitmap bitmap)
        {
            _pixelMatrix = new PixelMatrix(bitmap);
        }

        /// <inheritdoc />
        public HistogramResults Calculate()
        {
            var results = new HistogramResults(HistogramType.Colorscale);
            int width = _pixelMatrix.Width;
            int height = _pixelMatrix.Height;

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
