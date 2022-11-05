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
    public class CannyArgsBuilder : IArgsBuilder
    {
        private readonly CannyArgs _cannyArgs = new CannyArgs(null, false, 0, 0, 0, 0);
        public IEdgeDetectorArgs Build() => _cannyArgs;
        private CannyArgsBuilder() { }

        public static CannyArgsBuilder Init()
        {
            return new CannyArgsBuilder();
        }

        public CannyArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _cannyArgs.ImageToProcess = imageToProcess;
            return this;
        }

        public CannyArgsBuilder SetPrefiltration(bool prefiltration, int kernelSize, double sigma)
        {
            _cannyArgs.Prefiltration = prefiltration;
            _cannyArgs.KernelSize = kernelSize;
            _cannyArgs.Sigma = sigma;
            return this;
        }

        public CannyArgsBuilder SetHysteresisThresholds(int TLow, int THigh)
        {
            _cannyArgs.TLow = TLow;
            _cannyArgs.THigh = THigh;
            return this;
        }
    }
}
