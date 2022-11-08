using EdgeDetectionApp.ViewModel;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System.IO;

namespace EdgeDetectionApp.Commands
{
    public class SaveAsImageCommand : CommandBase
    {
        private readonly ImageViewModel _imageViewModel;
        private readonly IDialogService _dialogService;

        public SaveAsImageCommand(ImageViewModel imageViewModel, IDialogService dialogService)
        {
            _imageViewModel = imageViewModel;
            _dialogService = dialogService;
        }

        public override void Execute(object? parameter)
        {
            SaveBitmapAs();
        }
        private void SaveBitmapAs()
        {

            var settings = new SaveFileDialogSettings
            {
                Title = "Save As...",
                Filter = "PNG|*.png|JPEG|*.jpeg|BMP|*.bmp",
                CheckFileExists = false
            };

            bool? success = false;
            try
            {
                success = _dialogService.ShowSaveFileDialog(_imageViewModel, settings);
            }
            catch (Exception)
            {
                MessageBox.Show("View not registered");
            }
            if (success == true)
            {
                using (var bmp = (Bitmap)_imageViewModel.ImageToShow.Clone())
                {
                    if (File.Exists(settings.FileName))
                    {
                        File.Delete(settings.FileName);
                    }

                    switch (settings.FilterIndex)
                    {
                        case 1:
                            bmp.Save(settings.FileName, ImageFormat.Png);
                            break;
                        case 2:
                            bmp.Save(settings.FileName, ImageFormat.Jpeg);
                            break;
                        case 3:
                            bmp.Save(settings.FileName, ImageFormat.Bmp);
                            break;
                    }
                }
                _dialogService.ShowMessageBox(_imageViewModel,
                                             $"Image saved successfully!\nDirectory: {settings.FileName}",
                                             "Imaged Saved!",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Information);

            }
        }
    }
}
