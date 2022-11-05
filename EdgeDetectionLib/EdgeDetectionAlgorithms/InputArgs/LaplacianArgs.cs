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
        public double Alpha { get; set; }
        public LaplacianArgs(Bitmap? imageToProcess, double alpha, 
            bool thresholing, int threshold, bool prefiltration, int kernelSize, double sigma) 
            : base(imageToProcess, thresholing, threshold, prefiltration, kernelSize, sigma)
        {
            Alpha = alpha;
        }
    }
}
