using System.Drawing;


namespace EdgeDetection.EdgeDetectorAlgorithms
{
    interface IEdgeDetector
    {
        string Name { get; }
        Bitmap DetectEdges();
    }
}
