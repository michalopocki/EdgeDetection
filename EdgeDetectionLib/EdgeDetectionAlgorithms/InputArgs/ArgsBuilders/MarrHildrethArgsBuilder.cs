using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders
{
    public class MarrHildrethArgsBuilder : IArgsBuilder
    {
        private readonly MarrHildrethArgs _marrHildrethArgs = new MarrHildrethArgs(null, 0, 0);
        public IEdgeDetectorArgs Build() => _marrHildrethArgs;
        private MarrHildrethArgsBuilder() { }

        public static MarrHildrethArgsBuilder Init()
        {
            return new MarrHildrethArgsBuilder();
        }

        public MarrHildrethArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _marrHildrethArgs.ImageToProcess = imageToProcess;
            return this;
        }

        public MarrHildrethArgsBuilder SetLoGKernel(int kernelSize, double sigma)
        {
            _marrHildrethArgs.KernelSize = kernelSize;
            _marrHildrethArgs.Sigma = sigma;
            return this;
        }
    }
}
