using EdgeDetectionApp.EdgeDetectorAlgorithms.Kernels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class CannyDetector : EdgeDetectorBase
    {
        public override string Name => "Canny";
        private readonly int _gaussianKernelSize;
        private readonly double _sigma;
        private readonly int _THigh;
        private readonly int _TLow;
        private readonly double[][] _Gx = new double[3][]
{
            new double[] { -0.25, 0.0, 0.25},
            new double[] { -0.5 , 0.0, 0.5 },
            new double[] { -0.25, 0.0, 0.25 }
};
        private readonly double[][] _Gy = new double[3][]
        {
            new double[] {-0.25, -0.5, -0.25},
            new double[] { 0.0,   0.0,  0.0 },
            new double[] { 0.25,  0.5,  0.25 }
        };
        public CannyDetector() { }
        public CannyDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale)
        {
            _gaussianKernelSize = 6;
            _sigma = 3.0;
            _THigh = 45;
            _TLow = 19;
        }
        public override Bitmap DetectEdges()
        {
            //1) Noise Reduction - Gaussian filter
            IKernel gaussianKernel = new GaussianKernel(_gaussianKernelSize, _gaussianKernelSize, _sigma);
            double[][] kernel = gaussianKernel.Create();
            _PixelArray = Convolution(kernel);
            CutSides();

            //2) Finding the intensity gradient of the image
            PixelArray gradientGx = Convolution(_Gx);
            PixelArray gradientGy = Convolution(_Gy);
            PixelArray gradient = GradientMagnitude(gradientGx, gradientGy);
            PixelArray gradientDirection = GradientDirection(gradientGx, gradientGy);

            //3) Non-Maximum Suppression
            PixelArray nomMaximumSuppression = NonMaximumSuppression(gradient, gradientDirection); 
            nomMaximumSuppression.Normalize();

            //4) Hysteresis Thresholding
            PixelArray hysteresisThresholding = HysteresisThresholding(nomMaximumSuppression);

            return hysteresisThresholding.Bitmap;
        }
        private void CutSides()
        {
            int size = (int)Math.Ceiling((double)_gaussianKernelSize / 2);
            var cutPixelArray = new PixelArray(_width - 2 * size, _height - 2 * size);

            for (int x = size, i = 0; x < _width - size; x++, i++)
            {
                for (int y = size, j = 0; y < _height - size; y++, j++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        cutPixelArray[i, j, d] = _PixelArray[x, y, d];

                        if (_isGrayscale)
                            break;
                    }
                    if (_isGrayscale)
                        cutPixelArray[i, j, 2] = cutPixelArray[i, j, 1] = cutPixelArray[i, j, 0];
                }
            }
            _PixelArray = cutPixelArray;
            _width -= 2 * size;
            _height -= 2 * size;
        }
        //private double[] Max(PixelArray array)
        //{
        //    double[] max = new double[3];

        //    Parallel.For(0, _width, x =>
        //    {
        //        for (int y = 0; y < _height; y++)
        //        {
        //            for (int d = 0; d < 3; d++)
        //            {
        //                if (array[x, y, d] > max[d])
        //                    max[d] = array[x, y, d];
        //            }
        //        }
        //    });
        //    return max;
        //}
        private PixelArray HysteresisThresholding(PixelArray NMS)
        {
            var hysteresisThreshold = new PixelArray(_width, _height);

            Parallel.For(1, _width - 1, x =>
            {
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        if (NMS[x, y, d] < _TLow)
                        {
                            hysteresisThreshold[x, y, d] = 0d;
                        }
                        else if (NMS[x, y, d] > _THigh)
                        {
                            hysteresisThreshold[x, y, d] = 255d;
                        }
                        else if (NMS[x + 1, y, d] > _THigh ||
                                 NMS[x - 1, y, d] > _THigh ||
                                 NMS[x, y + 1, d] > _THigh ||
                                 NMS[x, y - 1, d] > _THigh ||
                                 NMS[x - 1, y - 1, d] > _THigh ||
                                 NMS[x + 1, y + 1, d] > _THigh ||
                                 NMS[x - 1, y + 1, d] > _THigh ||
                                 NMS[x + 1, y - 1, d] > _THigh)
                        {
                            hysteresisThreshold[x, y, d] = 255d;
                        }

                        if (_isGrayscale)
                            break;
                    }
                    if (_isGrayscale)
                        hysteresisThreshold[x, y, 2] = hysteresisThreshold[x, y, 1] = hysteresisThreshold[x, y, 0];
                }
            });
            return hysteresisThreshold;
        }

        private PixelArray NonMaximumSuppression(PixelArray gradient, PixelArray gradientDirection)
        {
            var NMS = new PixelArray(_width, _height);

            Parallel.For(1, _width - 1, x =>
            {
                double max = 0;
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        if (gradientDirection[x, y, d] == 0.0)
                        {
                            max = new double[] { Math.Round(gradient[x, y, d]), Math.Round(gradient[x, y + 1, d]), Math.Round(gradient[x, y - 1, d]) }.Max();
                        }
                        else if (gradientDirection[x, y, d] == 45.0)
                        {
                            max = new double[] { Math.Round(gradient[x, y, d]), Math.Round(gradient[x + 1, y - 1, d]), Math.Round(gradient[x - 1, y + 1, d]) }.Max();
                        }
                        else if (gradientDirection[x, y, d] == 90.0)
                        {
                            max = new double[] { Math.Round(gradient[x, y, d]), Math.Round(gradient[x + 1, y, d]), Math.Round(gradient[x - 1, y, d]) }.Max();
                        }
                        else if (gradientDirection[x, y, d] == 135.0)
                        {
                            max = new double[] { Math.Round(gradient[x, y, d]), Math.Round(gradient[x + 1, y + 1, d]), Math.Round(gradient[x - 1, y - 1, d]) }.Max();
                        }

                        if (Math.Round(gradient[x, y, d]) == max)
                        {
                            NMS[x, y, d] = gradient[x, y, d];
                        }

                        if (_isGrayscale)
                            break;
                    }
                    if (_isGrayscale)
                        NMS[x, y, 2] = NMS[x, y, 1] = NMS[x, y, 0];
                }
            });

            return NMS;
        }

        private PixelArray GradientDirection(PixelArray gradientGx, PixelArray gradientGy)
        {
            var gradientDirection = new PixelArray(_width, _height);
            double toDeg = 180d / Math.PI;

            Parallel.For(0, _width, x =>
            {
                double gradDir;
                for (int y = 0; y < _height; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        gradDir = Math.Atan2(gradientGy[x, y, d], gradientGx[x, y, d]) * toDeg;
                        gradDir = gradDir < 0 ? gradDir + 360d : gradDir;
                        gradientDirection[x, y, d] = RoundGradientDirectionAngle(gradDir);

                        if (_isGrayscale)
                            break;
                    }
                    if (_isGrayscale)
                        gradientDirection[x, y, 2] = gradientDirection[x, y, 1] = gradientDirection[x, y, 0];
                }
            });

            return gradientDirection;
        }
        private static double RoundGradientDirectionAngle(double angle)
        {
            double roundedAngle = 0;
            if ((angle >= 0 && angle < 22.5) || (angle >= 157.5 && angle < 202.5) || (angle >= 337.5 && angle <= 360))
            {
                roundedAngle = 0.0;
            }
            else if ((angle >= 22.5 && angle < 67.5) || (angle >= 202.5 && angle < 247.5))
            {
                roundedAngle = 45.0;
            }
            else if ((angle >= 67.5 && angle < 112.5) || (angle >= 247.5 && angle < 292.5))
            {
                roundedAngle = 90.0;
            }
            else if ((angle >= 112.5 && angle < 157.5) || (angle >= 292.5 && angle < 337.5))
            {
                roundedAngle = 135.0;
            }
            return roundedAngle;
        }
    }
}
