using EdgeDetectionApp.ViewModel;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using EdgeDetectionApp.Messages;

namespace EdgeDetectionApp.Commands
{
    public class LoadImageCommand : CommandBase
    {
        private readonly ImageViewModel _imageViewModel;
        private readonly IDialogService _dialogService;
        private readonly IMessenger _Messenger;

        public LoadImageCommand(ImageViewModel imageViewModel, IDialogService dialogService, IMessenger messenger)
        {
            _imageViewModel = imageViewModel;
            _dialogService = dialogService;
            _Messenger = messenger;
        }   
        public override void Execute(object? parameter)
        {
            LoadImageFromFile();
        }
        private void LoadImageFromFile()
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "Load Image",
                FilterIndex = 2,
            };

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                settings.Filter = String.Format("{0}{1}{2} ({3})|{3}", settings.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }
            settings.Filter = String.Format("{0}{1}{2} ({3})|{3}", settings.Filter, sep, "All Files", "*.*");

            bool? dialogResult = _dialogService.ShowOpenFileDialog(_imageViewModel, settings);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                using var resultBmp = new Bitmap(settings.FileName);

                _imageViewModel.OriginalImage = (Bitmap)resultBmp.Clone();
                _Messenger.Send(new HistogramDataChangedMessage((Bitmap)resultBmp.Clone()));
            }
        }
    }
}
