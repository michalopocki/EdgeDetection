using EdgeDetectionApp.Models;
using System;

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
