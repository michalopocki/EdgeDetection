using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public interface IEdgeDetectorFactory
    {
        IReadOnlyList<IEdgeDetector> GetAll();
        IEdgeDetector Get(IEdgeDetector edgeDetector, Bitmap originalImage, bool isGrayscale);
    }
}
