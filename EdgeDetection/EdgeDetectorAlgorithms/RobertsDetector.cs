using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetection.EdgeDetectorAlgorithms
{
    public class RobertsDetector : EdgeDetectorBase
    {
        public override string Name => "Roberts";
        private readonly double[][] Gx = new double[3][]
        {
            new double[] { 0, 0, -1},
            new double[] { 0, 1, 0 },
            new double[] { 0, 0, 0 }
        };
        private readonly double[][] Gy = new double[3][]
        {
            new double[] {-1, 0, 0 },
            new double[] { 0, 1, 0 },
            new double[] { 0, 0, 0 }
        };

        public RobertsDetector(){}
        public RobertsDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale) {}

        public override Bitmap DetectEdges()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            PixelArray imgGx = Convolution(Gx);
            PixelArray imgGy = Convolution(Gy);
            PixelArray magnitude = Magnitude(imgGx, imgGy);

            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Convolution:" + watch.ElapsedMilliseconds + " ms");


            return imgGx.Bitmap;
        }




        //public RobertsDetector(Bitmap originalImg) : base(originalImg){}

        //public override Bitmap DetectEdges()
        //{
        //    Pixel[,] imgGx = Convolution(Gx);
        //    Pixel[,] imgGy = Convolution(Gy);
        //    PixelArray = Magnitude(imgGx, imgGy);
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    Bitmap bmp = BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
        //    //return BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
        //    watch.Stop();
        //    System.Diagnostics.Trace.WriteLine("PixelArr to Bitmap:" + watch.ElapsedMilliseconds + " ms");
        //    return bmp;
        //}
    }
}
