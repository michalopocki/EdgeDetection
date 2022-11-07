using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class GradientArgs : BaseArgs, IGradientArgs
    {
        public bool Thresholding { get; set; }
        public int Threshold { get; set; }
        public bool Prefiltration { get; set; }
        public int KernelSize { get; set; } = 1;
        public double Sigma { get; set; }

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
