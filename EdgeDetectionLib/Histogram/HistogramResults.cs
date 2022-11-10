using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Histogram
{
    public class HistogramResults
    {
        private const int ColorDepth = 256;
        public List<int>? R_Series { get; set; }
        public List<int>? G_Series { get; set; }
        public List<int>? B_Series { get; set; }
        public List<int>? Gray_Series { get; set; }
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
