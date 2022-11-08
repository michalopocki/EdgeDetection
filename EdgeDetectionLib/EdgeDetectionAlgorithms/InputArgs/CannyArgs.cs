using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class CannyArgs : BaseArgs, ICannyArgs
    {
        private bool _prefiltration;
        private int _kernelSize;
        private double _sigma;
        private int _tHigh;
        private int _tLow;

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
                if (Prefiltration is true && (value < 0))
                {
                    throw new ArgumentOutOfRangeException("Sigma must be greater than or equal to zero.");
                }
                _sigma = value;
            }
        }

        public int THigh
        {
            get => _tHigh;
            set
            {
                if (value is < 0 or > 255)
                {
                    throw new ArgumentOutOfRangeException("Threshold high must be between 0 and 255");
                }
                _tHigh = value;
            }
        }

        public int TLow
        {
            get => _tLow;
            set
            {
                if (value is < 0 or > 255)
                {
                    throw new ArgumentOutOfRangeException("Threshold low must be between 0 and 255");
                }
                _tLow = value;
            }
        }

        public CannyArgs(Bitmap? imageToProcess, bool prefiltration, int kernelSize, double sigma, int tHigh, int tLow) : base(imageToProcess)
        {
            Prefiltration = prefiltration;
            KernelSize = kernelSize;
            Sigma = sigma;
            THigh = tHigh;
            TLow = tLow;
        }
    }
}
