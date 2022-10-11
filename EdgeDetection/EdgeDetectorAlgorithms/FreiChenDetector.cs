using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class FreiChenDetector : EdgeDetectorBase
    {
        public override string Name => "Frei-Chen";
        private readonly double[][] Gx = new double[3][]
        {
            new double[] { -1 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)), -1 / (2 + Sqrt(2)) },
            new double[] {  0 / (2 + Sqrt(2)),        0 / (2 + Sqrt(2)),  0 / (2 + Sqrt(2)) },
            new double[] {  1 / (2 + Sqrt(2)),  Sqrt(2) / (2 + Sqrt(2)),  1 / (2 + Sqrt(2)) }
        };
        private readonly double[][] Gy = new double[3][]
        {
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) },
            new double[] { Sqrt(2) / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)) },
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) }
        };
        public FreiChenDetector() { }
        public FreiChenDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale) { }
        public override Bitmap DetectEdges()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            PixelArray imgGx = Convolution(Gx);
            PixelArray imgGy = Convolution(Gy);
            watch.Stop();
            PixelArray magnitude = Magnitude(imgGx, imgGy);

            System.Diagnostics.Trace.WriteLine("Convolution:" + watch.ElapsedMilliseconds + " ms");

            return magnitude.Bitmap;
        }
    }
}
