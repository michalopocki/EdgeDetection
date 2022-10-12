using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public abstract class EdgeDetectorBase : IEdgeDetector
    {
        public abstract string Name { get; }
        protected PixelArray? PixelArray;
        protected int _width;
        protected int _height;
        protected bool _isGrayscale;

        public EdgeDetectorBase(Bitmap bitmap, bool isGrayscale = false)
        {
            PixelArray = new PixelArray(bitmap);
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
        protected PixelArray Magnitude(PixelArray imgGx, PixelArray imgGy)
        {
            PixelArray magnitudeImg = new PixelArray(_width, _height);
            imgGx.Abs();
            imgGy.Abs();
            magnitudeImg = imgGx + imgGy;

            return magnitudeImg;
        }
        private PixelArray Convolution3D(double[][] filter)
        {
            PixelArray resultImg = new PixelArray(_width, _height);
            int limiter = (filter.GetLength(0) - 1) / 2;

            if (PixelArray is not null)
            {
                //for (int x = limiter; x < _width - limiter; x++)
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
                                    resultImg[x, y, d] += PixelArray[x - m, y - n, d] * filter[m + limiter][n + limiter];
                                }
                            }
                        }
                    }
                });
            }
            return resultImg;
        }
        private PixelArray Convolution2D(double[][] filter)
        {
            PixelArray resultImg = new PixelArray(_width, _height);
            int limiter = (filter.GetLength(0) - 1) / 2;

            if (PixelArray is not null)
            {
                //for (int x = limiter; x < _width - limiter; x++)
                Parallel.For(limiter, _width - limiter, x =>
                {
                    for (int y = limiter; y < _height - limiter; y++)
                    {
                        for (int m = -limiter; m <= limiter; m++)
                        {
                            for (int n = -limiter; n <= limiter; n++)
                            {
                                resultImg[x, y, 0] += PixelArray[x - m, y - n, 0] * filter[m + limiter][n + limiter];
                            }
                        }
                        resultImg[x, y, 2] = resultImg[x, y, 1] = resultImg[x, y, 0];
                    }
                });
            }
            return resultImg;
        }
    }
}
