using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Math;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class FreiChenDetector : EdgeDetectorBase
    {
        public override string Name => "Frei-Chen";
        private readonly double[][] _Gx = new double[3][]
        {
            new double[] { -1 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)), -1 / (2 + Sqrt(2)) },
            new double[] {  0 / (2 + Sqrt(2)),        0 / (2 + Sqrt(2)),  0 / (2 + Sqrt(2)) },
            new double[] {  1 / (2 + Sqrt(2)),  Sqrt(2) / (2 + Sqrt(2)),  1 / (2 + Sqrt(2)) }
        };
        private readonly double[][] _Gy = new double[3][]
        {
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) },
            new double[] { Sqrt(2) / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)) },
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) }
        };
        public FreiChenDetector() { }
        public FreiChenDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale) { }
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
