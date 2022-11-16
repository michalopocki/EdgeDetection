using AForge.Video.DirectShow;
using AForge.Video;
using EdgeDetectionApp.ViewModel;
using MvvmDialogs;
using System;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows;
using EdgeDetectionLib;
using EdgeDetectionApp.Stores;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using EdgeDetectionApp.Messages;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;

namespace EdgeDetectionApp.Commands
{
    public class RunCameraCommand : CommandBase, IDisposable
    {
        private readonly VideoViewModel _videoViewModel;
        private IVideoSource _videoSource;
        private readonly IMessenger _messenger;
        private readonly IDialogService _dialogService;
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private readonly IDetectionParamsStore _detectionParamsStore;
        private IEdgeDetector _edgeDetector;
        private bool _videoStart = true;

        public RunCameraCommand(VideoViewModel videoViewModel, 
                                IVideoSource videoSource,
                                IMessenger messenger,
                                IDialogService dialogService,
                                IEdgeDetectorFactory edgeDetectorFactory,
                                IDetectionParamsStore detectionParamsStore)
        {
            _videoViewModel = videoViewModel;
            _videoSource = videoSource;
            _messenger = messenger;
            _dialogService = dialogService;
            _edgeDetectorFactory = edgeDetectorFactory;
            _detectionParamsStore = detectionParamsStore;

            _detectionParamsStore.ParamsCreated += _detectionParamsStore_ParamsCreated;
        }

        private void _detectionParamsStore_ParamsCreated(Models.DetectionParameters detectionParams)
        {
            string detectorName = detectionParams.DetectorName;
            IEdgeDetectorArgs args = detectionParams.Args;
            _edgeDetector = _edgeDetectorFactory.Get(detectorName, args);
        }

        public override void Execute(object? parameter)
        {
            _messenger.Send(new SendOptionsRequestMessage(_videoViewModel));
            if (_videoStart)
            {
                StartCamera();
            }
            else
            {
                StopCamera();
            }
        }

        private void video_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            { 
                BitmapImage bi;

                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {

                    using (var grayBitmap = bitmap.ToGrayscale())
                    {
                        _edgeDetector.SetBitmap(grayBitmap);
                        var result = _edgeDetector.DetectEdges();
                        bi = result.ProcessedImage.ToBitmapImage();
                    }
                }
                bi.Freeze();
                Dispatcher.CurrentDispatcher.Invoke(() => _videoViewModel.CurrentImage = bi);
            }
            catch (Exception exc)
            {
                _dialogService.ShowMessageBox(_videoViewModel, "Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void StartCamera()
        {
            if (_videoViewModel.CurrentDevice is not null)
            {
                _videoSource = new VideoCaptureDevice(_videoViewModel.CurrentDevice.MonikerString);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();
                _videoStart = false;
            }
            else
            {
                _dialogService.ShowMessageBox(_videoViewModel, "Current device can't be null");
            }
        }

        private void StopCamera()
        {
            if (_videoSource is not null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
                _videoStart = true;
            }
        }

        public void Dispose()
        {
            _detectionParamsStore.ParamsCreated -= _detectionParamsStore_ParamsCreated;
        }
    }
}
