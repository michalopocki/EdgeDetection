using BuildYourOwnMessenger.Services;
using EdgeDetectionApp.EdgeDetectorAlgorithms;
using EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram;
using EdgeDetectionApp.ViewModel;
using MVVMEssentials.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
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
            await Process();
        }

        private async Task Process()
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //RobertsDetector robertsDetector = new RobertsDetector(_mainViewModel.OriginalImage, false);
            IEdgeDetector edgeDetector = _edgeDetectorFactory.Get(_mainViewModel.SelectedEdgeDetector, _mainViewModel.OriginalImage, false);

            Bitmap processedImage = await Task.Run(() => edgeDetector.DetectEdges());

            var watch = System.Diagnostics.Stopwatch.StartNew();
            IHistogram histogram = new Histogram(processedImage);
            HistogramResults results = histogram.Calculate();
            
            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Roberts detector:" + watch.ElapsedMilliseconds + " ms");

            _mainViewModel.ProcessedImage = processedImage;
            _messenger.Send(results);
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
