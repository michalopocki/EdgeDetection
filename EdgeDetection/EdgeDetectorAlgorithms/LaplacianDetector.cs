using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class LaplacianDetector : EdgeDetectorBase
    {
        public override string Name => "Laplacian";
        private readonly double alpha = 0.5;
        private readonly double[][] kernel;
        public LaplacianDetector(){}
        public LaplacianDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale)
        {
            kernel = new double[3][]
            {   
                new double[] { alpha / (alpha + 1),       (1 - alpha) / (1 + alpha),   alpha / (alpha + 1) },
                new double[] { (1 - alpha) / (1 + alpha), -4 / (alpha + 1),            (1 - alpha) / (1 + alpha) },
                new double[] { alpha / (alpha + 1),       (1 - alpha) / (1 + alpha),   alpha / (alpha + 1) }
            };
        }

        public override Bitmap DetectEdges()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            PixelArray img = Convolution(kernel);
            img.Abs();

            //img.Normalize();

            System.Diagnostics.Trace.WriteLine($"Detecting time: { watch.ElapsedMilliseconds } ms");

            return img.Bitmap;
        }
    }
}
