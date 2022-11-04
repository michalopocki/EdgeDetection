using System.Drawing;


namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public interface IEdgeDetector
    {
        string Name { get; }
        void SetBitmap(Bitmap newBitamp);
        EdgeDetectionResult DetectEdges();
    }
}
