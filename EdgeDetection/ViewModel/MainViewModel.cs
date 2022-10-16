using EdgeDetectionApp.Commands;
using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Models;
using EdgeDetectionLib;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using MvvmDialogs;
using System;
using System.Drawing;
using System.Windows.Input;

namespace EdgeDetectionApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        private readonly IMessenger _messenger;
        private readonly IDialogService _dialogService;
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;

        public DetectionParameters DetectionParameters { get; set; }
        private Bitmap _originalImage;
        public Bitmap OriginalImage
        {
            get => _originalImage;
            set
            {
                _originalImage = (Bitmap)value.Clone();
                GrayscaleImage = value.MakeGrayscale();
                ImageToShow = (Bitmap)value.Clone();
            }
        }
        public Bitmap GrayscaleImage { get; set; }

        private Bitmap _imageToShow;
        public Bitmap ImageToShow
        {
            get => _imageToShow;
            set => SetField(ref _imageToShow, value);
        }
        #endregion
        #region Commands
        public ICommand Process { get; set; }
        public ICommand DropImage { get; set; }
        public ICommand Load { get; set; }
        public ICommand SaveAs { get; set; }
        #endregion
        #region Constructor
        public MainViewModel(IEdgeDetectorFactory edgeDetectorFactory, IMessenger messenger, IDialogService dialogService)
        {
            OriginalImage = new Bitmap(@"E:\VS202022Projects\EdgeDetection\EdgeDetection\bin\Debug\net6.0-windows\ptak3.jpg");
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            _dialogService = dialogService;
            SetupCommands();
            _messenger.Subscribe<SendOptionsMessage>(this, UpdateDetectionParamaters);
            _messenger.Subscribe<ColorModelChangedMessage>(this, ChangeColorModel);
            _messenger.Send(new HistogramDataChangedMessage(OriginalImage));
        }
        #endregion
        #region Methods
        private void SetupCommands()
        {
            Process = new ProcessImageCommand(this, _edgeDetectorFactory, _messenger);
            Load = new LoadImageCommand(this, _dialogService, _messenger);
            SaveAs = new SaveAsImageCommand(this, _dialogService);
            DropImage = new DropImageCommand(this, _dialogService);
        }
        private void ChangeColorModel(object obj)
        {
            var message = (ColorModelChangedMessage)obj;
            if (message.IsGrayscale)
            {
                ImageToShow = GrayscaleImage;
            }
            else
            {
                ImageToShow = OriginalImage;
            }
            _messenger.Send(new HistogramDataChangedMessage(ImageToShow, message.IsGrayscale));
        }
        private void UpdateDetectionParamaters(object obj)
        {
            var message = (SendOptionsMessage)obj;
            DetectionParameters = message.Parameters;
        }
        #endregion
    }
}
