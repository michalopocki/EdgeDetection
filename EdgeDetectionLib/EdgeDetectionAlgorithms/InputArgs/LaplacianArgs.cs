using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class LaplacianArgs : GradientArgs, ILaplacianArgs
    {
        private double _alpha;

        public double Alpha
        {
            get => _alpha;
            set
            {
                if (value is < 0 or > 1)
                {
                    throw new ArgumentOutOfRangeException("Alpha must be between 0 and 1");
                }
                _alpha = value;
            }
        }
        public LaplacianArgs(Bitmap? imageToProcess, double alpha, 
            bool thresholing, int threshold, bool prefiltration, int kernelSize, double sigma) 
            : base(imageToProcess, thresholing, threshold, prefiltration, kernelSize, sigma)
        {
            Alpha = alpha;
        }
    }
}
