using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class PrewittDetector : EdgeDetectorBase
    {
        public override string Name => "Prewitt";
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
        public PrewittDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale) { }
        public override Bitmap DetectEdges()
        {
            PixelArray gradientGx = Convolution(_Gx);
            PixelArray gradientGy = Convolution(_Gy);
            PixelArray gradient = GradientMagnitude(gradientGx, gradientGy);
            gradient.Normalize();

            return gradient.Bitmap;
        }
    }
}
