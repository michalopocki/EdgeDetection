using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class GradientArgs : BaseArgs
    {
        public bool Thresholding { get; set; }
        public int Threshold { get; set; }
        public bool Prefiltration { get; set; }
        public int KernelSize { get; set; }
        public double Sigma { get; set; }

        public GradientArgs(Bitmap? imageToProcess, bool isGrayscale, bool thresholding, int threshold,
                            bool prefiltration, int kernelSize, double sigma) : base(imageToProcess, isGrayscale)
        {
            Thresholding = thresholding;
            Threshold = threshold;
            Prefiltration = prefiltration;
            KernelSize = kernelSize;
            Sigma = sigma;
        }
    }
}
