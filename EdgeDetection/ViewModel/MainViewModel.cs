using EdgeDetectionApp.Commands;
using EdgeDetectionApp.EdgeDetectorAlgorithms;
using EdgeDetectionApp.Messages;
using Microsoft.Win32;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EdgeDetectionApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        private readonly IMessenger _Messenger;
        private readonly IDialogService _DialogService;
        private readonly IEdgeDetectorFactory _EdgeDetectorFactory;

        private Bitmap _originalImage;
        public Bitmap OriginalImage
        {
            get => _originalImage;
            set
            {
                _originalImage = (Bitmap)value.Clone();
                GrayscaleImage = value.MakeGrayscale();
                ProcessedImage = (Bitmap)value.Clone();
            }
        }
        public Bitmap GrayscaleImage { get; set; }

        private Bitmap _processedImage;
        public Bitmap ProcessedImage
        {
            get => _processedImage;
            set => SetField(ref _processedImage, value);
        }
        public ObservableCollection<IEdgeDetector> EdgeDetectors { get; init; }
        private IEdgeDetector _selectedEdgeDetector;
        public IEdgeDetector SelectedEdgeDetector
        {
            get => _selectedEdgeDetector;
            set => SetField(ref _selectedEdgeDetector, value);
        }

        #endregion
        #region Commands
        public ICommand Process { get; set; }
        public ICommand Load { get; set; }
        public ICommand SaveAs { get; set; }
        #endregion
        #region Constructor
        public MainViewModel(IMessenger messenger, IDialogService dialogService, IEdgeDetectorFactory edgeDetectorFactory)
        {
            OriginalImage = new Bitmap(@"E:\VS202022Projects\EdgeDetection\EdgeDetection\bin\Debug\net6.0-windows\ptak3.jpg");
            _Messenger = messenger;
            _DialogService = dialogService;
            _EdgeDetectorFactory = edgeDetectorFactory;
            EdgeDetectors = new ObservableCollection<IEdgeDetector>(_EdgeDetectorFactory.GetAll());
            SetupCommands();

            _Messenger.Send(new HistogramDataChangedMessage(OriginalImage));
        }
        #endregion
        #region Methods
        private void SetupCommands()
        {
            Process = new ProcessImageCommand(this, _EdgeDetectorFactory, _Messenger);
            Load = new LoadImageCommand(this, _DialogService, _Messenger);
            SaveAs = new SaveAsImageCommand(this, _DialogService);
        }
        #endregion
    }
}
