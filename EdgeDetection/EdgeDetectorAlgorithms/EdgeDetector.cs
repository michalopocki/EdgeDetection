using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public abstract class EdgeDetector
    {
        //protected int _width;
        //protected int _height;
        //protected bool _greyscale;
        //protected abstract Pixel[,] PixelArray { get; set; }
        //protected abstract int Width { get; set; }
        //protected abstract int Height { get; set; }
        //protected abstract bool Greyscale { get; set; }
        //protected EdgeDetector(Bitmap originalImg)
        //{
        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    PixelArray = originalImg.BitmapToDoubleArray();
        //    watch.Stop();
        //    System.Diagnostics.Trace.WriteLine("Bitmap to pixel arr:" + watch.ElapsedMilliseconds + " ms");
        //    Width = originalImg.Width;
        //    Height = originalImg.Height;
        //    Greyscale = false;
        //}
        //public abstract Bitmap DetectEdges();

        //protected Pixel[,] Convolution(double[,] filter)
        //{
        //    Pixel[,] resultImg = new Pixel[Width, Height];
        //    int limiter = (filter.GetLength(0) - 1) / 2;

        //    for (int x = limiter; x < Width - limiter; x++)
        //    {
        //        for (int y = limiter; y < Height - limiter; y++)
        //        {
        //            for (int m = -limiter; m <= limiter; m++)
        //            {
        //                for (int n = -limiter; n <= limiter; n++)
        //                {
        //                    if (!Greyscale)
        //                        resultImg[x, y] += PixelArray[x - m, y - n] * filter[m + limiter, n + limiter];
        //                    else
        //                        resultImg[x, y].R += PixelArray[x - m, y - n].R * filter[m + limiter, n + limiter];
        //                }
        //            }
        //        }
        //    }
        //    return resultImg;
        //}
        //protected Pixel[,] Magnitude(Pixel[,] imgGx, Pixel[,] imgGy)
        //{
        //    Pixel[,] magnitudeImg = new Pixel[Width, Height];
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            if (!Greyscale)
        //                magnitudeImg[x, y] = Pixel.Sqrt(imgGx[x, y] * imgGx[x, y] + imgGy[x, y] * imgGy[x, y]);
        //            else
        //                magnitudeImg[x, y].R = Math.Sqrt(imgGx[x, y].R * imgGx[x, y].R + imgGy[x, y].R * imgGy[x, y].R);
        //        }
        //    }
        //    return magnitudeImg;
        //}
        //protected Pixel[,] Thresholing(int threshold)
        //{
        //    Pixel[,] thresholdingImg = new Pixel[Width, Height];
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            if (!Greyscale)
        //                thresholdingImg[x, y] = Pixel.Thresholing(PixelArray[x, y], threshold);
        //            else
        //                thresholdingImg[x,y].R = PixelArray[x, y].R <= threshold ? 0 : 255;
        //        }
        //    }
        //    return thresholdingImg;
        //}
        //protected Pixel[,] Negative()
        //{
        //    Pixel[,] negativeImg = new Pixel[Width, Height];
        //    for (int x = 0; x < Width; x++)
        //    {
        //        for (int y = 0; y < Height; y++)
        //        {
        //            if (!Greyscale)
        //                negativeImg[x, y] = Pixel.Abs(PixelArray[x, y] - 255);
        //            else
        //                negativeImg[x, y].R = Math.Abs(PixelArray[x, y].R - 255);
        //        }
        //    }
        //    return negativeImg;
        //}
    }
}
