using System;
using System.Drawing;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    /// <summary>
    /// Class that specifies Marr-Hildreth detector input arguments.
    /// </summary>
    public class MarrHildrethArgs : BaseArgs, IMarrHildrethArgs
    {
        private int _kernelSize;
        private double _sigma;

        /// <inheritdoc />
        public int KernelSize
        {
            get => _kernelSize;
            set
            { 
                if(value < 2)
                {
                    throw new ArgumentOutOfRangeException("Laplacian of gaussian kernel size must be greater than or equal to two.");
                }
            
                _kernelSize = value;
            }
        }

        /// <inheritdoc />
        public double Sigma
        {
            get => _sigma;
            set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Sigma must be greater than or equal to zero.");
                }
                _sigma = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarrHildrethArgs"/> class.
        /// </summary>
        /// <param name="imageToProcess"></param>
        /// <param name="kernelSize"></param>
        /// <param name="sigma"></param>
        public MarrHildrethArgs(Bitmap? imageToProcess, int kernelSize, double sigma) : base(imageToProcess)
        {
            KernelSize = kernelSize;
            Sigma = sigma;
        }
    }
}
