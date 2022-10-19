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
        private bool _prefiltrationVisibility;
        private bool _alphaVisibility;
        private bool _loGkernelVisibility;
        private bool _prefiltration = true;
        private bool _hystTreshVisibility;
        private bool _thresholding;
        private int _previewThreshold;
        private int _threshold;
        private int _tLow = 15;
        private int _tHigh = 40;
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
        public DetectionParameters DetectionParameters
        {
            get => _detectionParameters;
            set => _detectionParameters = value;
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
        public bool Prefiltration { get => _prefiltration; set => SetField(ref _prefiltration, value); }
        public int PrefiltrationKernelSize { get; set; } = 3;
        public double PrefiltrationSigma { get; set; } = 1.5;
        public bool Thresholding
        {
            get => _thresholding;
            set
            {
                if (value == false)
                {
                    _previewThreshold = Threshold;
                    Threshold = 0;
                }
                else
                {
                    Threshold = _previewThreshold;
                }
                SetField(ref _thresholding, value);
            }
        }
        public int Threshold
        {
            get => _threshold;
            set
            {
                SetField(ref _threshold, value);
                _messenger.Send(new ThresholdChangedMessage(value, 0));
            }
        }
        public bool Negative { get; set; }
        public double Alpha { get; set; } = 0.5;
        public int LoGKernelSize { get; set; } = 5;
        public double LoGSigma { get; set; } = 1.5;    
        public int TLow
        {
            get => _tLow;
            set
            {
                SetField(ref _tLow, value);
                _messenger.Send(new ThresholdChangedMessage(value, THigh, true));
            }
        }
        public int THigh
        {
            get => _tHigh;
            set
            {
                SetField(ref _tHigh, value);
                _messenger.Send(new ThresholdChangedMessage(TLow, value, true));
            }
        }
        public bool ThresholingVisibility { get => _thresholingVisibility; set => SetField(ref _thresholingVisibility, value); }
        public bool PrefiltrationVisibility { get => _prefiltrationVisibility; set => SetField(ref _prefiltrationVisibility, value); }
        public bool AlphaVisibility { get => _alphaVisibility; set => SetField(ref _alphaVisibility, value); }
        public bool LoGKernelVisibility { get => _loGkernelVisibility; set => SetField(ref _loGkernelVisibility, value); }
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
                args = new CannyArgs(null, IsGrayscale, Prefiltration, PrefiltrationKernelSize, PrefiltrationSigma, THigh, TLow);
                parameters.Args = args;
            }
            else if (detectorType.IsAssignableFrom(typeof(MarrHildrethDetector)))
            {
                args = new MarrHildrethArgs(null, IsGrayscale, LoGKernelSize, LoGSigma);
                parameters.Args = args;
            }
            else if (detectorType.IsAssignableFrom(typeof(LaplacianDetector)))
            {
                args = new LaplacianArgs(null, IsGrayscale, Alpha, Thresholding, Threshold, Prefiltration, PrefiltrationKernelSize, PrefiltrationSigma);
                parameters.Args = args;
            }
            else
            {
                args = new GradientArgs(null, IsGrayscale, Thresholding, Threshold, Prefiltration, PrefiltrationKernelSize, PrefiltrationSigma);
                parameters.Args = args;
            }

            _messenger.Send(new SendOptionsMessage(parameters));
        }
        private void AdjustOptions()
        {
            Type detectorType = SelectedEdgeDetector.GetType();

            if (detectorType.IsAssignableFrom(typeof(CannyDetector)))
            {
                PrefiltrationVisibility = true;
                ThresholingVisibility = false;
                AlphaVisibility = false;
                LoGKernelVisibility = false;
                HystTreshVisibility = true;
                _messenger.Send(new ThresholdChangedMessage(TLow, THigh, true));
            }
            else if (detectorType.IsAssignableFrom(typeof(MarrHildrethDetector)))
            {
                PrefiltrationVisibility = false;
                ThresholingVisibility = false;
                AlphaVisibility = false;
                LoGKernelVisibility = true;
                HystTreshVisibility = false;
                _messenger.Send(new ThresholdChangedMessage(0, 0));
            }
            else if (detectorType.IsAssignableFrom(typeof(LaplacianDetector)))
            {
                PrefiltrationVisibility = true;
                ThresholingVisibility = true;
                AlphaVisibility = true;
                LoGKernelVisibility = false;
                HystTreshVisibility = false;
                _messenger.Send(new ThresholdChangedMessage(Threshold, 0));
            }
            else
            {
                PrefiltrationVisibility = true;
                ThresholingVisibility = true;
                AlphaVisibility = false;
                LoGKernelVisibility = false;
                HystTreshVisibility = false;
                _messenger.Send(new ThresholdChangedMessage(Threshold, 0));
            }

        }
    }
}
