using System.Drawing;


namespace EdgeDetectionApp.EdgeDetectorAlgorithms
{
    interface IEdgeDetector
    {
        string Name { get; }
        Bitmap DetectEdges();
    }
}
