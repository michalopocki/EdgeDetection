using EdgeDetectionApp.Models;
using EdgeDetectionApp.Stores;
using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionApp.Commands
{
    public class CreateDetectionParamsCommand : CommandBase
    {
        private readonly OptionsViewModel _optionsViewModel;
        private readonly IDetectionParamsStore _detectionParamsStore;

        public CreateDetectionParamsCommand(OptionsViewModel optionsViewModel, IDetectionParamsStore detectionParamsStore)
        {
            _optionsViewModel = optionsViewModel;
            _detectionParamsStore = detectionParamsStore;
        }

        public override void Execute(object? parameter)
        {
            var detectionParams= CreateDetectionParameters();
            _detectionParamsStore.CreateDetectionParams(detectionParams);
        }

        private DetectionParameters CreateDetectionParameters()
        {
            var parameters = new DetectionParameters()
            {
                DetectorName = _optionsViewModel.SelectedEdgeDetector,
                Negative = _optionsViewModel.Negative
            };
            string detectorType = _optionsViewModel.SelectedEdgeDetector;
            IEdgeDetectorArgs args;

            if (detectorType.Equals(EdgeDetectorBase.GetName(typeof(CannyDetector))))
            {
                args = CannyArgsBuilder.Init()
                    .SetPrefiltration(_optionsViewModel.Prefiltration,
                                      _optionsViewModel.PrefiltrationKernelSize,
                                      _optionsViewModel.PrefiltrationSigma)
                    .SetHysteresisThresholds(_optionsViewModel.TLow,
                                             _optionsViewModel.THigh)
                    .Build();
                parameters.Args = args;
            }
            else if (detectorType.Equals(EdgeDetectorBase.GetName(typeof(MarrHildrethDetector))))
            {
                args = MarrHildrethArgsBuilder.Init()
                    .SetLoGKernel(_optionsViewModel.LoGKernelSize, _optionsViewModel.LoGSigma)
                    .Build();
                parameters.Args = args;
            }
            else if (detectorType.Equals(EdgeDetectorBase.GetName(typeof(LaplacianDetector))))
            {
                args = LaplacianArgsBuilder.Init()
                    .SetPrefiltration(_optionsViewModel.Prefiltration,
                                      _optionsViewModel.PrefiltrationKernelSize,
                                      _optionsViewModel.PrefiltrationSigma)
                    .SetThresholding(_optionsViewModel.Thresholding, _optionsViewModel.Threshold)
                    .SetAlpha(_optionsViewModel.Alpha)
                    .Build();
                parameters.Args = args;
            }
            else
            {
                args = GradientArgsBuilder.Init()
                    .SetPrefiltration(_optionsViewModel.Prefiltration,
                                      _optionsViewModel.PrefiltrationKernelSize,
                                      _optionsViewModel.PrefiltrationSigma)
                    .SetThresholding(_optionsViewModel.Thresholding, _optionsViewModel.Threshold)
                    .Build();
                parameters.Args = args;
            }
            return parameters;
        }
    }
}
