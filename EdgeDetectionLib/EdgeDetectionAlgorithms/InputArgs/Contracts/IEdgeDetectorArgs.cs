using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    /// <summary>
    /// Interface abstracting the edge detector arguments.
    /// </summary>
    public interface IEdgeDetectorArgs
    {
        /// <summary>
        /// The image which edges will be detected.
        /// </summary>
        Bitmap? ImageToProcess { get; set; }
    }
}
