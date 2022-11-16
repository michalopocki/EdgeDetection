using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using EdgeDetectionLib.Kernels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class CannyDetector : EdgeDetectorBase
    {
        public override string Name => GetName(this);
        protected bool _prefiltration;
        private readonly int _gaussianKernelSize;
        private readonly double _sigma;
        private readonly int _THigh;
        private readonly int _TLow;
        internal readonly double[][] _Gx = new double[3][]
        {
            new double[] { -0.25, 0.0, 0.25},
            new double[] { -0.5 , 0.0, 0.5 },
            new double[] { -0.25, 0.0, 0.25 }
        };
        internal readonly double[][] _Gy = new double[3][]
        {
            new double[] {-0.25, -0.5, -0.25},
            new double[] { 0.0,   0.0,  0.0 },
            new double[] { 0.25,  0.5,  0.25 }
        };
        public CannyDetector(ICannyArgs args) : base(args)
        {
            _prefiltration = args.Prefiltration;
            _gaussianKernelSize = args.KernelSize;
            _sigma = args.Sigma;
            _THigh = args.THigh;
            _TLow = args.TLow;
        }
        public override EdgeDetectionResult DetectEdges()
        {
            if (_pixelMatrix is null)
            {
                throw new ArgumentNullException("pixelMatrix", "PixelMatrix can not be null");
            }

            //1) Noise Reduction - Gaussian filter
            if (_prefiltration)
            {
                IKernel gaussianKernel = new GaussianKernel(_gaussianKernelSize, _gaussianKernelSize, _sigma);
                double[][] kernel = gaussianKernel.Create();
                _pixelMatrix = Convolution(kernel);
                CutSides(_gaussianKernelSize);
            }

            //2) Finding the intensity gradient of the image
            PixelMatrix gradientGx = Convolution(_Gx);
            PixelMatrix gradientGy = Convolution(_Gy);
            PixelMatrix gradient = GradientMagnitude(gradientGx, gradientGy);
            PixelMatrix gradientDirection = GradientDirection(gradientGx, gradientGy);

            //3) Non-Maximum Suppression
            PixelMatrix nomMaximumSuppression = NonMaximumSuppression(gradient, gradientDirection); 
            nomMaximumSuppression.Normalize();
            var imageBeforeThresholding = nomMaximumSuppression.Bitmap;

            //4) Hysteresis Thresholding
            PixelMatrix hysteresisThresholding = HysteresisThresholding(nomMaximumSuppression);

            var result = new EdgeDetectionResult(hysteresisThresholding.Bitmap, imageBeforeThresholding);

            return result;
        }

        private PixelMatrix HysteresisThresholding(PixelMatrix NMS)
        {
            var hysteresisThreshold = new PixelMatrix(_width, _height, _dimensions);

            Parallel.For(1, _width - 1, x =>
            {
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < _dimensions; d++)
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
                    }
                }
            });
            return hysteresisThreshold;
        }

        private PixelMatrix NonMaximumSuppression(PixelMatrix gradient, PixelMatrix gradientDirection)
        {
            var NMS = new PixelMatrix(_width, _height, _dimensions);

            Parallel.For(1, _width - 1, x =>
            {
                double max = 0;
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < _dimensions; d++)
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
                    }
                }
            });

            return NMS;
        }

        private PixelMatrix GradientDirection(PixelMatrix gradientGx, PixelMatrix gradientGy)
        {
            var gradientDirection = new PixelMatrix(_width, _height, _dimensions);
            double toDeg = 180d / Math.PI;

            Parallel.For(0, _width, x =>
            {
                double gradDir;
                for (int y = 0; y < _height; y++)
                {
                    for (int d = 0; d < _dimensions; d++)
                    {
                        gradDir = Math.Atan2(gradientGy[x, y, d], gradientGx[x, y, d]) * toDeg;
                        //gradDir = gradDir < 0 ? gradDir + 360d : gradDir;
                        gradientDirection[x, y, d] = RoundGradientDirectionAngle(gradDir);
                    }
                }
            });

            return gradientDirection;
        }
        internal static double RoundGradientDirectionAngle(double angle)
        {
            angle = angle < 0 ? angle + 360d : angle;
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
