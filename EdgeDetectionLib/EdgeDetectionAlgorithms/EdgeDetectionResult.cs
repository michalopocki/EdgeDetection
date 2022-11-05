using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
