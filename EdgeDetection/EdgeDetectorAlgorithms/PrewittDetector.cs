using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class PrewittDetector : EdgeDetectorBase
    {
        public override string Name => "Prewitt";
        private readonly double[][] Gx = new double[3][]
        {
            new double[] { 1 / 3.0, 0 / 3.0, -1 / 3.0},
            new double[] { 1 / 3.0, 0 / 3.0, -1 / 3.0 },
            new double[] { 1 / 3.0, 0 / 3.0, -1 / 3.0 }
        };
        private readonly double[][] Gy = new double[3][]
        {
            new double[] {  1 / 3.0,  1 / 3.0,  1 / 3.0 },
            new double[] {  0 / 3.0,  0 / 3.0,  0 / 3.0 },
            new double[] { -1 / 3.0, -1 / 3.0, -1 / 3.0 }
        };
        public PrewittDetector(){}
        public PrewittDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale) { }
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
