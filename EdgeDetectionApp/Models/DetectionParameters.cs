using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Models
{
    public class DetectionParameters
    {
        public string DetectorName { get; set; }
        public IEdgeDetectorArgs Args { get; set; }
        public bool Negative { get; set; }

        public DetectionParameters(string detectorName, IEdgeDetectorArgs args)
        {
            DetectorName = detectorName;
            Args = args;
        }
        public DetectionParameters(){}
    }
}
