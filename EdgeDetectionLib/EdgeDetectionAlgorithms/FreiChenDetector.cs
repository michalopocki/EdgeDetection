using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class FreiChenDetector : GradientDetectorBase
    {
        public override string Name => GetName(this);
        internal readonly double[][] _Gx = new double[3][]
        {
            new double[] { -1 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)), -1 / (2 + Sqrt(2)) },
            new double[] {  0 / (2 + Sqrt(2)),        0 / (2 + Sqrt(2)),  0 / (2 + Sqrt(2)) },
            new double[] {  1 / (2 + Sqrt(2)),  Sqrt(2) / (2 + Sqrt(2)),  1 / (2 + Sqrt(2)) }
        };
        internal readonly double[][] _Gy = new double[3][]
        {
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) },
            new double[] { Sqrt(2) / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)) },
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) }
        };
        public FreiChenDetector(IGradientArgs args) : base(args) {}
        public override EdgeDetectionResult DetectEdges()
        {
            if (_pixelMatrix is null)
            {
                throw new ArgumentNullException("pixelMatrix", "PixelMatrix can not be null");
            }

            Prefiltration();
            PixelMatrix gradientGx = Convolution(_Gx);
            PixelMatrix gradientGy = Convolution(_Gy);
            PixelMatrix gradient = GradientMagnitude(gradientGx, gradientGy);
            gradient.Normalize();

            var imageBeforeThresholding = (Bitmap)gradient.Bitmap.Clone();

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            var result = new EdgeDetectionResult(gradient.Bitmap, imageBeforeThresholding);

            return result;
        }

    }
}
