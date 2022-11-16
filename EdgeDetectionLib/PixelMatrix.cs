using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;


namespace EdgeDetectionLib
{
    /// <summary>
    /// Class that converts bitmap to array.
    /// </summary>
    public class PixelMatrix
    {
        #region Fields
        /// <summary>
        /// Array contaning image pixel values.
        /// </summary>
        internal double[] Bits;
        #endregion

        #region Properties
        /// <summary>
        /// Returns <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        public Bitmap Bitmap => ToBitmap();

        /// <summary>
        /// Height of an image.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Width of an image.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Number of dimensions of an image. If colorscale then <see cref="Dimensions"/> equlas 3, if grayscale equals 1.
        /// </summary>
        public int Dimensions { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PixelMatrix"/> class.
        /// </summary>
        /// <param name="width"> Width of an image. </param>
        /// <param name="height"> Height of an image. </param>
        /// <param name="dimensions"> Number of dimensions of an image. </param>
        public PixelMatrix(int width, int height, int dimensions)
        {
            Width = width;
            Height = height;
            Dimensions = dimensions;
            Bits = new double[width * height * dimensions];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PixelMatrix"/> class.
        /// </summary>
        /// <param name="bitmap"> Image to convert. </param>
        /// <exception cref="ArgumentNullException"></exception>
        public PixelMatrix(Bitmap bitmap)
        {
            if (bitmap is null)
            {
                throw new ArgumentNullException($"{nameof(bitmap)} can not be null.");
            }

            Width = bitmap.Width;
            Height = bitmap.Height;
            Dimensions = BitmapExtensions.GetBytesPerPixel(bitmap.PixelFormat);
            Bits = new double[Width * Height * Dimensions];
            LoadBitmapData(bitmap);
        }
        #endregion

        #region Get and Set pixel
        /// <summary>
        /// Gets and sets pixel value.
        /// </summary>
        /// <param name="x"> Horizontal number of a pixel. </param>
        /// <param name="y"> Vertical number of a pixel. </param>
        /// <param name="dimension"> Dimension of a pixel. </param>
        /// <returns></returns>
        public double this[int x, int y, int dimension]
        {
            get => Bits[dimension * Width * Height + y * Width + x];
            set => Bits[dimension * Width * Height + y * Width + x] = value;
        }

        /// <summary>
        /// Sets pixel value.
        /// </summary>
        /// <param name="x"> Horizontal number of a pixel. </param>
        /// <param name="y"> Vertical number of a pixel. </param>
        /// <param name="dimension"> Dimension of a pixel. </param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetPixel(int x, int y, int dimension, double value)
        {
            if (x < 0 || y < 0 ||
                x > Width || y > Height ||
                dimension < 0 || dimension > 2)
            {
                throw new ArgumentOutOfRangeException("The pixel value or dimension is out of range.");
            }
            int index = dimension * Width * Height + y * Width + x;
            Bits[index] = value;
        }

        /// <summary>
        /// Gets pixel value.
        /// </summary>
        /// <param name="x"> Horizontal number of a pixel. </param>
        /// <param name="y"> Vertical number of a pixel. </param>
        /// <param name="dimension"> Dimension of a pixel. </param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public double GetPixel(int x, int y, int dimension)
        {
            if (x < 0 || y < 0 ||
                x > Width || y > Height ||
                dimension < 0 || dimension > 2)
            {
                throw new ArgumentOutOfRangeException("The pixel value or dimension is out of range.");
            }
            int index = dimension * Width * Height + y * Width + x;
            return Bits[index];
        }
        #endregion

        #region Methods

        /// <summary>
        /// Converts each pixel to the absolute value.
        /// </summary>
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

        /// <summary>
        /// Changes value of negative pixels to zero.
        /// </summary>
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

        /// <summary>
        /// Calculates mean of all pixels.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Normalizes pixels in range 0-255.
        /// </summary>
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

        /// <summary>
        /// Applies thresholding to all pixels.
        /// </summary>
        /// <param name="threshold"> Threshold in range 0-255. </param>
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

        /// <summary>
        /// Adds two <see cref="PixelMatrix"/> instances.
        /// </summary>
        /// <param name="pixelArray1"></param>
        /// <param name="pixelArray2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts <see cref="PixelMatrix"/> to <see cref="System.Drawing.Bitmap"/>.
        /// </summary>
        /// <returns></returns>
        public unsafe Bitmap ToBitmap()
        {
            unsafe
            {
                var pixelFormat = Dimensions == 1 ? PixelFormat.Format8bppIndexed : PixelFormat.Format24bppRgb;
                var processedBitmap = new Bitmap(Width, Height, pixelFormat);

                if (pixelFormat == PixelFormat.Format8bppIndexed)
                {
                    processedBitmap.SetGrayscalePalette();
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
