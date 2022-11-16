using System.Collections.Generic;


namespace EdgeDetectionLib.Histogram
{
    /// <summary>
    /// Class containing histogram series.
    /// </summary>
    public class HistogramResults
    {
        private const int ColorDepth = 256;

        /// <summary>
        /// Red color histogram series.
        /// </summary>
        public List<int>? R_Series { get; set; }

        /// <summary>
        /// Green color histogram series.
        /// </summary>
        public List<int>? G_Series { get; set; }

        /// <summary>
        /// Blue color histogram series.
        /// </summary>
        public List<int>? B_Series { get; set; }

        /// <summary>
        /// Gray histogram series.
        /// </summary>
        public List<int>? Gray_Series { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramResults"/> class.
        /// </summary>
        /// <param name="histogramType"> Specifies RGB of gray histogram series. </param>
        public HistogramResults(HistogramType histogramType)
        {
            switch(histogramType)
            {
                case HistogramType.Colorscale:
                    {
                        R_Series = new List<int>(new int[ColorDepth]);
                        G_Series= new List<int>(new int[ColorDepth]);
                        B_Series= new List<int>(new int[ColorDepth]);
                        break;
                    }
                case HistogramType.Grayscale:
                    {
                        Gray_Series = new List<int>(new int[ColorDepth]);
                        break;
                    }
            }
        }
    }
}
