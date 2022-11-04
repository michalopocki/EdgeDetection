﻿using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
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
        protected EdgeDetectionResult _result = new();
        protected PixelMatrix _pixelMatrix;
        protected int _width;
        protected int _height;
        protected int _dimensions;

        public EdgeDetectorBase() { }
        public EdgeDetectorBase(IEdgeDetectorArgs args)
        {
            if (args.ImageToProcess is not null)
            {
                _pixelMatrix = new PixelMatrix(args.ImageToProcess);
                _width = args.ImageToProcess.Width;
                _height = args.ImageToProcess.Height;
                _dimensions = args.ImageToProcess.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ? 1 : 3;
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

        protected PixelMatrix Convolution(double[][] filter)
        {
            var resultMatrix = new PixelMatrix(_width, _height, _dimensions);
            int limiter = (filter.GetLength(0) - 1) / 2;

            Parallel.For(limiter, _width - limiter, x =>
            {
                for (int y = limiter; y < _height - limiter; y++)
                {
                    for (int m = -limiter; m <= limiter; m++)
                    {
                        for (int n = -limiter; n <= limiter; n++)
                        {
                            for (int d = 0; d < _dimensions; d++)
                            {
                                resultMatrix[x, y, d] += _pixelMatrix[x - m, y - n, d] * filter[m + limiter][n + limiter];
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
                var max = _width * (workerId + 1) / degreeOfParallelism;
                for (int x = _width * workerId / degreeOfParallelism; x < max; x++)
                {
                    for (int y = 0; y < _height; y++)
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
