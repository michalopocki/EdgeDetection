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
    /// Builder class that specifies Gradient detector arguments.
    /// </summary>
    public class GradientArgsBuilder : IArgsBuilder
    {
        protected readonly GradientArgs _gradientArgs = new GradientArgs(null, false, 0, false, 2, 0);

        /// <inheritdoc />
        public virtual IEdgeDetectorArgs Build() => _gradientArgs;

        /// <summary>
        /// Initializes the builder.
        /// </summary>
        /// <returns></returns>
        public static GradientArgsBuilder Init()
        {
            return new GradientArgsBuilder();
        }

        /// <summary>
        /// Sets input image.
        /// </summary>
        /// <param name="imageToProcess"></param>
        /// <returns></returns>
        public GradientArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _gradientArgs.ImageToProcess = imageToProcess;
            return this;
        }

        /// <summary>
        /// Sets gaussian smoothing prefiltration
        /// </summary>
        /// <param name="prefiltration"></param>
        /// <param name="kernelSize"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public GradientArgsBuilder SetPrefiltration(bool prefiltration, int kernelSize, double sigma)
        {
            _gradientArgs.Prefiltration = prefiltration;
            _gradientArgs.KernelSize = kernelSize;
            _gradientArgs.Sigma = sigma;
            return this;
        }

        /// <summary>
        /// Sets thresholding.
        /// </summary>
        /// <param name="thresholding"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public GradientArgsBuilder SetThresholding(bool thresholding, int threshold)
        {
            _gradientArgs.Thresholding = thresholding;
            _gradientArgs.Threshold = threshold;
            return this;
        }
    }
}
