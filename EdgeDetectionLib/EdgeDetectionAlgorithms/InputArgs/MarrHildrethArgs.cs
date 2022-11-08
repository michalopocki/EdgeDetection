using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class MarrHildrethArgs : BaseArgs, IMarrHildrethArgs
    {
        private int _kernelSize;
        private double _sigma;

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
        public MarrHildrethArgs(Bitmap? imageToProcess, int kernelSize, double sigma) : base(imageToProcess)
        {
            KernelSize = kernelSize;
            Sigma = sigma;
        }
    }
}
