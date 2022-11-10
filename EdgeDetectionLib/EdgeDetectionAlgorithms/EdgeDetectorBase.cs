using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
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
        protected internal PixelMatrix _pixelMatrix;
        protected internal int _width;
        protected internal int _height;
        protected internal int _dimensions;

        public EdgeDetectorBase(IEdgeDetectorArgs args)
        {
            if (args.ImageToProcess is not null)
            {
                SetBitmap(args.ImageToProcess);
            }
        }

        public abstract EdgeDetectionResult DetectEdges();

        public void SetBitmap(Bitmap newBitamp)
        {
            _pixelMatrix = new PixelMatrix(newBitamp);
            _width = newBitamp.Width;
            _height = newBitamp.Height;
            _dimensions = newBitamp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ? 1 : 3;
        }

        public static string GetName(IEdgeDetector edgeDetector)
        {
            Type type = edgeDetector.GetType();
            return type.Name.Replace("Detector", "");
        }

        public static string GetName(Type edgeDetector)
        {
            return edgeDetector.Name.Replace("Detector", "");
        }

        protected PixelMatrix Convolution(double[][] filter)
        {
            var resultMatrix = new PixelMatrix(_width, _height, _dimensions);
            int limit = (filter.GetLength(0) - 1) / 2;

            Parallel.For(limit, _height - limit, y =>
            {
                for (int x = limit; x < _width - limit; x++)
                {
                    for (int n = -limit; n <= limit; n++)
                    {
                        for (int m = -limit; m <= limit; m++)
                        {
                            for (int d = 0; d < _dimensions; d++)
                            {
                                resultMatrix[x, y, d] += _pixelMatrix[x - m, y - n, d] * filter[m + limit][n + limit];
                            }
                        }
                    }
                }
            });

            return resultMatrix;
        }

        //protected PixelMatrix Convolution(double[][] filter)
        //{
        //    var resultMatrix = new PixelMatrix(_width, _height, _dimensions);
        //    int limiter = (filter.GetLength(0) - 1) / 2;

        //    int degreeOfParallelism = Environment.ProcessorCount;
        //    int from = limiter; //0
        //    int to = _width; //length

        //    Parallel.For(0, degreeOfParallelism, workerId =>
        //    {
        //        var max = to * (workerId + 1) / degreeOfParallelism;
        //        int limitFrom = 0, limitTo = 0;
        //        if (workerId == 0)
        //        {
        //            limitFrom = limiter;
        //            limitTo = 0;
        //        }
        //        if (workerId == degreeOfParallelism - 1)
        //        {
        //            limitFrom = 0;
        //            limitTo = limiter;
        //        }

        //        for (int x = to * workerId / degreeOfParallelism + limitFrom; x < max - limitTo; x++)
        //        {
        //            for (int y = limiter; y < _height - limiter; y++)
        //            {
        //                for (int m = -limiter; m <= limiter; m++)
        //                {
        //                    for (int n = -limiter; n <= limiter; n++)
        //                    {
        //                        for (int d = 0; d < _dimensions; d++)
        //                        {
        //                            resultMatrix[x, y, d] += _pixelMatrix[x - m, y - n, d] * filter[m + limiter][n + limiter];
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    });

        //    return resultMatrix;
        //}

        protected void CutSides(int kernelSize)
        {
            int size = (int)Math.Ceiling((double)kernelSize / 2);

            var cutPixelMatrix = new PixelMatrix(_width - 2 * size, _height - 2 * size, _dimensions);

            for (int x = size, i = 0; x < _width - size; x++, i++)
            {
                for (int y = size, j = 0; y < _height - size; y++, j++)
                {
                    for (int d = 0; d < _dimensions; d++)
                    {
                        cutPixelMatrix[i, j, d] = _pixelMatrix[x, y, d];
                    }
                }
            }
            _pixelMatrix = cutPixelMatrix;
            _width -= 2 * size;
            _height -= 2 * size;
        }

        protected PixelMatrix GradientMagnitude(PixelMatrix gradientGx, PixelMatrix gradientGy)
        {
            //gradientGx.Abs();
            //gradientGy.Abs();
            //PixelMatrix gradientMagnitude = gradientGx + gradientGy;

            var gradientMagnitude = new PixelMatrix(_width, _height, _dimensions);
            int degreeOfParallelism = Environment.ProcessorCount;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = _height * (workerId + 1) / degreeOfParallelism;
                for (int y = _height * workerId / degreeOfParallelism; y < max; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int d = 0; d < _dimensions; d++)
                        {
                            gradientMagnitude[x, y, d] = Math.Sqrt(gradientGx[x, y, d] * gradientGx[x, y, d] + gradientGy[x, y, d] * gradientGy[x, y, d]);
                        }
                    }
                }
            });

            return gradientMagnitude;
        }
    }
}
