using EdgeDetectionApp.ViewModel;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace EdgeDetectionApp.Commands
{
    public class DropImageCommand : CommandBase
    {
        private readonly ImageViewModel _imageViewModel;
        private readonly IDialogService _dialogService;
        private readonly List<string> _imageExtensions = GetImageExtensions();
        public DropImageCommand(ImageViewModel imgeViewModel, IDialogService dialogService)
        {
            _imageViewModel = imgeViewModel;
            _dialogService = dialogService;
        }
        public override void Execute(object parameter)
        {
            var e = (DragEventArgs)parameter;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string fileExtension = Path.GetExtension(files[0]);
                string filename = files[0];

                if(_imageExtensions.Contains(fileExtension))
                {
                    _imageViewModel.OriginalImage = new Bitmap(filename);
                }
                else
                {
                    _dialogService.ShowMessageBox(_imageViewModel, "Error!", "Invalid file!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
        }
        private static List<string> GetImageExtensions()
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            List<string> imageExtensions = new List<string>();

            foreach(var codec in codecs)
            {
                string[] extensions = codec.FilenameExtension.Split(';');
                foreach (var extension in extensions)
                {
                    imageExtensions.Add(extension.Substring(1).ToLower());
                }
            }
            return imageExtensions;
        }
    }
}
