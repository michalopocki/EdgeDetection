using System.Drawing;


namespace EdgeDetectionLib.EdgeDetectionAlgorithms
{
    /// <summary>
    /// Interface abstracting the edge detector.
    /// </summary>
    public interface IEdgeDetector
    {
        /// <value>
        /// The <c>Name</c> property represents a name of edge detector.
        /// </value>
        /// <remarks>
        /// Gets the <see cref="Name"/> that is a <see langword="string"/>
        /// describing edge detector.
        /// </remarks>
        string Name { get; }

        /// <summary>
        /// Sets a new image to detect its edges.
        /// </summary>
        /// <remarks>
        /// For image processing in grayscale, bitmap should have pixel format Format8bppIndexed. 
        /// </remarks>
        /// <param name="newBitamp">The bitmap whose edges will be detected.</param>
        void SetBitmap(Bitmap newBitamp);

        /// <summary>
        /// Detects edges in an image based on the selected method.
        /// </summary>
        /// <returns>
        /// Class <see cref="EdgeDetectionResult"/> containing two bitmaps that 
        /// represent result image and image before thresholding.
        /// </returns>
        EdgeDetectionResult DetectEdges();
    }
}
