using System;
using System.Drawing;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    /// <summary>
    /// Class that specifies Laplacian detector input arguments.
    /// </summary>
    public class LaplacianArgs : GradientArgs, ILaplacianArgs
    {
        private double _alpha;

        /// <inheritdoc />
        public double Alpha
        {
            get => _alpha;
            set
            {
                if (value is < 0 or > 1)
                {
                    throw new ArgumentOutOfRangeException("Alpha must be between 0 and 1");
                }
                _alpha = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaplacianArgs"/> class.
        /// </summary>
        /// <param name="imageToProcess"></param>
        /// <param name="alpha"></param>
        /// <param name="thresholing"></param>
        /// <param name="threshold"></param>
        /// <param name="prefiltration"></param>
        /// <param name="kernelSize"></param>
        /// <param name="sigma"></param>
        public LaplacianArgs(Bitmap? imageToProcess, double alpha, 
            bool thresholing, int threshold, bool prefiltration, int kernelSize, double sigma) 
            : base(imageToProcess, thresholing, threshold, prefiltration, kernelSize, sigma)
        {
            Alpha = alpha;
        }
    }
}
