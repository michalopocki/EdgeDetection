using EdgeDetectionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Stores
{
    public class DetectionParamsStore : IDetectionParamsStore
    {
        public event Action<DetectionParameters>? ParamsCreated;

        public void CreateDetectionParams(DetectionParameters detectionParameters)
        {
            ParamsCreated?.Invoke(detectionParameters);
        }
    }
}
