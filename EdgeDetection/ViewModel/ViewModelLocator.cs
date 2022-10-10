using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel
        {
            get
            {
                return Ioc.Default.GetRequiredService<MainViewModel>();
            }
        }
        public ChartViewModel ChartViewModel
        {
            get
            {
                return Ioc.Default.GetRequiredService<ChartViewModel>();
            }
        }
    }
}
