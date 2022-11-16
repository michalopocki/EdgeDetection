using System;


namespace EdgeDetectionLib.Histogram
{
    /// <summary>
    /// Interface abstracting the histogram of an image.
    /// </summary>
    public interface IHistogram
    {
        /// <summary>
        /// Calculates histogram series.
        /// </summary>
        /// <returns>
        /// Instance of class <see cref="HistogramResults"/> containing two
        /// bitmaps that 
        /// </returns>
        HistogramResults Calculate();
    }
}
