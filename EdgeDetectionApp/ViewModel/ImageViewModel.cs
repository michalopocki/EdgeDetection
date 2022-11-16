using EdgeDetectionApp.Commands;
using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Stores;
using EdgeDetectionLib;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using MvvmDialogs;
using System.Drawing;
using System.Windows.Input;

namespace EdgeDetectionApp.ViewModel
{
    public class ImageViewModel : ViewModelBase
    {
        #region Fields
        private readonly IMessenger _messenger;
        private readonly IDialogService _dialogService;
        private readonly IDetectionParamsStore _detectionParamsStore;
        private readonly IEdgeDetectorFactory _edgeDetectorFactory;
        private Bitmap _originalImage;
        private Bitmap _grayscaleImage;
        private Bitmap _imageToShow;
        private int _computingTime = 0;
        #endregion

        #region Properties
        public bool IsGrayscale { get; set; }
        public Bitmap OriginalImage
        {
            get => _originalImage;
            set
            {
                _originalImage = value;
                GrayscaleImage = value.ToGrayscale();
                ImageToShow = value;
            }
        }
        public Bitmap GrayscaleImage { get => _grayscaleImage; set => _grayscaleImage = value; }
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
        public ImageViewModel(IEdgeDetectorFactory edgeDetectorFactory, IMessenger messenger, 
                             IDialogService dialogService, IDetectionParamsStore detectionParamsStore)
        {
            OriginalImage = new Bitmap(@"Resources\SampleImage\bird.jpg");
            _edgeDetectorFactory = edgeDetectorFactory;
            _messenger = messenger;
            _dialogService = dialogService;
            _detectionParamsStore = detectionParamsStore;
            SetupCommands();
            SetupMessages();
        }
        #endregion

        #region Methods
        private void SetupCommands()
        {
            Process = new ProcessImageCommand(this, _edgeDetectorFactory, _messenger, _detectionParamsStore);
            Load = new LoadImageCommand(this, _dialogService, _messenger);
            SaveAs = new SaveAsImageCommand(this, _dialogService);
            DropImage = new DropImageCommand(this, _dialogService);
        }

        private void SetupMessages()
        {
            _messenger.Subscribe<ColorModelChangedMessage>(this, ChangeColorModel);
            _messenger.Send(new HistogramDataChangedMessage(ImageToShow));
        }

        private void ChangeColorModel(object obj)
        {
            var message = (ColorModelChangedMessage)obj;
            if (message.IsGrayscale)
            {
                ImageToShow = GrayscaleImage;
                IsGrayscale = true;
            }
            else
            {
                ImageToShow = OriginalImage;
                IsGrayscale = false;
            }
            _messenger.Send(new HistogramDataChangedMessage(ImageToShow));
        }

        public override void Dispose()
        {
            OriginalImage.Dispose();
            GrayscaleImage.Dispose();
            ImageToShow.Dispose();
            _messenger.Unsubscribe<ColorModelChangedMessage>(this);
            base.Dispose();
        }
        #endregion
    }
}
