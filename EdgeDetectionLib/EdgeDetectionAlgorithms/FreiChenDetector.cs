using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using static System.Math;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Class that enables edge detection in an image utilising Frei-Chen operator.
    /// </summary>
    public class FreiChenDetector : GradientDetectorBase
    {
        /// <inheritdoc />
        public override string Name => GetName(this);

        /// <summary> Frei-Chen operator horizontal kernel. </summary>
        internal readonly double[][] _Gx = new double[3][]
        {
            new double[] { -1 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)), -1 / (2 + Sqrt(2)) },
            new double[] {  0 / (2 + Sqrt(2)),        0 / (2 + Sqrt(2)),  0 / (2 + Sqrt(2)) },
            new double[] {  1 / (2 + Sqrt(2)),  Sqrt(2) / (2 + Sqrt(2)),  1 / (2 + Sqrt(2)) }
        };

        /// <summary> Frei-Chen operator vertical kernel. </summary>
        internal readonly double[][] _Gy = new double[3][]
        {
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) },
            new double[] { Sqrt(2) / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)), -Sqrt(2) / (2 + Sqrt(2)) },
            new double[] {       1 / (2 + Sqrt(2)), 0 / (2 + Sqrt(2)),       -1 / (2 + Sqrt(2)) }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="FreiChenDetector"/> class.
        /// </summary>
        /// <param name="args">
        /// Class that contains gradient detector arguments (implements <see cref="IGradientArgs"></see> interface).
        /// </param>
        public FreiChenDetector(IGradientArgs args) : base(args) {}

        /// <summary>
        /// Detects edges in an image utilising Frei-Chen operator.
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
            PixelMatrix gradientGx = Convolution(_Gx);
            PixelMatrix gradientGy = Convolution(_Gy);
            PixelMatrix gradient = GradientMagnitude(gradientGx, gradientGy);
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
