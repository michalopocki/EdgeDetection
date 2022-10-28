using EdgeDetectionApp.Messages;
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

        public ProcessImageCommand(ImageViewModel imageViewModel, IEdgeDetectorFactory edgeDetectorFactory, IMessenger messenger)
        {
            _imageViewModel = imageViewModel;
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            _imageViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            _messenger.Send(new SendOptionsRequestMessage());
            await Process();
        }

        private async Task Process()
        {
            string detectorName = _imageViewModel.DetectionParameters.DetectorName;
            IEdgeDetectorArgs args = _imageViewModel.DetectionParameters.Args;
            args.ImageToProcess = _imageViewModel.IsGrayscale ? _imageViewModel.GrayscaleImage : _imageViewModel.OriginalImage;

            var watch = System.Diagnostics.Stopwatch.StartNew();
            IEdgeDetector edgeDetector = _edgeDetectorFactory.Get(detectorName, args);   
            
            EdgeDetectionResult detectionResult = await Task.Run(() => edgeDetector.DetectEdges());

            if (_imageViewModel.DetectionParameters.Negative)
            {
                detectionResult.ProcessedImage.MakeNegative();
            }

            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Detector:" + watch.ElapsedMilliseconds + " ms");
            _imageViewModel.ComputingTime = (int)watch.ElapsedMilliseconds;

            _imageViewModel.ImageToShow = detectionResult.ProcessedImage;
            _messenger.Send(new HistogramDataChangedMessage(detectionResult.ImageBeforeThresholding));
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
