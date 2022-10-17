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
        public GradientArgs(Bitmap? imageToProcess, bool isGrayscale, bool thresholding, int threshold) : base(imageToProcess, isGrayscale)
        {
            Thresholding = thresholding;
            Threshold = threshold;
        }
    }
}
