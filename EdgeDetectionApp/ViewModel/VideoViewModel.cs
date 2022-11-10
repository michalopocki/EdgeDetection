using AForge.Video;
using AForge.Video.DirectShow;
using CommunityToolkit.Mvvm.Input;
using EdgeDetectionApp.Commands;
using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Stores;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EdgeDetectionApp.ViewModel
{
    public class VideoViewModel : ViewModelBase
    {
        #region Fields
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private readonly IDialogService _dialogService;
        private readonly IMessenger _messenger;
        private readonly IDetectionParamsStore _detectionParamsStore;
        private IVideoSource _videoSource;
        private FilterInfo _currentDevice;
        private BitmapImage _bitmapImage;
        #endregion

        #region Properties
        public ObservableCollection<FilterInfo> VideoDevices { get; set; }
        public FilterInfo CurrentDevice
        {
            get => _currentDevice;
            set => SetField(ref _currentDevice, value);
        }
        public BitmapImage CurrentImage
        {
            get => _bitmapImage;
            set => SetField(ref _bitmapImage, value);
        }
        #endregion

        #region Commands
        public ICommand CameraStartStop { get; set; }
        public ICommand SaveSnapshot { get; set; }
        public ICommand ChangeOptions { get; set; }
        #endregion

        #region Constructor
        public VideoViewModel(IEdgeDetectorFactory edgeDetectorFactory,
                              IDialogService dialogService, 
                              IMessenger messenger,
                              IDetectionParamsStore detectionParamsStore)
        {
            _edgeDetectorFactory = edgeDetectorFactory;
            _dialogService = dialogService;
            _messenger = messenger;
            _detectionParamsStore = detectionParamsStore;
            GetVideoDevices();
            CameraStartStop = new RunCameraCommand(this, _videoSource, _messenger, _dialogService,
                                                   _edgeDetectorFactory, _detectionParamsStore);
            SaveSnapshot = new SaveSnapshotCommand(this);
            ChangeOptions = new RelayCommand(() => _messenger.Send(new SendOptionsRequestMessage(this)));
        }
        #endregion

        #region Methods
        private void GetVideoDevices()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in devices)
            {
                VideoDevices.Add(device);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                _dialogService.ShowMessageBox(this, "No video sources found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void Dispose()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
        }
        #endregion
    }
}
