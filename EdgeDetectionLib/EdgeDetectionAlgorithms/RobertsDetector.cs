using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class RobertsDetector : GradientDetectorBase
    {
        public override string Name => "Roberts";
        private readonly double[][] _Gx = new double[3][]
        {
            new double[] { 0.0, 0.0, -1.0},
            new double[] { 0.0, 1.0, 0.0 },
            new double[] { 0.0, 0.0, 0.0 }
        };
        private readonly double[][] _Gy = new double[3][]
        {
            new double[] {-1.0, 0.0, 0.0},
            new double[] { 0.0, 1.0, 0.0 },
            new double[] { 0.0, 0.0, 0.0 }
        };

        public RobertsDetector(){}
        public RobertsDetector(GradientArgs args) : base(args){}

        public override EdgeDetectionResult DetectEdges()
        {
            PixelMatrix gradientGx = Convolution(_Gx);
            PixelMatrix gradientGy = Convolution(_Gy);
            PixelMatrix gradient = GradientMagnitude(gradientGx, gradientGy);
            gradient.Normalize();
            _result.ImageBeforeThresholding = gradient.Bitmap;

            if (_thresholding)
            {
               gradient.Thresholding(_threshold);
            }
            _result.ProcessedImage = gradient.Bitmap;

            return _result;
        }
    }
}
