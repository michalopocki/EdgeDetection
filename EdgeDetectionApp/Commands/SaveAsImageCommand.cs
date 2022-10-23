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
        private readonly MainViewModel _mainViewModel;
        private readonly IDialogService _dialogService;

        public SaveAsImageCommand(MainViewModel mainViewModel, IDialogService dialogService)
        {
            _mainViewModel = mainViewModel;
            _dialogService = dialogService;
        }

        public override void Execute(object parameter)
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

            bool? success = _dialogService.ShowSaveFileDialog(_mainViewModel, settings);
            if (success == true)
            {
                var tmp = _mainViewModel.ImageToShow;
                using (var bmp = new Bitmap(tmp))
                {
                    if (File.Exists(settings.FileName))
                    {
                        File.Delete(settings.FileName);
                    }

                    switch (settings.FilterIndex)
                    {
                        case 0:
                            bmp.Save(settings.FileName, ImageFormat.Png);
                            break;
                        case 1:
                            bmp.Save(settings.FileName, ImageFormat.Jpeg);
                            break;
                        case 2:
                            bmp.Save(settings.FileName, ImageFormat.Bmp);
                            break;
                    }
                }
                _dialogService.ShowMessageBox(_mainViewModel,
                                             $"Image saved successfully!\nDirectory: {settings.FileName}",
                                             "Imaged Saved!",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Information);

            }
        }
    }
}
