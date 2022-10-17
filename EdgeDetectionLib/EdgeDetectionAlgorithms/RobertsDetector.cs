﻿using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class RobertsDetector : EdgeDetectorBase
    {
        public override string Name => "Roberts";
        private readonly bool _thresholding;
        private readonly int _threshold;
        private readonly double[][] _Gx = new double[3][]
        {
            new double[] { 0.0, 0.0, -1.0},
            new double[] { 0.0, 1.0, 0.0 },
            new double[] { 0.0, 0.0, 0.0 }
        };
        private readonly double[][] _Gy = new double[3][]
        {
            new double[] {-1.0, 0.0, 0.0},
            new double[] { 0.0, 1.0, 0.0 },
            new double[] { 0.0, 0.0, 0.0 }
        };

        public RobertsDetector(){}
        public RobertsDetector(GradientArgs args) : base(args)
        {
            _thresholding = args.Thresholding;
            _threshold = args.Threshold;
        }

        public override Bitmap DetectEdges()
        {
            PixelArray gradientGx = Convolution(_Gx);
            PixelArray gradientGy = Convolution(_Gy);
            PixelArray gradient = GradientMagnitude(gradientGx, gradientGy);
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
