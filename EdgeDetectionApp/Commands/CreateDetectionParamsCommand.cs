using EdgeDetectionApp.Models;
using EdgeDetectionApp.Stores;
using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Commands
{
    public class CreateDetectionParamsCommand : CommandBase
    {
        private readonly OptionsViewModel _optionsViewModel;
        private readonly DetectionParamsStore _detectionParamsStore;

        public CreateDetectionParamsCommand(OptionsViewModel optionsViewModel, DetectionParamsStore detectionParamsStore)
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
                DetectorName = _optionsViewModel.SelectedEdgeDetector.Name,
                Negative = _optionsViewModel.Negative
            };
            Type detectorType = _optionsViewModel.SelectedEdgeDetector.GetType();
            IEdgeDetectorArgs args;

            if (detectorType.IsAssignableFrom(typeof(CannyDetector)))
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
            else if (detectorType.IsAssignableFrom(typeof(MarrHildrethDetector)))
            {
                args = MarrHildrethArgsBuilder.Init()
                    .SetLoGKernel(_optionsViewModel.LoGKernelSize, _optionsViewModel.LoGSigma)
                    .Build();
                parameters.Args = args;
            }
            else if (detectorType.IsAssignableFrom(typeof(LaplacianDetector)))
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
