using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders
{
    /// <summary>
    /// Builder class that specifies Marr-Hildreth detector arguments.
    /// </summary>
    public class MarrHildrethArgsBuilder : IArgsBuilder
    {
        private readonly MarrHildrethArgs _marrHildrethArgs = new MarrHildrethArgs(null, 2, 0);

        /// <inheritdoc />
        public IEdgeDetectorArgs Build() => _marrHildrethArgs;
        private MarrHildrethArgsBuilder() { }

        /// Initializes the builder.
        /// </summary>
        /// <returns></returns>
        public static MarrHildrethArgsBuilder Init()
        {
            return new MarrHildrethArgsBuilder();
        }

        /// <summary>
        /// Sets input image.
        /// </summary>
        /// <param name="imageToProcess"></param>
        /// <returns></returns>
        public MarrHildrethArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _marrHildrethArgs.ImageToProcess = imageToProcess;
            return this;
        }

        /// <summary>
        /// Sets Laplacian of Gaussian kernel parameters.
        /// </summary>
        /// <param name="kernelSize"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public MarrHildrethArgsBuilder SetLoGKernel(int kernelSize, double sigma)
        {
            _marrHildrethArgs.KernelSize = kernelSize;
            _marrHildrethArgs.Sigma = sigma;
            return this;
        }
    }
}
