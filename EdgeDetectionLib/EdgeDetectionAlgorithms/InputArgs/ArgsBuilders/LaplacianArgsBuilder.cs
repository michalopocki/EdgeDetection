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
    /// Builder class that specifies Laplacian detector arguments.
    /// </summary>
    public class LaplacianArgsBuilder : IArgsBuilder
    {

        private readonly LaplacianArgs _laplacianArgs = new LaplacianArgs(null, 0, false, 0, false, 2, 0);

        /// <inheritdoc />
        public IEdgeDetectorArgs Build() => _laplacianArgs;

        /// <summary>
        /// Initializes the builder.
        /// </summary>
        /// <returns></returns>
        public static LaplacianArgsBuilder Init()
        {
            return new LaplacianArgsBuilder();
        }

        /// <summary>
        /// Sets input image.
        /// </summary>
        /// <param name="imageToProcess"></param>
        /// <returns></returns>
        public LaplacianArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _laplacianArgs.ImageToProcess = imageToProcess;
            return this;
        }

        /// <summary>
        /// Sets gaussian smoothing prefiltration
        /// </summary>
        /// <param name="prefiltration"></param>
        /// <param name="kernelSize"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public LaplacianArgsBuilder SetPrefiltration(bool prefiltration, int kernelSize, double sigma)
        {
            _laplacianArgs.Prefiltration = prefiltration;
            _laplacianArgs.KernelSize = kernelSize;
            _laplacianArgs.Sigma = sigma;
            return this;
        }

        /// <summary>
        /// Sets thresholding.
        /// </summary>
        /// <param name="thresholding"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public LaplacianArgsBuilder SetThresholding(bool thresholding, int threshold)
        {
            _laplacianArgs.Thresholding = thresholding;
            _laplacianArgs.Threshold = threshold;
            return this;
        }

        /// <summary>
        /// Sets alpha value.
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public LaplacianArgsBuilder SetAlpha(double alpha)
        {
            _laplacianArgs.Alpha = alpha;
            return this;
        }
    }
}
