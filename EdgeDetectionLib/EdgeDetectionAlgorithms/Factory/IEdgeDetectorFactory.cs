using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms.Factory
{
    public interface IEdgeDetectorFactory
    {
        IReadOnlyList<string> GetAll();
        IEdgeDetector Get(string name, IEdgeDetectorArgs args);
    }
}
