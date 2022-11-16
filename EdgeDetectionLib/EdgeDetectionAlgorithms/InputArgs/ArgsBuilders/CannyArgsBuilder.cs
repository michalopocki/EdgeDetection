using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders
{
    /// <summary>
    /// Builder class that specifies Canny detector arguments.
    /// </summary>
    public class CannyArgsBuilder : IArgsBuilder
    {
        private readonly CannyArgs _cannyArgs = new CannyArgs(null, false, 2, 0, 0, 0);

        /// <inheritdoc />
        public IEdgeDetectorArgs Build() => _cannyArgs;
        private CannyArgsBuilder() { }

        /// <summary>
        /// Initializes the builder.
        /// </summary>
        /// <returns></returns>
        public static CannyArgsBuilder Init()
        {
            return new CannyArgsBuilder();
        }

        /// <summary>
        /// Sets input image.
        /// </summary>
        /// <param name="imageToProcess"></param>
        /// <returns></returns>
        public CannyArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _cannyArgs.ImageToProcess = imageToProcess;
            return this;
        }

        /// <summary>
        /// Sets gaussian smoothing prefiltration
        /// </summary>
        /// <param name="prefiltration"></param>
        /// <param name="kernelSize"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public CannyArgsBuilder SetPrefiltration(bool prefiltration, int kernelSize, double sigma)
        {
            _cannyArgs.Prefiltration = prefiltration;
            _cannyArgs.KernelSize = kernelSize;
            _cannyArgs.Sigma = sigma;
            return this;
        }

        /// <summary>
        /// Sets high and low threshold.
        /// </summary>
        /// <param name="TLow"></param>
        /// <param name="THigh"></param>
        /// <returns></returns>
        public CannyArgsBuilder SetHysteresisThresholds(int TLow, int THigh)
        {
            _cannyArgs.TLow = TLow;
            _cannyArgs.THigh = THigh;
            return this;
        }
    }
}
