using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders
{
    /// <summary>
    /// Interface abstracting edte detectors arguments builder.
    /// </summary>
    public interface IArgsBuilder
    {
        /// <summary>
        /// Builds edge detector arguments.
        /// </summary>
        /// <returns>
        /// Class that implements <see cref="IEdgeDetectorArgs"/> interface.
        /// </returns>
        IEdgeDetectorArgs Build();
    }
}
