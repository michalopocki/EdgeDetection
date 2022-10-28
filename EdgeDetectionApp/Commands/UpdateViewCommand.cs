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
        private readonly ViewModelLocator _viewModelLocator;
        public UpdateViewCommand(MainViewModel mainViewModel, ViewModelLocator viewModelLocator)
        {
            _mainViewModel = mainViewModel;
            _viewModelLocator = viewModelLocator;
        }

        public override void Execute(object parameter)
        {
            if (parameter.ToString() == "Image")
            {
                _mainViewModel.SelectedViewModel = _viewModelLocator.ImageViewModel;
            }
            else if (parameter.ToString() == "Video")
            {
                _mainViewModel.SelectedViewModel = _viewModelLocator.VideoViewModel;
            }
        }
    }
}
