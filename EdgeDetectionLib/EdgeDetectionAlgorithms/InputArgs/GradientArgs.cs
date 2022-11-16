using System;
using System.Drawing;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class GradientArgs : BaseArgs, IGradientArgs
    {
        private bool _thresholding;
        private int _threshold;
        private bool _prefiltration;
        private int _kernelSize;
        private double _sigma;


        public bool Thresholding
        {
            get => _thresholding;
            set => _thresholding = value;
        }

        public int Threshold
        {
            get => _threshold;
            set
            {
                if (value is < 0 or > 255)
                {
                    throw new ArgumentOutOfRangeException("Threshold must be between 0 and 255");
                }
                _threshold = value;
            }
        }

        public bool Prefiltration
        {
            get => _prefiltration;
            set => _prefiltration = value;
        }

        public int KernelSize
        {
            get => _kernelSize;
            set
            {
                if (Prefiltration is true && (value < 2))
                {
                    throw new ArgumentOutOfRangeException("Gaussian kernel size must be greater than or equal to two.");
                }
                _kernelSize = value;
            }
        }

        public double Sigma
        {
            get => _sigma;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Sigma must be greater than or equal to zero.");
                }
                _sigma = value;
            }
        }

        public GradientArgs(Bitmap? imageToProcess, bool thresholding, int threshold,
                            bool prefiltration, int kernelSize, double sigma) : base(imageToProcess)
        {
            Thresholding = thresholding;
            Threshold = threshold;
            Prefiltration = prefiltration;
            KernelSize = kernelSize;
            Sigma = sigma;
        }
    }
}
