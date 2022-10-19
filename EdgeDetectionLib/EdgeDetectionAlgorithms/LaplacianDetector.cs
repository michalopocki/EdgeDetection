using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class LaplacianDetector : GradientDetectorBase
    {
        public override string Name => "Laplacian";
        private readonly double _alpha;

        private readonly double[][] _kernel;
        public LaplacianDetector(){}
        public LaplacianDetector(LaplacianArgs args) : base(args)
        {
            _alpha = args.Alpha;
            _kernel = new double[3][]
            {   
                new double[] { _alpha / (_alpha + 1),       (1 - _alpha) / (1 + _alpha),  _alpha / (_alpha + 1) },
                new double[] { (1 - _alpha) / (1 + _alpha), -4 / (_alpha + 1),            (1 - _alpha) / (1 + _alpha) },
                new double[] { _alpha / (_alpha + 1),       (1 - _alpha) / (1 + _alpha),  _alpha / (_alpha + 1) }
            };
        }
        public override Bitmap DetectEdges()
        {
            PixelArray gradient = Convolution(_kernel);
            gradient.ChangeNegativeNumberToZero();
            gradient.Normalize();

            BeforeThresholdingBitmap = gradient.Bitmap;

            if (_thresholding)
            {
                gradient.Thresholding(_threshold);
            }

            return gradient.Bitmap;
        }
    }
}
