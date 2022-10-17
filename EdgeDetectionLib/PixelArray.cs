using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace EdgeDetectionLib
{
    public class PixelArray
    {
        #region Properties
        public Bitmap Bitmap => ToBitmap();
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
        }
        public PixelArray(Bitmap bitmap) : this(bitmap.Width, bitmap.Height)
        {
            LoadBitmapData(bitmap);
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
            int length = Width * Height * 3;
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
            int length = Width * Height * 3;
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
            int length = Width * Height * 3;
            int degreeOfParallelism = Environment.ProcessorCount;
            double mean = 0;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = length * (workerId + 1) / degreeOfParallelism;
                for (int i = length * workerId / degreeOfParallelism; i < max; i++)
                {
                    mean += Math.Abs(Bits[i]) / length;
                }
            });
            return mean;
        }
        public void Normalize()
        {
            double minValue = Bits.AsParallel().Min();
            double maxValue = Bits.AsParallel().Max();
            int length = Width * Height * 3;
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
            int length = Width * Height * 3;
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
        public static PixelArray operator +(PixelArray pixelArray1, PixelArray pixelArray2)
        {
            var resultArray = new PixelArray(pixelArray1.Width, pixelArray1.Height);
            int length = pixelArray1.Width * pixelArray1.Height * 3;
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
                var processedBitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                BitmapData bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.WriteOnly, processedBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x ++)
                    {
                        double pixelBlue = this[x, y, 2];
                        double pixelGreen = this[x, y, 1];
                        double pixelRed = this[x, y, 0];

                        currentLine[x * bytesPerPixel] = (byte)pixelBlue;
                        currentLine[x * bytesPerPixel + 1] = (byte)pixelGreen;
                        currentLine[x * bytesPerPixel + 2] = (byte)pixelRed;
                    }
                });
                processedBitmap.UnlockBits(bitmapData);
                return processedBitmap;
            }
        }
        #endregion
    }
}
