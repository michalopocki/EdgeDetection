using System;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    public class EdgeDetectionResult : IDisposable
    {
        public Bitmap ProcessedImage { get; set; }
        public Bitmap ImageBeforeThresholding { get; set; }
        public EdgeDetectionResult(Bitmap processedImage, Bitmap imageBeforeThresholding)
        {
            ProcessedImage = processedImage;
            ImageBeforeThresholding = imageBeforeThresholding;
        }

        public void Dispose()
        {
            ProcessedImage.Dispose();
            ImageBeforeThresholding.Dispose(); 
        }
    }
}
