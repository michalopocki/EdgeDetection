using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Models;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.Collections.ObjectModel;

namespace EdgeDetectionApp.ViewModel
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private readonly IMessenger _messenger;
        private IEdgeDetector _selectedEdgeDetector;
        private DetectionParameters _detectionParameters;
        private bool _isGrayscale;
        private bool _thresholingVisibility;
        private bool _alphaVisibility;
        private bool _kernelVisibility;
        private bool _hystTreshVisibility;

        public ObservableCollection<IEdgeDetector> EdgeDetectors { get; init; }
        public IEdgeDetector SelectedEdgeDetector
        {
            get => _selectedEdgeDetector;
            set
            {
                SetField(ref _selectedEdgeDetector, value);
                AdjustOptions();
            }
        }
        public bool IsGrayscale
        {
            get => _isGrayscale;
            set
            {
                _isGrayscale = value;
                _messenger.Send(new ColorModelChangedMessage(value));
            }

        }
        public DetectionParameters DetectionParameters
        {
            get => _detectionParameters;
            set
            {
                _detectionParameters = value;
            }
        }
        public bool Thresholding { get; set; }
        public int Threshold { get; set; }
        public bool Negative { get; set; }
        public double Alpha { get; set; } = 0.5;
        public int KernelSize { get; set; } = 5;
        public double Sigma { get; set; } = 1.5;
        public int TLow { get; set; } = 15;
        public int THigh { get; set; } = 40;
        //Visibility

        public bool ThresholingVisibility { get => _thresholingVisibility; set => SetField(ref _thresholingVisibility, value); }
        public bool AlphaVisibility { get => _alphaVisibility; set => SetField(ref _alphaVisibility, value); }
        public bool KernelVisibility { get => _kernelVisibility; set => SetField(ref _kernelVisibility, value); }
        public bool HystTreshVisibility { get => _hystTreshVisibility; set => SetField(ref _hystTreshVisibility, value); }
        public OptionsViewModel(IEdgeDetectorFactory edgeDetectorFactory, IMessenger messenger)
        {
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            EdgeDetectors = new ObservableCollection<IEdgeDetector>(_edgeDetectorFactory.GetAll());
            _messenger.Subscribe<SendOptionsRequestMessage>(this, SendOptions);
            DetectionParameters = new DetectionParameters();
        }

        private void SendOptions(object obj)
        {
            var parameters = new DetectionParameters()
            {
                DetectorName = SelectedEdgeDetector.Name,
                Negative = Negative
            };
            Type detectorType = SelectedEdgeDetector.GetType();
            IEdgeDetectorArgs args;

            if (detectorType.IsAssignableFrom(typeof(CannyDetector)))
            {
                args = new CannyArgs(null, IsGrayscale, KernelSize, Sigma, THigh, TLow);
                parameters.Args = args;
            }
            else if (detectorType.IsAssignableFrom(typeof(MarrHildrethDetector)))
            {
                args = new MarrHildrethArgs(null, IsGrayscale, KernelSize, Sigma);
                parameters.Args = args;
            }
            else if (detectorType.IsAssignableFrom(typeof(LaplacianDetector)))
            {
                args = new LaplacianArgs(null, IsGrayscale, Alpha, Thresholding, Threshold);
                parameters.Args = args;
            }
            else
            {
                args = new GradientArgs(null, IsGrayscale, Thresholding, Threshold);
                parameters.Args = args;
            }

            _messenger.Send(new SendOptionsMessage(parameters));
        }
        private void AdjustOptions()
        {
            Type detectorType = SelectedEdgeDetector.GetType();

            if (detectorType.IsAssignableFrom(typeof(CannyDetector)))
            {
                ThresholingVisibility = false;
                AlphaVisibility = false;
                KernelVisibility = true;
                HystTreshVisibility = true;
            }
            else if(detectorType.IsAssignableFrom(typeof(MarrHildrethDetector)))
            {
                ThresholingVisibility = false;
                AlphaVisibility = false;
                KernelVisibility = true;
                HystTreshVisibility = false;
            }
            else if (detectorType.IsAssignableFrom(typeof(LaplacianDetector)))
            {
                ThresholingVisibility = true;
                AlphaVisibility = true;
                KernelVisibility = false;
                HystTreshVisibility = false;
            }
            else
            {
                ThresholingVisibility = true;
                AlphaVisibility = false;
                KernelVisibility = false;
                HystTreshVisibility = false;
            }

        }
    }
}
