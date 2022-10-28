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
        //private readonly INavigationService _navigationService;

        //public ICommand ShowImageCommand { get; private set; }
        //public ICommand ShowVideoCommand { get; private set; }
        //public MainViewModel(INavigationService navigationService)
        //{
        //    _navigationService = navigationService;

        //    ShowImageCommand = new RelayCommand(ShowImage);
        //    ShowVideoCommand = new RelayCommand(ShowVideo);
        //}

        //private void ShowImage()
        //{
        //    _navigationService.Navigate("Image");
        //}
        //private void ShowVideo()
        //{
        //    _navigationService.Navigate("Video");
        //}
        private ViewModelBase _selectedViewModel;
        private ViewModelLocator _viewModelLocator = new();

        public ViewModelBase SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public ICommand UpdateViewCommand { get; set; }
        public MainViewModel()
        {
            //_viewModelLocator = new();
            UpdateViewCommand = new UpdateViewCommand(this, _viewModelLocator);
            _selectedViewModel = _viewModelLocator.ImageViewModel;
        }
    }
}
