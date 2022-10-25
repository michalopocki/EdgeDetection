using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EdgeDetectionLib
{
    public static class BitmapExtensions
    {
        public static Bitmap MakeGrayscale(this Bitmap original)
        {
            var newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            var colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });
            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();

            return newBitmap;
        }

        public static Bitmap ToGrayscale(this Bitmap bitmap)
        {
            var result = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format8bppIndexed);
            result.SetGrayscalePalete();

            BitmapData data = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            byte[] bytes = new byte[data.Height * data.Stride];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var c = bitmap.GetPixel(x, y);
                    var grayPixel = (byte)(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);

                    bytes[y * data.Stride + x] = grayPixel;
                }
            }

            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            result.UnlockBits(data);

            return result;
        }

        public static void SetGrayscalePalete(this Bitmap bitmap)
        {
            var resultPalette = bitmap.Palette;

            for (int i = 0; i < 256; i++)
            {
                resultPalette.Entries[i] = Color.FromArgb(255, i, i, i);
            }

            bitmap.Palette = resultPalette;
        }

        public static unsafe void MakeNegative(this Bitmap bitmap)
        {
            unsafe
            {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        int bluePixel = currentLine[x];
                        int greenPixel = currentLine[x + 1];
                        int redPixel = currentLine[x + 2];

                        currentLine[x] = (byte)(Math.Abs(bluePixel - 255));
                        currentLine[x + 1] = (byte)(Math.Abs(greenPixel - 255));
                        currentLine[x + 2] = (byte)(Math.Abs(redPixel - 255));
                    }
                });
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
