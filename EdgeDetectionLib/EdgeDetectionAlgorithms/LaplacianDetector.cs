using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class LaplacianDetector : GradientDetectorBase
    {
        public override string Name => GetName(this);
        private readonly double _alpha;
        private readonly double[][] _kernel;
        public LaplacianDetector(ILaplacianArgs args) : base(args)
        {
            _alpha = args.Alpha;
            _kernel = CreateLaplacianKernel();
        }

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

        public override EdgeDetectionResult DetectEdges()
        {
            Prefiltration();
            PixelMatrix gradient = Convolution(_kernel);
            gradient.ChangeNegativeNumberToZero();
            gradient.Normalize();

            var imageBeforeThresholding = (Bitmap)gradient.Bitmap.Clone();

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            var result = new EdgeDetectionResult(gradient.Bitmap, imageBeforeThresholding);

            return result;
        }
    }
}
