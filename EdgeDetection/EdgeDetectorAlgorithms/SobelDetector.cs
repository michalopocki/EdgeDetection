using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class SobelDetector : EdgeDetectorBase
    {
        public override string Name => "Sobel";
        private readonly double[][] Gx = new double[3][]
        {
            new double[] { -0.25, 0.0, 0.25},
            new double[] { -0.5 , 0.0, 0.5 },
            new double[] { -0.25, 0.0, 0.25 }
        };
        private readonly double[][] Gy = new double[3][]
        {
            new double[] {-0.25, -0.5, -0.25},
            new double[] { 0.0,   0.0,  0.0 },
            new double[] { 0.25,  0.5,  0.25 }
        };
        public SobelDetector(){ }
        public SobelDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale) { }
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
