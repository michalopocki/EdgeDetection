using System.Drawing;


namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    public interface IEdgeDetector
    {
        string Name { get; }
        Bitmap DetectEdges();
    }
}
