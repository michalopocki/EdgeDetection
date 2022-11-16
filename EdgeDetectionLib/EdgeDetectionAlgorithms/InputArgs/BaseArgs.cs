using System.Drawing;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs
{
    /// <summary>
    /// Base abstract class for each edge detection arguments classes.
    /// </summary>
    public abstract class BaseArgs : IEdgeDetectorArgs
    {
        /// <inheritdoc />
        public Bitmap? ImageToProcess { get; set; }

        /// <summary>
        /// Base constructor initializing input image.
        /// </summary>
        /// <param name="imageToProcess"></param>
        public BaseArgs(Bitmap? imageToProcess)
        {
            ImageToProcess = imageToProcess;
        }
    }
}
