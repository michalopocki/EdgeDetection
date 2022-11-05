using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using EdgeDetectionLib.Kernels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public abstract class GradientDetectorBase : EdgeDetectorBase
    {
        protected internal bool _thresholding;
        protected internal int _threshold;
        protected internal bool _prefiltration;
        protected internal int _kernelSize;
        protected internal double _sigma;
        internal readonly double[][] _kernel;

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
