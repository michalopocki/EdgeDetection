using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public abstract class EdgeDetectorBase : IEdgeDetector
    {
        public abstract string Name { get; }
        protected PixelArray _PixelArray;
        protected int _width;
        protected int _height;
        protected bool _isGrayscale;

        public EdgeDetectorBase(Bitmap bitmap, bool isGrayscale = false)
        {
            _PixelArray = new PixelArray(bitmap);
            _width = bitmap.Width;
            _height = bitmap.Height;
            _isGrayscale = isGrayscale;
        }
        public EdgeDetectorBase() { }
        public abstract Bitmap DetectEdges();

        protected PixelArray Convolution(double[][] filter)
        {
            return _isGrayscale ? Convolution2D(filter) : Convolution3D(filter);
        }
        protected static PixelArray GradientMagnitude(PixelArray gradientGx, PixelArray gradientGy)
        {
            //PixelArray gradientMagnitude = PixelArray.Abs(gradientGx) + PixelArray.Abs(gradientGy);
            int width = gradientGx.Width;
            int height = gradientGx.Height;
            var gradientMagnitude = new PixelArray(width, height);

            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        gradientMagnitude[x, y, d] = Math.Sqrt(gradientGx[x, y, d] * gradientGx[x, y, d] + gradientGy[x, y, d] * gradientGy[x, y, d]);
                    }
                }
            });
            return gradientMagnitude;
        }
        private PixelArray Convolution3D(double[][] filter)
        {
            var resultArray = new PixelArray(_width, _height);
            int limiter = (filter.GetLength(0) - 1) / 2;

            Parallel.For(limiter, _width - limiter, x =>
            {
                for (int y = limiter; y < _height - limiter; y++)
                {
                    for (int m = -limiter; m <= limiter; m++)
                    {
                        for (int n = -limiter; n <= limiter; n++)
                        {
                            for (int d = 0; d < 3; d++)
                            {
                                resultArray[x, y, d] += _PixelArray[x - m, y - n, d] * filter[m + limiter][n + limiter];
                            }
                        }
                    }
                }
            });

            return resultArray;
        }
        private PixelArray Convolution2D(double[][] filter)
        {
            var resultArray = new PixelArray(_width, _height);
            int limiter = (filter.GetLength(0) - 1) / 2;

            Parallel.For(limiter, _width - limiter, x =>
            {
                for (int y = limiter; y < _height - limiter; y++)
                {
                    for (int m = -limiter; m <= limiter; m++)
                    {
                        for (int n = -limiter; n <= limiter; n++)
                        {
                            resultArray[x, y, 0] += _PixelArray[x - m, y - n, 0] * filter[m + limiter][n + limiter];
                        }
                    }
                    resultArray[x, y, 2] = resultArray[x, y, 1] = resultArray[x, y, 0];
                }
            });

            return resultArray;
        }
    }
}
