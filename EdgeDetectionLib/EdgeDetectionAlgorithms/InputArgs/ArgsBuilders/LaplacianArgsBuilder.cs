using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders
{
    public class LaplacianArgsBuilder : IArgsBuilder
    {

        private LaplacianArgs _laplacianArgs = new LaplacianArgs(null, false, 0, false, 0, false, 0, 0);
        public IEdgeDetectorArgs Build() => _laplacianArgs;
        public static LaplacianArgsBuilder Init()
        {
            return new LaplacianArgsBuilder();
        }
        public LaplacianArgsBuilder SetBitmap(Bitmap imageToProcess)
        {
            _laplacianArgs.ImageToProcess = imageToProcess;
            return this;
        }
        public LaplacianArgsBuilder SetGrayscale(bool isGrayscale)
        {
            _laplacianArgs.IsGrayscale = isGrayscale;
            return this;
        }
        public LaplacianArgsBuilder SetPrefiltration(bool prefiltration, int kernelSize, double sigma)
        {
            _laplacianArgs.Prefiltration = prefiltration;
            _laplacianArgs.KernelSize = kernelSize;
            _laplacianArgs.Sigma = sigma;
            return this;
        }
        public LaplacianArgsBuilder SetThresholding(bool thresholding, int threshold)
        {
            _laplacianArgs.Thresholding = thresholding;
            _laplacianArgs.Threshold = threshold;
            return this;
        }
        public LaplacianArgsBuilder SetAlpha(double alpha)
        {
            _laplacianArgs.Alpha = alpha;
            return this;
        }
    }
}
