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

        public override void Execute(object? parameter)
        {
            if (parameter is null) return;

            string view = Convert.ToString(parameter)!;

            if (view.Equals("Image"))
            {
                _mainViewModel.SelectedViewModel = _imageViewModel;
            }
            else if (view.Equals("Video"))
            {
                _mainViewModel.SelectedViewModel = _videoViewModel;
            }
        }
    }
}
