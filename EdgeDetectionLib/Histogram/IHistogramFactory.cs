using System.Drawing;


namespace EdgeDetectionLib.Histogram
{
    public interface IHistogramFactory
    {
        IHistogram Create(Bitmap bitmap);
    }
}
