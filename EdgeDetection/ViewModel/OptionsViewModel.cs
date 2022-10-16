using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Models;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using System;
using System.Collections.ObjectModel;

namespace EdgeDetectionApp.ViewModel
{
    public class OptionsViewModel : ViewModelBase
    {
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private readonly IMessenger _messenger;

        public ObservableCollection<IEdgeDetector> EdgeDetectors { get; init; }
        private IEdgeDetector _selectedEdgeDetector;
        public IEdgeDetector SelectedEdgeDetector
        {
            get => _selectedEdgeDetector;
            set => SetField(ref _selectedEdgeDetector, value);
        }
        private bool _isGrayscale;
        public bool IsGrayscale
        {
            get => _isGrayscale;
            set
            {
                _isGrayscale = value;
                _messenger.Send(new ColorModelChangedMessage(value));
            }

        }
        public OptionsViewModel(IEdgeDetectorFactory edgeDetectorFactory, IMessenger messenger)
        {
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            EdgeDetectors = new ObservableCollection<IEdgeDetector>(_edgeDetectorFactory.GetAll());
            _messenger.Subscribe<SendOptionsRequestMessage>(this, SendOptions);
        }

        private void SendOptions(object obj)
        {
            DetectionParameters parameters = new DetectionParameters();
            parameters.DetectorName = SelectedEdgeDetector.Name;
            _messenger.Send(new SendOptionsMessage(parameters));
        }
    }
}
