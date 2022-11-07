using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class KirschDetector : GradientDetectorBase
    {
        public override string Name => GetName(this);
        internal readonly double[][] _W = new double[3][]
        {
            new double[] { 5, -3, -3 },
            new double[] { 5,  0, -3 },
            new double[] { 5, -3, -3 }
        };
        internal readonly double[][] _SW = new double[3][]
        {
            new double[] { -3, -3, -3 },
            new double[] {  5,  0, -3 },
            new double[] {  5,  5, -3 }
        };
        internal readonly double[][] _S = new double[3][]
        {
            new double[] { -3, -3, -3 },
            new double[] { -3,  0, -3 },
            new double[] {  5,  5,  5 }
        };
        internal readonly double[][] _SE = new double[3][]
        {
            new double[] { -3, -3, -3 },
            new double[] { -3,  0,  5 },
            new double[] { -3,  5,  5 }
        };
        internal readonly double[][] _E = new double[3][]
        {
            new double[] { -3, -3,  5 },
            new double[] { -3,  0,  5 },
            new double[] { -3, -3,  5 }
        };
        internal readonly double[][] _NE = new double[3][]
        {
            new double[] { -3,  5,  5 },
            new double[] { -3,  0,  5 },
            new double[] { -3, -3, -3 }
        };
        internal readonly double[][] _N = new double[3][]
        {
            new double[] {  5,  5,  5 },
            new double[] { -3,  0, -3 },
            new double[] { -3, -3, -3 }
        };
        internal readonly double[][] _NW = new double[3][]
        {
            new double[] {  5,  5, -3 },
            new double[] {  5,  0, -3 },
            new double[] { -3, -3, -3 }
        };
        public KirschDetector(IGradientArgs args) : base(args){}
        public override EdgeDetectionResult DetectEdges()
        {
            if (_pixelMatrix is null)
            {
                throw new ArgumentNullException("pixelMatrix", "PixelMatrix can not be null");
            }

            Prefiltration();

            PixelMatrix[] gradientMagnitudes = new PixelMatrix[8];
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
            PixelMatrix gradient = FindMaxMagnitude(gradientMagnitudes);
            gradient.Normalize();

            var imageBeforeThresholding = gradient.Bitmap;

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            var result = new EdgeDetectionResult(gradient.Bitmap, imageBeforeThresholding);

            return result;
        }

        private PixelMatrix FindMaxMagnitude(params PixelMatrix[] pixelMatrixs)
        {
            var gradientMagnitude = new PixelMatrix(_width, _height, _dimensions);

            Parallel.For(0, _width, x =>
            {
                double maxMagnitude = double.MinValue;
                for (int y = 0; y < _height; y++)
                {
                    for (int d = 0; d < _dimensions; d++)
                    {
                        foreach (var pixelMatrix in pixelMatrixs)
                        {
                            if (pixelMatrix[x, y, d] > maxMagnitude)
                                maxMagnitude = pixelMatrix[x, y, d];
                        }
                        gradientMagnitude[x, y, d] = maxMagnitude;
                        maxMagnitude = double.MinValue;
                    }
                }
            });
            return gradientMagnitude;
        }
    }
}
