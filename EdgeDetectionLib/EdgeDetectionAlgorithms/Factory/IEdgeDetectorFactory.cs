using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System.Collections.Generic;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.Factory
{
    public interface IEdgeDetectorFactory
    {
        IReadOnlyList<string> GetAll();
        IEdgeDetector Get(string name, IEdgeDetectorArgs args);
    }
}
