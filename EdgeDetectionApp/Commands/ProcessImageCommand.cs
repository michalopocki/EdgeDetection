using EdgeDetectionApp.Messages;
using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;

namespace EdgeDetectionApp.Commands
{
    public class ProcessImageCommand : AsyncCommandBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private readonly IMessenger _messenger;

        public ProcessImageCommand(MainViewModel imageViewModel, IEdgeDetectorFactory edgeDetectorFactory, IMessenger messenger)
        {
            _mainViewModel = imageViewModel;
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            _mainViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            _messenger.Send(new SendOptionsRequestMessage());
            await Process();
        }

        private async Task Process()
        {
            string detectorName = _mainViewModel.DetectionParameters.DetectorName;
            IEdgeDetectorArgs args = _mainViewModel.DetectionParameters.Args;
            args.ImageToProcess = args.IsGrayscale ? _mainViewModel.GrayscaleImage : _mainViewModel.OriginalImage;
                
            IEdgeDetector edgeDetector = _edgeDetectorFactory.Get(detectorName, args);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Bitmap processedImage = await Task.Run(() => edgeDetector.DetectEdges());

            if (_mainViewModel.DetectionParameters.Negative)
            {
                processedImage.MakeNegative();
            }

            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Detector:" + watch.ElapsedMilliseconds + " ms");

            _mainViewModel.ImageToShow = processedImage;
            _messenger.Send(new HistogramDataChangedMessage(edgeDetector.BeforeThresholdingBitmap, args.IsGrayscale));
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
