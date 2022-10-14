using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public class LaplacianDetector : EdgeDetectorBase
    {
        public override string Name => "Laplacian";
        private readonly double _alpha = 0.5;
        private readonly double[][] _kernel;
        public LaplacianDetector(){}
        public LaplacianDetector(Bitmap bitmap, bool isGrayscale = false) : base(bitmap, isGrayscale)
        {
            _kernel = new double[3][]
            {   
                new double[] { _alpha / (_alpha + 1),       (1 - _alpha) / (1 + _alpha),  _alpha / (_alpha + 1) },
                new double[] { (1 - _alpha) / (1 + _alpha), -4 / (_alpha + 1),            (1 - _alpha) / (1 + _alpha) },
                new double[] { _alpha / (_alpha + 1),       (1 - _alpha) / (1 + _alpha),  _alpha / (_alpha + 1) }
            };
        }
        public override Bitmap DetectEdges()
        {
            PixelArray img = Convolution(_kernel);
            img = PixelArray.Abs(img);
            img.Normalize();

            return img.Bitmap;
        }
    }
}
