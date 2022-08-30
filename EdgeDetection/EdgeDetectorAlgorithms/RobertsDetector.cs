using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetection.EdgeDetectorAlgorithms
{
    public class RobertsDetector : EdgeDetector
    {
        protected override Pixel[,] PixelArray { get; set; }
        protected override int Width { get => _width; set => _width = value; }
        protected override int Height { get => _height; set => _height = value; }
        protected override bool Greyscale { get => _greyscale; set => _greyscale = value; }

        private readonly double[,] Gx = new double[3, 3]
        {
             {0, 0, -1 },
             {0, 1, 0 },
             {0, 0, 0 }
        };
        private readonly double[,] Gy = new double[3, 3]
        {
             {-1, 0, 0 },
             {0, 1, 0 },
             {0, 0, 0 }
        };

        public RobertsDetector(Bitmap originalImg) : base(originalImg){}
        public Bitmap DoNothing()
        {
            return BitmapExtensions.DoubleArrayToBitmap(PixelArray, false);
        }
        public override Bitmap MakeGreyscale()
        {
            PixelArray = ImageInGreyscale();
            Greyscale = true;
            return BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
        }
        public override Bitmap DetectEdges()
        {
            Pixel[,] imgGx = Convolution(Gx);
            Pixel[,] imgGy = Convolution(Gy);
            PixelArray = Magnitude(imgGx, imgGy);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Bitmap bmp = BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
            //return BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
            watch.Stop();
            System.Diagnostics.Trace.WriteLine("PixelArr to Bitmap:" + watch.ElapsedMilliseconds + " ms");
            return bmp;
        }
        public override Bitmap ApplyThresholding(int threshold)
        {
            PixelArray = Thresholing(threshold);
            return BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
        }
        public override Bitmap MakeNegative()
        {
            PixelArray = Negative();
            return BitmapExtensions.DoubleArrayToBitmap(PixelArray, Greyscale);
        }
    }
}
