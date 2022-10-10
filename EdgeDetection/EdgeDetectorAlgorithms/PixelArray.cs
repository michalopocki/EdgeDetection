﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdgeDetection.EdgeDetectorAlgorithms
{
    public class PixelArray
    {
        #region Properties
        private Bitmap? _bitmap;
        public Bitmap Bitmap
        {
            get => ToBitmap();
            set => _bitmap = value;
        }
        private double[] Bits { get; set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        #endregion
        #region Constructors
        public PixelArray(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new double[width * height * 3];
            Bitmap = new Bitmap(width, height);
        }
        public PixelArray(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            Bits = new double[Width * Height * 3];
            Bitmap = bitmap;
            LoadBitmapData(bitmap);
        }
        #endregion
        #region Get and Set pixel
        public double this[int x, int y, int dimension]
        {
            //get => GetPixel(x, y, dimension);
            //set => SetPixel(x, y, dimension, value);
            get => Bits[dimension * Width * Height + y * Width + x];
            set => Bits[dimension * Width * Height + y * Width + x] = value;
        }
        public void SetPixel(int x, int y, int dimension, double value)
        {
            if (x < 0 || y < 0 ||
                x > Width || y > Height ||
                dimension < 0 || dimension > 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            int index = dimension * Width * Height + y * Width + x;
            Bits[index] = value;
        }
        public double GetPixel(int x, int y, int dimension)
        {
            if (x < 0 || y < 0 ||
                x > Width || y > Height ||
                dimension < 0 || dimension > 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            int index = dimension * Width * Height + y * Width + x;
            return Bits[index];
        }
        #endregion
        #region Methods
        public void Abs()
        {
            int length = Width * Height * 3;
            for (int i = 0; i < length; i++)
            {
                Bits[i] = Math.Abs(Bits[i]);
            }
        }
        public static PixelArray operator +(PixelArray pixelArray1, PixelArray pixelArray2)
        {
            PixelArray pixelArray = new PixelArray(pixelArray1.Width, pixelArray1.Height);
            int length = pixelArray1.Width * pixelArray1.Height * 3;
            for (int i = 0; i < length; i++)
            {
                pixelArray.Bits[i] = pixelArray1.Bits[i] + pixelArray2.Bits[i];
            }
            return pixelArray;
        }
        private unsafe void LoadBitmapData(Bitmap processedBitmap)
        {
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadOnly, processedBitmap.PixelFormat);
                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int height = bitmapData.Height;
                int width = bitmapData.Width;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, height, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < width; x++)
                    {
                        double pixelBlue = currentLine[x * bytesPerPixel];
                        double pixelGreen = currentLine[x * bytesPerPixel + 1];
                        double pixelRed = currentLine[x * bytesPerPixel + 2];

                        this[x, y, 0] = pixelRed;
                        this[x, y, 1] = pixelGreen;
                        this[x, y, 2] = pixelBlue;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }
        private unsafe Bitmap ToBitmap()
        {
            unsafe
            {
                Bitmap processedBitmap = new Bitmap(Width, Height, PixelFormat.Format48bppRgb);
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.WriteOnly, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                    {
                        double pixelBlue = this[x / bytesPerPixel, y, 2];
                        double pixelGreen = this[x / bytesPerPixel, y, 1];
                        double pixelRed = this[x / bytesPerPixel, y, 0];

                        currentLine[x] = (byte)pixelBlue;
                        currentLine[x + 1] = (byte)pixelGreen;
                        currentLine[x + 2] = (byte)pixelRed;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
                return processedBitmap;
            }
        }
        #endregion
    }
}
