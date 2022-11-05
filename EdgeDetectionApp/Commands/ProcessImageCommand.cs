using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Models;
using EdgeDetectionApp.Stores;
using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Commands
{
    public class ProcessImageCommand : AsyncCommandBase
    {
        private readonly ImageViewModel _imageViewModel;
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private readonly IMessenger _messenger;
        private readonly DetectionParamsStore _detectionParamsStore;
        private IEdgeDetector _edgeDetector;
        private DetectionParameters _detectionParameters = new DetectionParameters();

        public ProcessImageCommand(ImageViewModel imageViewModel, IEdgeDetectorFactory edgeDetectorFactory, 
                                   IMessenger messenger, DetectionParamsStore detectionParamsStore)
        {
            _imageViewModel = imageViewModel;
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            _detectionParamsStore = detectionParamsStore;
            _detectionParamsStore.ParamsCreated += DetectionParamsStore_ParamsCreated;
            _imageViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void DetectionParamsStore_ParamsCreated(DetectionParameters detectionParams)
        {
            _detectionParameters = detectionParams;
            _edgeDetector = _edgeDetectorFactory.Get(_detectionParameters.DetectorName,
                                                     _detectionParameters.Args);
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            _messenger.Send(new SendOptionsRequestMessage(_imageViewModel));
            await Process();
        }

        private async Task Process()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            _edgeDetector.SetBitmap(_imageViewModel.IsGrayscale ? _imageViewModel.GrayscaleImage : _imageViewModel.OriginalImage);

            using (EdgeDetectionResult detectionResult = await Task.Run(() => _edgeDetector.DetectEdges()))
            {
                if (_detectionParameters.Negative)
                {
                    detectionResult.ProcessedImage.MakeNegative();
                }

                watch.Stop();
                _imageViewModel.ComputingTime = (int)watch.ElapsedMilliseconds;
                _imageViewModel.ImageToShow = (Bitmap)detectionResult.ProcessedImage.Clone();
                _messenger.Send(new HistogramDataChangedMessage(detectionResult.ImageBeforeThresholding));
            }
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
