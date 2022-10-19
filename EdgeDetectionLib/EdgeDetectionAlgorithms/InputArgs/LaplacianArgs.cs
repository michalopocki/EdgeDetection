using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class LaplacianArgs : GradientArgs
    {
        public double Alpha { get; set; }
        public LaplacianArgs(Bitmap? imageToProcess, bool isGrayscale, double alpha, 
            bool thresholing, int threshold, bool prefiltration, int kernelSize, double sigma) 
            : base(imageToProcess, isGrayscale, thresholing, threshold, prefiltration, kernelSize, sigma)
        {
            Alpha = alpha;
        }
    }
}
