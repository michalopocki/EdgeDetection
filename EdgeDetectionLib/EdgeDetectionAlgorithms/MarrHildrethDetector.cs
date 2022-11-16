using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using EdgeDetectionLib.Kernels;
using System;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class MarrHildrethDetector : EdgeDetectorBase
    {
        public override string Name => GetName(this);
        private readonly int _LoGKernelSize;
        private readonly double _sigma;

        public MarrHildrethDetector(IMarrHildrethArgs args) : base(args)
        {
            _LoGKernelSize = args.KernelSize;
            _sigma = args.Sigma;
        }

        public override EdgeDetectionResult DetectEdges()
        {
            if (_pixelMatrix is null)
            {
                throw new ArgumentNullException("pixelMatrix", "PixelMatrix can not be null");
            }

            IKernel LoGKernel = new LaplacianOfGaussianKernel(_LoGKernelSize, _LoGKernelSize, _sigma);
            double[][] kernel = LoGKernel.Create();

            PixelMatrix img = Convolution(kernel);
            PixelMatrix imgZeroCrossing = ZeroCrossing(img);

            img.Abs();
            img.Normalize();

            var result = new EdgeDetectionResult(imgZeroCrossing.Bitmap, img.Bitmap);

            return result;
        }

        private PixelMatrix ZeroCrossing(PixelMatrix pixelMatrix)
        {
            var resultMatrix = new PixelMatrix(_width, _height, _dimensions);
            double avar = 0.5 * pixelMatrix.Mean();

            Parallel.For(1, _width - 1, x =>
            {
                for (int y = 1; y < _height - 1; y++)
                {
                    for (int d = 0; d < _dimensions; d++)
                    {
                        if (pixelMatrix[x, y, d] < 0 && pixelMatrix[x + 1, y, d] > 0 &&
                           (Math.Abs(pixelMatrix[x + 1, y, d]) - pixelMatrix[x, y, d]) > avar)
                        {
                            resultMatrix[x, y, d] = pixelMatrix[x, y, d];
                        }
                        else if (pixelMatrix[x, y, d] < 0 && pixelMatrix[x - 1, y, d] > 0 &&
                           (Math.Abs(pixelMatrix[x - 1, y, d]) - pixelMatrix[x, y, d]) > avar)
                        {
                            resultMatrix[x, y, d] = pixelMatrix[x, y, d];
                        }
                        else if (pixelMatrix[x, y, d] < 0 && pixelMatrix[x, y - 1, d] > 0 &&
                           (Math.Abs(pixelMatrix[x, y - 1, d]) - pixelMatrix[x, y, d]) > avar)
                        {
                            resultMatrix[x, y, d] = pixelMatrix[x, y, d];
                        }
                        else if (pixelMatrix[x, y, d] < 0 && pixelMatrix[x, y + 1, d] > 0 &&
                           (Math.Abs(pixelMatrix[x, y + 1, d]) - pixelMatrix[x, y, d]) > avar)
                        {
                            resultMatrix[x, y, d] = pixelMatrix[x, y, d];
                        }
                    }
                }
            });
            return resultMatrix;
        }
    }
}
