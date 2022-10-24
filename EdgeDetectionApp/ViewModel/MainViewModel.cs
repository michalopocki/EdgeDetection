using CommunityToolkit.Mvvm.Input;
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
        #region Fields
        private readonly IMessenger _messenger;
        private readonly IDialogService _dialogService;
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private Bitmap _originalImage;
        private Bitmap _imageToShow;
        private int _computingTime = 0;
        #endregion
        #region Properties
        public DetectionParameters DetectionParameters { get; set; }  
        public Bitmap OriginalImage
        {
            get => _originalImage;
            set
            {
                _originalImage = value;
                GrayscaleImage = value.MakeGrayscale();
                ImageToShow = value;
            }
        }
        public Bitmap GrayscaleImage { get; set; }      
        public Bitmap ImageToShow
        {
            get => _imageToShow;
            set => SetField(ref _imageToShow, value);
        }
        public int ComputingTime
        {
            get => _computingTime;
            set => SetField(ref _computingTime, value);
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
            OriginalImage = new Bitmap(@"E:\VS202022Projects\EdgeDetection\EdgeDetectionApp\bin\Debug\net6.0-windows\ptak3.jpg");
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            _dialogService = dialogService;
            SetupCommands();
            SetupMessages();
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
        private void SetupMessages()
        {
            _messenger.Subscribe<SendOptionsMessage>(this, UpdateDetectionParamaters);
            _messenger.Subscribe<ColorModelChangedMessage>(this, ChangeColorModel);
            _messenger.Send(new HistogramDataChangedMessage(OriginalImage));
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
