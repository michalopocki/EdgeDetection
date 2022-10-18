using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class KirschDetector : EdgeDetectorBase
    {
        public override string Name => "Kirsch";
        private readonly bool _thresholding;
        private readonly int _threshold;
        private readonly double[][] _W = new double[3][]
        {
            new double[] { 5, -3, -3 },
            new double[] { 5,  0, -3 },
            new double[] { 5, -3, -3 }
        };
        private readonly double[][] _SW = new double[3][]
        {
            new double[] { -3, -3, -3 },
            new double[] {  5,  0, -3 },
            new double[] {  5,  5, -3 }
        };
        private readonly double[][] _S = new double[3][]
        {
            new double[] { -3, -3, -3 },
            new double[] { -3,  0, -3 },
            new double[] {  5,  5,  5 }
        };
        private readonly double[][] _SE = new double[3][]
        {
            new double[] { -3, -3, -3 },
            new double[] { -3,  0,  5 },
            new double[] { -3,  5,  5 }
        };
        private readonly double[][] _E = new double[3][]
        {
            new double[] { -3, -3,  5 },
            new double[] { -3,  0,  5 },
            new double[] { -3, -3,  5 }
        };
        private readonly double[][] _NE = new double[3][]
        {
            new double[] { -3,  5,  5 },
            new double[] { -3,  0,  5 },
            new double[] { -3, -3, -3 }
        };
        private readonly double[][] _N = new double[3][]
        {
            new double[] {  5,  5,  5 },
            new double[] { -3,  0, -3 },
            new double[] { -3, -3, -3 }
        };
        private readonly double[][] _NW = new double[3][]
        {
            new double[] {  5,  5, -3 },
            new double[] {  5,  0, -3 },
            new double[] { -3, -3, -3 }
        };
        public KirschDetector(){}
        public KirschDetector(GradientArgs args) : base(args)
        {
            _thresholding = args.Thresholding;
            _threshold = args.Threshold;
        }
        public override Bitmap DetectEdges()
        {
            PixelArray[] gradientMagnitudes = new PixelArray[8];
            gradientMagnitudes[0] = Convolution(_W);
            gradientMagnitudes[1] = Convolution(_SW); 
            gradientMagnitudes[2] = Convolution(_S); 
            gradientMagnitudes[3] = Convolution(_SE); 
            gradientMagnitudes[4] = Convolution(_E); 
            gradientMagnitudes[5] = Convolution(_NE); 
            gradientMagnitudes[6] = Convolution(_N); 
            gradientMagnitudes[7] = Convolution(_NW); 

            foreach (var gradMag in gradientMagnitudes)
            {
                gradMag.Abs();
            }
            PixelArray gradient = FindMaxMagnitude(gradientMagnitudes);
            gradient.Normalize();

            BeforeThresholdingBitmap = gradient.Bitmap;

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            return gradient.Bitmap;
        }
        private PixelArray FindMaxMagnitude(params PixelArray[] pixelArrays)
        {
            var gradientMagnitude = new PixelArray(_width, _height);

            Parallel.For(0, _width, x =>
            {
                double maxMagnitude = double.MinValue;
                for (int y = 0; y < _height; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        foreach (var pixelArray in pixelArrays)
                        {
                            if (pixelArray[x, y, d] > maxMagnitude)
                                maxMagnitude = pixelArray[x, y, d];
                        }
                        gradientMagnitude[x, y, d] = maxMagnitude;
                        maxMagnitude = double.MinValue;
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
    }
}
