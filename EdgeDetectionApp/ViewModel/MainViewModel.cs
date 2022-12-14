using EdgeDetectionApp.Commands;
using System.Windows.Input;

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
