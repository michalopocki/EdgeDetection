using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders
{
    public class GradientArgsBuilder : IArgsBuilder
    {
        protected readonly GradientArgs _gradientArgs = new GradientArgs(null, false, 0, false, 2, 0);

        public virtual IEdgeDetectorArgs Build() => _gradientArgs;

        public static GradientArgsBuilder Init()
        {
            return new GradientArgsBuilder();
        }

        public GradientArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _gradientArgs.ImageToProcess = imageToProcess;
            return this;
        }

        public GradientArgsBuilder SetPrefiltration(bool prefiltration, int kernelSize, double sigma)
        {
            _gradientArgs.Prefiltration = prefiltration;
            _gradientArgs.KernelSize = kernelSize;
            _gradientArgs.Sigma = sigma;
            return this;
        }

        public GradientArgsBuilder SetThresholding(bool thresholding, int threshold)
        {
            _gradientArgs.Thresholding = thresholding;
            _gradientArgs.Threshold = threshold;
            return this;
        }
    }
}
