﻿using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Class that enables edge detection in an image utilising Sobel operator.
    /// </summary>
    public class SobelDetector : GradientDetectorBase
    {
        /// <inheritdoc />
        public override string Name => GetName(this);

        internal readonly double[][] _Gx = new double[3][]
        {
            new double[] { -0.25, 0.0, 0.25},
            new double[] { -0.5 , 0.0, 0.5 },
            new double[] { -0.25, 0.0, 0.25 }
        };
        internal readonly double[][] _Gy = new double[3][]
        {
            new double[] {-0.25, -0.5, -0.25},
            new double[] { 0.0,   0.0,  0.0 },
            new double[] { 0.25,  0.5,  0.25 }
        };
        public SobelDetector(IGradientArgs args) : base(args) {}
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
