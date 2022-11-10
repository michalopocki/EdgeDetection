using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Base abstract class for each edge detector containing basic alghoritms.
    /// </summary>
    public abstract class EdgeDetectorBase : IEdgeDetector
    {
        /// <inheritdoc />
        public abstract string Name { get; }

        /// <summary>Class containing image pixels.</summary>
        protected internal PixelMatrix _pixelMatrix;
        /// <summary>Width of an image.</summary>
        protected internal int _width;
        /// <summary>Height of an image.</summary>
        protected internal int _height;
        /// <summary>If an image is grayscale equal 1, else equal 3.</summary>
        protected internal int _dimensions;

        /// <summary>
        /// Base constructor initializing input image.
        /// </summary>
        /// <param name="args"></param>
        public EdgeDetectorBase(IEdgeDetectorArgs args)
        {
            if (args.ImageToProcess is not null)
            {
                SetBitmap(args.ImageToProcess);
            }
        }

        /// <inheritdoc />
        public abstract EdgeDetectionResult DetectEdges();

        /// <inheritdoc />
        public void SetBitmap(Bitmap newBitamp)
        {
            _pixelMatrix = new PixelMatrix(newBitamp);
            _width = newBitamp.Width;
            _height = newBitamp.Height;
            _dimensions = newBitamp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ? 1 : 3;
        }

        /// <summary>
        /// Gets name of edge detector.
        /// </summary>
        /// <param name="edgeDetector">
        /// Instance of class that implements <see cref="IEdgeDetector"/> interface.
        /// </param>
        /// <returns>
        /// A <see langword="string"/> value of detector name.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <see cref="IEdgeDetector"/> does not contain 'Detector' word. 
        /// </exception>
        public static string GetName(IEdgeDetector edgeDetector)
        {
            Type type = edgeDetector.GetType();

            if (!type.Name.Contains("Detector"))
            {
                throw new ArgumentException("Type name must contain string Detector");
            }

            return type.Name.Replace("Detector", "");
        }

        /// <summary>
        /// Gets name of edge detector.
        /// </summary>
        /// <param name="edgeDetector">
        /// Type of instance that constains "Detector" word.
        /// </param>
        /// <returns>
        /// A <see langword="string"/> value of detector name.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Type does not contain 'Detector' word. 
        /// </exception>
        public static string GetName(Type edgeDetector)
        {
            if (!edgeDetector.Name.Contains("Detector"))
            {
                throw new ArgumentException("Type name must contain string Detector");
            }

            return edgeDetector.Name.Replace("Detector", "");
        }

        /// <summary>
        /// Implementation of mathematical convolution operation that process 
        /// image based on specific kernel.
        /// </summary>
        /// <param name="filter">
        /// A square matrix that represents a kernel (or mask).
        /// </param>
        /// <returns>
        /// Creates <see cref="PixelMatrix"/> instance which imply processed pixels.
        /// </returns>
        protected PixelMatrix Convolution(double[][] filter)
        {
            var resultMatrix = new PixelMatrix(_width, _height, _dimensions);
            int limit = (filter.GetLength(0) - 1) / 2;

            Parallel.For(limit, _height - limit, y =>
            {
                for (int x = limit; x < _width - limit; x++)
                {
                    for (int n = -limit; n <= limit; n++)
                    {
                        for (int m = -limit; m <= limit; m++)
                        {
                            for (int d = 0; d < _dimensions; d++)
                            {
                                resultMatrix[x, y, d] += _pixelMatrix[x - m, y - n, d] * filter[m + limit][n + limit];
                            }
                        }
                    }
                }
            });

            return resultMatrix;
        }

        /// <summary>
        /// Cut sides of an image after convolution
        /// </summary>
        /// <param name="kernelSize">
        /// Size of square kernel.
        /// </param>
        protected void CutSides(int kernelSize)
        {
            int size = (int)Math.Ceiling((double)kernelSize / 2);

            var cutPixelMatrix = new PixelMatrix(_width - 2 * size, _height - 2 * size, _dimensions);

            for (int x = size, i = 0; x < _width - size; x++, i++)
            {
                for (int y = size, j = 0; y < _height - size; y++, j++)
                {
                    for (int d = 0; d < _dimensions; d++)
                    {
                        cutPixelMatrix[i, j, d] = _pixelMatrix[x, y, d];
                    }
                }
            }
            _pixelMatrix = cutPixelMatrix;
            _width -= 2 * size;
            _height -= 2 * size;
        }

        /// <summary>
        /// Calculate magintude of two gradients.
        /// </summary>
        /// <param name="gradientGx"></param>
        /// <param name="gradientGy"></param>
        /// <returns>
        /// Creates <see cref="PixelMatrix"/> instance which imply magnitude pixels.
        /// </returns>
        protected PixelMatrix GradientMagnitude(PixelMatrix gradientGx, PixelMatrix gradientGy)
        {
            //gradientGx.Abs();
            //gradientGy.Abs();
            //PixelMatrix gradientMagnitude = gradientGx + gradientGy;

            var gradientMagnitude = new PixelMatrix(_width, _height, _dimensions);
            int degreeOfParallelism = Environment.ProcessorCount;

            Parallel.For(0, degreeOfParallelism, workerId =>
            {
                var max = _height * (workerId + 1) / degreeOfParallelism;
                for (int y = _height * workerId / degreeOfParallelism; y < max; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        for (int d = 0; d < _dimensions; d++)
                        {
                            gradientMagnitude[x, y, d] = Math.Sqrt(gradientGx[x, y, d] * gradientGx[x, y, d] + gradientGy[x, y, d] * gradientGy[x, y, d]);
                        }
                    }
                }
            });

            return gradientMagnitude;
        }
    }
}
