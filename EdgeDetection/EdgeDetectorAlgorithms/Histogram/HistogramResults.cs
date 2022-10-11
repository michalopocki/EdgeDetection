using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram
{
    public class HistogramResults
    {
        private const int ColorDepth = 256;
        public List<int> R_Series { get; set; } = new List<int>(new int[ColorDepth]);
        public List<int> G_Series { get; set; } = new List<int>(new int[ColorDepth]);
        public List<int> B_Series { get; set; } = new List<int>(new int[ColorDepth]);
        public List<int> Gray_Series { get; set; } = new List<int>(new int[ColorDepth]);
    }
}
