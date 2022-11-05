using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    public interface IEdgeDetectorArgs
    {
        Bitmap? ImageToProcess { get; set; }
    }
}
