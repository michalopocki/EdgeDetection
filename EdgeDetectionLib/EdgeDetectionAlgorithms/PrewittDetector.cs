using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class PrewittDetector : GradientDetectorBase
    {
        public override string Name => GetName(this);
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
        public PrewittDetector(IGradientArgs args) : base(args) {}
        public override EdgeDetectionResult DetectEdges()
        {
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
