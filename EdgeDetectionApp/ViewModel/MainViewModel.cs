using CommunityToolkit.Mvvm.Input;
using EdgeDetectionApp.Commands;
using EdgeDetectionApp.Messages;
using EdgeDetectionApp.Models;
using EdgeDetectionLib;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using MvvmDialogs;
using System;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Navigation;

namespace EdgeDetectionApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _selectedViewModel;

        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetField(ref _selectedViewModel, value);
        }

        public ICommand UpdateViewCommand { get; set; }
        public MainViewModel(ImageViewModel imageViewModel, VideoViewModel videoViewModel)
        {
            UpdateViewCommand = new UpdateViewCommand(this, imageViewModel, videoViewModel);
            _selectedViewModel = imageViewModel;
        }
    }
}
