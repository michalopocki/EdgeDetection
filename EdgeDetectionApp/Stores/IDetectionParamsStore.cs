using EdgeDetectionApp.Models;
using System;

namespace EdgeDetectionApp.Stores
{
    public interface IDetectionParamsStore
    {
        event Action<DetectionParameters>? ParamsCreated;
        void CreateDetectionParams(DetectionParameters detectionParameters);
    }
}