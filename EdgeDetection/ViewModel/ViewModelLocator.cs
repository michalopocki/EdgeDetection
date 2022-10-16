using CommunityToolkit.Mvvm.DependencyInjection;
using System;

namespace EdgeDetectionApp.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel
        {
            get { return Ioc.Default.GetRequiredService<MainViewModel>(); }
        }
        public ChartViewModel ChartViewModel
        {
            get { return Ioc.Default.GetRequiredService<ChartViewModel>(); }
        }
        public OptionsViewModel OptionsViewModel
        {
            get { return Ioc.Default.GetRequiredService<OptionsViewModel>(); }
        }
    }
}
