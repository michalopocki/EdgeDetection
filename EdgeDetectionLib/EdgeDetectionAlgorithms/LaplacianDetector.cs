using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Class that enables edge detection in an image utilising Laplacian detector.
    /// </summary>
    public class LaplacianDetector : GradientDetectorBase
    {
        /// <inheritdoc />
        public override string Name => GetName(this);
        internal readonly double _alpha;
        internal readonly double[][] _mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaplacianDetector"/> class.
        /// </summary>
        /// <param name="args">
        /// Class that contains Laplacian detector arguments (implements <see cref="ILaplacianArgs"></see> interface).
        /// </param>
        public LaplacianDetector(ILaplacianArgs args) : base(args)
        {
            _alpha = args.Alpha;
            _mask = CreateLaplacianKernel();
        }

        /// <summary>
        /// Creates Laplacian kernel
        /// </summary>
        /// <returns>
        /// Two-dimensional array that represents Laplacian mask.
        /// </returns>
        private double[][] CreateLaplacianKernel()
        {
            double[][] kernel = new double[3][]
            {
                new double[] { _alpha / (_alpha + 1),       (1 - _alpha) / (1 + _alpha),  _alpha / (_alpha + 1) },
                new double[] { (1 - _alpha) / (1 + _alpha), -4 / (_alpha + 1),            (1 - _alpha) / (1 + _alpha) },
                new double[] { _alpha / (_alpha + 1),       (1 - _alpha) / (1 + _alpha),  _alpha / (_alpha + 1) }
            };
            return kernel;
        }

        /// <summary>
        /// Detects edges in an image utilising Laplacian detactor.
        /// </summary>
        /// <returns>
        /// Instance of class <see cref="EdgeDetectionResult"/> containing two bitmaps that 
        /// represent result image and image before thresholding.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The bitmap has not been set.
        /// </exception>
        public override EdgeDetectionResult DetectEdges()
        {
            if (_pixelMatrix is null)
            {
                throw new ArgumentNullException("pixelMatrix", "PixelMatrix can not be null");
            }

            Prefiltration();
            PixelMatrix gradient = Convolution(_mask);
            gradient.ChangeNegativeNumberToZero();
            gradient.Normalize();

            var imageBeforeThresholding = gradient.Bitmap;

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            var result = new EdgeDetectionResult(gradient.Bitmap, imageBeforeThresholding);

            return result;
        }
    }
}
