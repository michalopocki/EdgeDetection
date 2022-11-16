using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using EdgeDetectionLib.Kernels;
using System;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Base abstract class for gradient edge detectors.
    /// </summary>
    public abstract class GradientDetectorBase : EdgeDetectorBase
    {
        /// <summary>Determines if apply binary thresholding </summary>
        protected internal bool _thresholding;
        /// <summary>Value of threshold between 0 and 155.</summary>
        protected internal int _threshold;
        /// <summary>Determines if apply gaussian smoothing before edge detection.</summary>
        protected internal bool _prefiltration;
        /// <summary>Size of gaussian kernel.</summary>
        protected internal int _kernelSize;
        /// <summary> Gaussian standard deviation of gaussian kernel.</summary>
        protected internal double _sigma;
        /// <summary>Square matrix that represents gaussian kernel.</summary>
        internal readonly double[][] _kernel;

        /// <summary>
        /// Gradient methods base constructor initializing gussian kernel.
        /// </summary>
        /// <param name="args"></param>
        public GradientDetectorBase(IGradientArgs args) :base(args)
        {
            _thresholding = args.Thresholding;
            _threshold = args.Threshold;
            _prefiltration = args.Prefiltration;
            _kernelSize = args.KernelSize;
            _sigma = args.Sigma;
            IKernel gaussianKernel = new GaussianKernel(_kernelSize, _sigma);
            _kernel = gaussianKernel.Create();
        }

        /// <summary>
        /// Blurs an image utilising gaussian smoothing.
        /// </summary>
        protected internal void Prefiltration()
        {
            if (_prefiltration && ValidateKernelSize())
            {
                _pixelMatrix = Convolution(_kernel);
                CutSides(_kernelSize);
            }
        }

        internal bool ValidateKernelSize()
        {
            int size = (int)Math.Ceiling((double)_kernelSize);
            if (_width - 2 * size <= 0 || _height - 2 * size <= 0)
            {
                throw new ArgumentException("Kernel is over-sized");
            }
            return true;
        }

    }
}
