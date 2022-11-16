using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System.Buffers.Text;
using System.Collections.Generic;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.Factory
{
    /// <summary>
    /// Interface abstracting the edge detector factory.
    /// </summary>
    public interface IEdgeDetectorFactory
    {
        /// <summary>
        /// Gets edge detectors names.
        /// </summary>
        /// <returns>
        /// Collection of <see langword="string" /> with edge detectors names.
        /// </returns>
        IReadOnlyList<string> GetAll();

        /// <summary>
        /// Gets edge detector based on its name and given arguments.
        /// </summary>
        /// <param name="name">
        /// Name of edge detector.
        /// </param>
        /// <param name="args">
        /// Input arguments of edge detector.
        /// </param>
        /// <returns>
        /// Instance of class that implements <see cref="IEdgeDetector"/> interface.
        /// </returns>
        IEdgeDetector Get(string name, IEdgeDetectorArgs args);
    }
}
