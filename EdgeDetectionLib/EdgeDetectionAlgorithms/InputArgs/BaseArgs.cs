using System.Drawing;
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
    }
}
