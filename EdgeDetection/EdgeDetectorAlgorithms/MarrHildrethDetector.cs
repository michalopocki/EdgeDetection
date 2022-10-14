﻿using EdgeDetectionApp.EdgeDetectorAlgorithms.Kernels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class MarrHildrethDetector : EdgeDetectorBase
    {
        public override string Name => "Marr-Hildreth";
        private readonly int _LoGKernelSize;
        private readonly double _sigma;
        public MarrHildrethDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale)
        {
            _sigma = 1.5;
            _LoGKernelSize = 5;
        }
        public MarrHildrethDetector() { }

        public override Bitmap DetectEdges()
        {
            IKernel LoGKernel = new LaplacianOfGaussianKernel(_LoGKernelSize, _LoGKernelSize, _sigma);
            double[][] kernel = LoGKernel.Create();

            PixelArray img = Convolution(kernel);
            PixelArray imgZeroCrossing = ZeroCrossing(img);

            return imgZeroCrossing.Bitmap;
        }
        private PixelArray ZeroCrossing(PixelArray pixelArray)
        {
            var resultArray = new PixelArray(_width, _height);
            double avar = 0.5 * pixelArray.Mean();

            Parallel.For(1, _width - 1, x =>
            {
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        if (pixelArray[x, y, d] < 0 && pixelArray[x + 1, y, d] > 0 &&
                           (Math.Abs(pixelArray[x + 1, y, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                        }
                        else if (pixelArray[x, y, d] < 0 && pixelArray[x - 1, y, d] > 0 &&
                           (Math.Abs(pixelArray[x - 1, y, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                        }
                        else if (pixelArray[x, y, d] < 0 && pixelArray[x, y - 1, d] > 0 &&
                           (Math.Abs(pixelArray[x, y - 1, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                        }
                        else if (pixelArray[x, y, d] < 0 && pixelArray[x, y + 1, d] > 0 &&
                           (Math.Abs(pixelArray[x, y + 1, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                        }
                        if (_isGrayscale) 
                            break;
                    }
                    if (_isGrayscale)
                        resultArray[x, y, 2] = resultArray[x, y, 1] = resultArray[x, y, 0];
                }
            });
            return resultArray;
        }
 
    }
}
