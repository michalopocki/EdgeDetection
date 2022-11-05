using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class RobertsDetector : GradientDetectorBase
    {
        public override string Name => GetName(this);
        internal readonly double[][] _Gx = new double[3][]
        {
            new double[] { 0.0, 0.0, -1.0},
            new double[] { 0.0, 1.0, 0.0 },
            new double[] { 0.0, 0.0, 0.0 }
        };
        internal readonly double[][] _Gy = new double[3][]
        {
            new double[] {-1.0, 0.0, 0.0},
            new double[] { 0.0, 1.0, 0.0 },
            new double[] { 0.0, 0.0, 0.0 }
        };

        public RobertsDetector(IGradientArgs args) : base(args){}

        public override EdgeDetectionResult DetectEdges()
        {
            if (_pixelMatrix is null)
                throw new ArgumentNullException("pixelMatrix", "PixelMatrix can not bu null");

            Prefiltration();
            PixelMatrix gradientGx = Convolution(_Gx);
            PixelMatrix gradientGy = Convolution(_Gy);
            PixelMatrix gradient = GradientMagnitude(gradientGx, gradientGy);
            gradient.Normalize();

            var imageBeforeThresholding = gradient.Bitmap;

            if (_thresholding)
            {
               gradient.Thresholding(_threshold);
            }

            var result = new EdgeDetectionResult(gradient.Bitmap, imageBeforeThresholding);

            return result;
        }
    }
}
