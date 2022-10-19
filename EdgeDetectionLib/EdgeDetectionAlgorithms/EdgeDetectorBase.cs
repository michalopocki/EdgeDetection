using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public abstract class EdgeDetectorBase : IEdgeDetector
    {
        public abstract string Name { get; }
        public Bitmap? BeforeThresholdingBitmap { get; protected set; }
        protected PixelArray _PixelArray;
        protected int _width;
        protected int _height;
        protected bool _isGrayscale;

        public EdgeDetectorBase() { }
        public EdgeDetectorBase(IEdgeDetectorArgs args)
        {
            _PixelArray = new PixelArray(args.ImageToProcess);
            _width = args.ImageToProcess.Width;
            _height = args.ImageToProcess.Height;
            _isGrayscale = args.IsGrayscale;
        }
        public abstract Bitmap DetectEdges();

        protected PixelArray Convolution(double[][] filter)
        {
            return _isGrayscale ? Convolution2D(filter) : Convolution3D(filter);
        }
        protected void CutSides(int kernelSize)
        {
            int size = (int)Math.Ceiling((double)kernelSize / 2);
            var cutPixelArray = new PixelArray(_width - 2 * size, _height - 2 * size);

            for (int x = size, i = 0; x < _width - size; x++, i++)
            {
                for (int y = size, j = 0; y < _height - size; y++, j++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        cutPixelArray[i, j, d] = _PixelArray[x, y, d];

                        if (_isGrayscale)
                        {
                            cutPixelArray[i, j, 2] = cutPixelArray[i, j, 1] = cutPixelArray[i, j, 0];
                            break;
                        }
                    }
                }
            }
            _PixelArray = cutPixelArray;
            _width -= 2 * size;
            _height -= 2 * size;
        }
        protected PixelArray GradientMagnitude(PixelArray gradientGx, PixelArray gradientGy)
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

                        if (_isGrayscale)
                        {
                            gradientMagnitude[x, y, 2] = gradientMagnitude[x, y, 1] = gradientMagnitude[x, y, 0];
                            break;
                        }
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
        protected void Thresholding(int threshold)
        {
            Parallel.For(0, _width, x =>
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        if (_PixelArray[x, y, d] > threshold)
                        {
                            _PixelArray[x, y, d] = 255;
                        }
                        else
                        {
                            _PixelArray[x, y, d] = 0;
                        }
                        if (_isGrayscale)
                        {
                            _PixelArray[x, y, 2] = _PixelArray[x, y, 1] = _PixelArray[x, y, 0];
                            break;
                        }
                    }
                }
            });
        }
    }
}
