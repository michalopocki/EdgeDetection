using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class MarrHildrethDetector : EdgeDetectorBase
    {
        public override string Name => "Marr-Hildreth";
        private readonly double sigma = 1.5;
        public MarrHildrethDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale)
        {
        }
        public MarrHildrethDetector() { }

        public override Bitmap DetectEdges()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            double[][] kernel = LaplacianOfGaussian(sigma);
            PixelArray img = Convolution(kernel);
            PixelArray = ZeroCrossing(img);

            //img.Abs();
            //img.Normalize();
            watch.Stop();
            System.Diagnostics.Trace.WriteLine($"Detecting time: {watch.ElapsedMilliseconds} ms");

            return PixelArray.Bitmap;
        }
        private PixelArray ZeroCrossing(PixelArray pixelArray)
        {
            double avar = 0.5 * pixelArray.Mean();
            PixelArray resultArray = new PixelArray(_width, _height);

            Parallel.For(1, _width, x =>
            {
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < 3; d++)
                    {
                        if (pixelArray[x, y, d] < 0 && pixelArray[x + 1, y, d] > 0
                            && (Math.Abs(pixelArray[x + 1, y, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                            continue;
                        }
                        if (pixelArray[x, y, d] < 0 && pixelArray[x - 1, y, d] > 0
                            && (Math.Abs(pixelArray[x - 1, y, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                            continue;
                        }
                        if (pixelArray[x, y, d] < 0 && pixelArray[x, y - 1, d] > 0
                            && (Math.Abs(pixelArray[x, y - 1, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                            continue;
                        }
                        if (pixelArray[x, y, d] < 0 && pixelArray[x, y + 1, d] > 0
                            && (Math.Abs(pixelArray[x, y + 1, d]) - pixelArray[x, y, d]) > avar)
                        {
                            resultArray[x, y, d] = pixelArray[x, y, d];
                            continue;
                        }
                    }
                }
            });
            return resultArray;
        }
        private double[][] LaplacianOfGaussian(double sigma)
        {
            int size = (int)Math.Ceiling(sigma) * 5;
            size = (size - 1) / 2;
            double[][] meshX = CreateMeshGrid(size, "x");
            double[][] meshY = CreateMeshGrid(size, "y");
            double[][] a = CreateAarray(meshX, meshY, sigma);
            double[][] b = CreateBarray(meshX, meshY, sigma);
            double c = 1;
            double[][] LoG = CreateLoGarray(a, b, c);
            return LoG;
        }
        private double[][] CreateLoGarray(double[][] a, double[][] b, double c)
        {
            int length = a.Length;
            double[][] LoG = InitializeJaggedArray(length);
            double sum = 0;
            double mean2;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    LoG[i][j] = c * a[i][j] * b[i][j];
                    sum += LoG[i][j];
                }
            }
            mean2 = sum / (length * length);

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    LoG[i][j] -= mean2;
                }
            }
            return LoG;
        }
        private double[][] CreateBarray(double[][] meshX, double[][] meshY, double sigma)
        {
            int length = meshX.Length;
            double[][] b = InitializeJaggedArray(length);
            double sigma2 = sigma * sigma;
            double sum = 0;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    b[i][j] = Math.Exp(-(meshX[i][j] * meshX[i][j] + meshY[i][j] * meshY[i][j]) / (2 * sigma2));
                    sum += b[i][j];
                }
            }
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    b[i][j] /= sum;
                }
            }
            return b;
        }
        private double[][] CreateAarray(double[][] meshX, double[][] meshY, double sigma)
        {
            int length = meshX.Length;
            double[][] a = InitializeJaggedArray(length);
            double sigma2 = sigma * sigma;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    a[i][j] = (meshX[i][j] * meshX[i][j] + meshY[i][j] * meshY[i][j] - sigma2) / (sigma2 * sigma2);
                }
            }
            return a;
        }
        private double[][] CreateMeshGrid(int size, string xOry)
        {
            int length = size * 2 + 1;
            double[][] meshGrid = InitializeJaggedArray(length);
            for (int i = 0, y = -size; i < length; i++, y++)
            {
                for (int j = 0, x = -size; j < length; j++, x++)
                {
                    meshGrid[i][j] = xOry == "x" ? x : y;
                }
            }
            return meshGrid;
        }
        private double[][] InitializeJaggedArray(int length)
        {
            double[][] jaggedArray = new double[length][];
            for (int i = 0; i < length; i++)
            {
                jaggedArray[i] = new double[length];
            }
            return jaggedArray;
        }
    }
}
