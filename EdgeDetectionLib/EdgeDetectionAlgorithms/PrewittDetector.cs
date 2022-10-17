using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class PrewittDetector : EdgeDetectorBase
    {
        public override string Name => "Prewitt";
        private readonly bool _thresholding;
        private readonly int _threshold;
        private readonly double[][] _Gx = new double[3][]
        {
            new double[] { 1 / 3.0, 0 / 3.0, -1 / 3.0},
            new double[] { 1 / 3.0, 0 / 3.0, -1 / 3.0 },
            new double[] { 1 / 3.0, 0 / 3.0, -1 / 3.0 }
        };
        private readonly double[][] _Gy = new double[3][]
        {
            new double[] {  1 / 3.0,  1 / 3.0,  1 / 3.0 },
            new double[] {  0 / 3.0,  0 / 3.0,  0 / 3.0 },
            new double[] { -1 / 3.0, -1 / 3.0, -1 / 3.0 }
        };
        public PrewittDetector(){}
        public PrewittDetector(GradientArgs args) : base(args) 
        {
            _thresholding = args.Thresholding;
            _threshold = args.Threshold;
        }
        public override Bitmap DetectEdges()
        {
            PixelArray gradientGx = Convolution(_Gx);
            PixelArray gradientGy = Convolution(_Gy);
            PixelArray gradient = GradientMagnitude(gradientGx, gradientGy);
            gradient.Normalize();
            BeforeThresholdingBitmap = gradient.Bitmap;

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            return gradient.Bitmap;
        }
    }
}
