using System.Drawing;


namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public interface IEdgeDetector
    {
        string Name { get; }
        EdgeDetectionResult DetectEdges();
    }
}
