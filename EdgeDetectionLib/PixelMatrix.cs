using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Media3D;

namespace EdgeDetectionLib
{
    public class PixelMatrix
    {
        #region Fields
        internal double[] Bits { get; set; }
        #endregion

        #region Properties
        public Bitmap Bitmap => ToBitmap();
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int Dimensions { get; private set; }
        #endregion

        #region Constructors
        public PixelMatrix(int width, int height, int dimensions)
        {
            Width = width;
            Height = height;
            Dimensions = dimensions;
            Bits = new double[width * height * dimensions];
        }

        public PixelMatrix(Bitmap bitmap)
        {
            if (bitmap is not null)
            {
                Width = bitmap.Width;
                Height = bitmap.Height;
                Dimensions = BitmapExtensions.GetBytesPerPixel(bitmap.PixelFormat);
                Bits = new double[Width * Height * Dimensions];
                LoadBitmapData(bitmap);
            }
        }

        #endregion

        #region Get and Set pixel
        public double this[int x, int y, int dimension]
        {
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
            int length = Width * Height * Dimensions;
            int degreeOfParallelism = Environment.ProcessorCount;
            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    Bits[i] = Math.Abs(Bits[i]);
                }
            });
        }

        public void ChangeNegativeNumberToZero()
        {
            int length = Width * Height * Dimensions;
            int degreeOfParallelism = Environment.ProcessorCount;
            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    Bits[i] = Bits[i] < 0 ? 0 : Bits[i];
                }
            });
        }

        public double Mean()
        {
            int length = Width * Height * Dimensions;
            int degreeOfParallelism = Environment.ProcessorCount;
            double mean = 0;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                double meanpart = 0;
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    meanpart += Math.Abs(Bits[i]) / length;
                }
                mean += meanpart;
            });
            return mean;
        }

        public void Normalize()
        {
            double minValue = Bits.AsParallel().Min();
            double maxValue = Bits.AsParallel().Max();
            int length = Width * Height * Dimensions;
            int degreeOfParallelism = Environment.ProcessorCount;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    Bits[i] = (Bits[i] - minValue) * 255.0 / (maxValue - minValue);
                }
            });
        }

        public void Thresholding(int threshold)
        {
            int length = Width * Height * Dimensions;
            int degreeOfParallelism = Environment.ProcessorCount;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    Bits[i] = Bits[i] > threshold ? 255d : 0d;
                }
            });
        }

        public static PixelMatrix operator +(PixelMatrix pixelArray1, PixelMatrix pixelArray2)
        {
            var resultArray = new PixelMatrix(pixelArray1.Width, pixelArray1.Height, pixelArray1.Dimensions);
            int length = pixelArray1.Width * pixelArray1.Height * pixelArray1.Dimensions;
            int degreeOfParallelism = Environment.ProcessorCount;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    resultArray.Bits[i] = pixelArray1.Bits[i] + pixelArray2.Bits[i];
                }
            });
            return resultArray;
        }

        internal unsafe void LoadBitmapData(Bitmap processedBitmap)
        {
            unsafe
            {
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadOnly, processedBitmap.PixelFormat);
                int bytesPerPixel = BitmapExtensions.GetBytesPerPixel(processedBitmap.PixelFormat);
                int height = bitmapData.Height;
                int width = bitmapData.Width;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, height, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < width; x++)
                    {
                        for (int dim = 0; dim < Dimensions; dim++)
                        {
                            double pixel = currentLine[x * bytesPerPixel + dim];
                            this[x, y, dim] = pixel;
                        }
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
            }
        }

        public unsafe Bitmap ToBitmap()
        {
            unsafe
            {
                var pixelFormat = Dimensions == 1 ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;
                var processedBitmap = new Bitmap(Width, Height, pixelFormat);

                if (pixelFormat == PixelFormat.Format8bppIndexed)
                {
                    processedBitmap.SetGrayscalePalete();
                }

                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.WriteOnly, processedBitmap.PixelFormat);
                int bytesPerPixel = BitmapExtensions.GetBytesPerPixel(processedBitmap.PixelFormat);
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x++)
                    {
                        for (int dim = 0; dim < Dimensions; dim++)
                        {
                            double pixel = this[x, y, dim];
                            currentLine[x * bytesPerPixel + dim] = (byte)pixel;
                        }
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
                return processedBitmap;
            }
        }
        #endregion
    }
}
