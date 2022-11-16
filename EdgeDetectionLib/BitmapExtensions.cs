﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace EdgeDetectionLib
{
    public static class BitmapExtensions
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public unsafe static Bitmap ToGrayscale(this Bitmap bitmap)
        {
            var result = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format8bppIndexed);
            result.SetGrayscalePalete();

            BitmapData originalBitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            BitmapData resultBitmapData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, result.PixelFormat);

            int bytesPerPixel = GetBytesPerPixel(bitmap.PixelFormat);
            int height = originalBitmapData.Height;
            int width = originalBitmapData.Width;
            byte* PtrFirstPixel = (byte*)originalBitmapData.Scan0;
            byte* PtrFirstPixelResult = (byte*)resultBitmapData.Scan0;

            Parallel.For(0, height, y =>
            {
                byte* currentLine = PtrFirstPixel + (y * originalBitmapData.Stride);
                byte* currentLineResult = PtrFirstPixelResult + (y * resultBitmapData.Stride);
                for (int x = 0; x < width; x++)
                {
                    double bluePixel = currentLine[x * bytesPerPixel];
                    double greenPixel = currentLine[x * bytesPerPixel + 1];
                    double redPixel = currentLine[x * bytesPerPixel + 2];
                    byte grayPixel = (byte)(0.114 * bluePixel + 0.587 * greenPixel + 0.299 * redPixel);

                    currentLineResult[x] = grayPixel;
                }
            });
            bitmap.UnlockBits(originalBitmapData);
            result.UnlockBits(resultBitmapData);

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

                int bytesPerPixel = GetBytesPerPixel(bitmap.PixelFormat);
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;
                byte maxRange = byte.MaxValue;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        byte bluePixel = currentLine[x];
                        byte greenPixel = currentLine[x + 1];
                        byte redPixel = currentLine[x + 2];

                        currentLine[x] = (byte)(maxRange - bluePixel);
                        currentLine[x + 1] = (byte)(maxRange - greenPixel);
                        currentLine[x + 2] = (byte)(maxRange - redPixel);
                    }
                });
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static int GetBytesPerPixel(PixelFormat pixelFormat)
        {
            return System.Drawing.Bitmap.GetPixelFormatSize(pixelFormat) / 8;
        }

        [DllImport("msvcrt.dll")]
        private static extern int memcmp(IntPtr b1, IntPtr b2, long count);
        public static bool CompareMemCmp(Bitmap b1, Bitmap b2)
        {
            if ((b1 is null) != (b2 is null)) return false;
            if (b1!.Size != b2!.Size) return false;

            var bd1 = b1.LockBits(new Rectangle(new Point(0, 0), b1.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var bd2 = b2.LockBits(new Rectangle(new Point(0, 0), b2.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            try
            {
                IntPtr bd1scan0 = bd1.Scan0;
                IntPtr bd2scan0 = bd2.Scan0;

                int stride = bd1.Stride;
                int len = stride * b1.Height;

                return memcmp(bd1scan0, bd2scan0, len) == 0;
            }
            finally
            {
                b1.UnlockBits(bd1);
                b2.UnlockBits(bd2);
            }
        }
    }
}
