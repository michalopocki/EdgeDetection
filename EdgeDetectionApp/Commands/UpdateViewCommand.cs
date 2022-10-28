using CommunityToolkit.Mvvm.DependencyInjection;
using EdgeDetectionApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EdgeDetectionApp.Commands
{
    public class UpdateViewCommand : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ImageViewModel _imageViewModel;
        private readonly VideoViewModel _videoViewModel;

        public UpdateViewCommand(MainViewModel mainViewModel, ImageViewModel imageViewModel, VideoViewModel videoViewModel)
        {
            _mainViewModel = mainViewModel;
            _imageViewModel = imageViewModel;
            _videoViewModel = videoViewModel;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "Image")
            {
                _mainViewModel.SelectedViewModel = _imageViewModel;
            }
            else if (parameter.ToString() == "Video")
            {
                //ChartViewModel.canGet = false;
                _mainViewModel.SelectedViewModel = _videoViewModel;
            }
        }
    }
}
