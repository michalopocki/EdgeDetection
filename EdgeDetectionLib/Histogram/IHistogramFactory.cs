using System.Drawing;


namespace EdgeDetectionLib.Histogram
{
    /// <summary>
    /// Interface abstracting the histogramfactory.
    /// </summary>
    public interface IHistogramFactory
    {
        /// <summary>
        /// Creates histogram based on bitmap pixel format.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns> If bitmap pixel format is Format8bppIndexed returns GrayHistogram, else returns RGBHistogram. </returns>
        IHistogram Create(Bitmap bitmap);
    }
}
