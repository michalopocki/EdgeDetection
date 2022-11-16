using System;
using System.Drawing;

namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Class that contains results of edge detection.
    /// </summary>
    public class EdgeDetectionResult : IDisposable
    {
        /// <summary>
        /// Final bitmap with detected edges.
        /// </summary>
        public Bitmap ProcessedImage { get; set; }

        /// <summary>
        /// Bitmap before thresholding.
        /// </summary>
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
