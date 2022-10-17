using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public interface IEdgeDetectorFactory
    {
        IReadOnlyList<IEdgeDetector> GetAll();
        IEdgeDetector Get(string name, IEdgeDetectorArgs args);
    }
}
