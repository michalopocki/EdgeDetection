using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public class CannyArgs : BaseArgs
    {
        public bool Prefiltration { get; set; }
        public int KernelSize { get; set; }
        public double Sigma { get; set; }
        public int THigh { get; set; }
        public int TLow { get; set; }
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
