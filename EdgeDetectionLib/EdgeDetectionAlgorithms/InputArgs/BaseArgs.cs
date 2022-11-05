using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    public abstract class BaseArgs : IEdgeDetectorArgs
    {
        public Bitmap? ImageToProcess { get; set; }
        public BaseArgs(Bitmap? imageToProcess)
        {
            ImageToProcess = imageToProcess;
        }
        public BaseArgs(){}
    }
}
