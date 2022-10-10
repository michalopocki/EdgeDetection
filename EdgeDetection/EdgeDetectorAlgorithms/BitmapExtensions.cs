using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetection.EdgeDetectorAlgorithms
{
    public static class BitmapExtensions
    {
        public static Bitmap MakeGrayscale(this Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();

            return newBitmap;
        }
        public static unsafe void ProcessUsingLockbitsAndUnsafeAndParallel(this Bitmap processedBitmap)
        {
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        currentLine[x] = (byte)oldBlue;
                        currentLine[x + 1] = (byte)oldGreen;
                        currentLine[x + 2] = (byte)oldRed;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }
        public static Pixel[,] BitmapToDoubleArray(this Bitmap bitmap)
        {
            Pixel[,] image = new Pixel[bitmap.Width, bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    image[x, y] = bitmap.GetPixel(x, y);
                }
            }
            return image;
        }
        public static Bitmap DoubleArrayToBitmap(Pixel[,] array, bool greyscale)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            double maxValue = MaxValue(array);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    //bitmap.SetPixel(x, y, Color.FromArgb(DoubleToImage(array[x, y].R, maxValue),
                    //                                     DoubleToImage(array[x, y].G, maxValue),
                    //                                     DoubleToImage(array[x, y].B, maxValue)));
                    if (!greyscale)
                        bitmap.SetPixel(x, y, Color.FromArgb((int)array[x, y].R,
                                                             (int)array[x, y].G,
                                                             (int)array[x, y].B));
                    else
                        bitmap.SetPixel(x, y, Color.FromArgb((int)array[x, y].R,
                                                             (int)array[x, y].R,
                                                             (int)array[x, y].R));
                }
            }
            return bitmap;
        }
        private static int DoubleToImage(double oldPixel, double maxValue)
        {
            if (oldPixel > 0)
                return (int)(255 * oldPixel / maxValue);
            return 0;
        }
        private static double MaxValue(Pixel[,] array)
        {
            double maxValue = 0;
            foreach (Pixel pixel in array)
            {
                if (pixel.R > maxValue) maxValue = pixel.R;
            }
            return maxValue;
        }
    }
}
