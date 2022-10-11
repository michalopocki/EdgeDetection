using EdgeDetectionApp.EdgeDetectorAlgorithms;
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

        public ProcessImageCommand(MainViewModel imageViewModel, IEdgeDetectorFactory edgeDetectorFactory)
        {
            _mainViewModel = imageViewModel;
            _edgeDetectorFactory = edgeDetectorFactory;
            _mainViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await Process();
        }

        private async Task Process()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //RobertsDetector robertsDetector = new RobertsDetector(_mainViewModel.OriginalImage, false);
            IEdgeDetector edgeDetector = _edgeDetectorFactory.Get(_mainViewModel.SelectedEdgeDetector, _mainViewModel.OriginalImage, false);

            Bitmap processedImage = await Task.Run(() => edgeDetector.DetectEdges());

            watch.Stop();
            System.Diagnostics.Trace.WriteLine("Roberts detector:" + watch.ElapsedMilliseconds + " ms");

            _mainViewModel.ProcessedImage = processedImage;
        }
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }
    }
}
