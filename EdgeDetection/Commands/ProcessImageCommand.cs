using EdgeDetectionApp.EdgeDetectorAlgorithms;
using EdgeDetectionApp.Messages;
using EdgeDetectionApp.ViewModel;
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
            IEdgeDetector edgeDetector = _edgeDetectorFactory.Get(_mainViewModel.SelectedEdgeDetector, _mainViewModel.OriginalImage, false);
 
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Bitmap processedImage = await Task.Run(() => edgeDetector.DetectEdges());

            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Detector:" + watch.ElapsedMilliseconds + " ms");

            _mainViewModel.ProcessedImage = processedImage;
            _messenger.Send(new HistogramDataChangedMessage(processedImage, false));
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
