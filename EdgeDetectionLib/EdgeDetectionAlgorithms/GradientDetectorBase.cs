﻿using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
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
        protected bool _thresholding;
        protected int _threshold;
        protected bool _prefiltration;
        protected int _kernelSize;
        protected double _sigma;
        private double[][] _kernel;

        public GradientDetectorBase() { }
        public GradientDetectorBase(GradientArgs args) :base(args)
        {
            _thresholding = args.Thresholding;
            _threshold = args.Threshold;
            _prefiltration = args.Prefiltration;
            _kernelSize = args.KernelSize;
            _sigma = args.Sigma;
            IKernel gaussianKernel = new GaussianKernel(_kernelSize, _sigma);
            _kernel = gaussianKernel.Create();
        }
        protected void Prefiltration()
        {
            if (_prefiltration)
            {
                _pixelMatrix = Convolution(_kernel);
                CutSides(_kernelSize);
            }
        }

    }
}
