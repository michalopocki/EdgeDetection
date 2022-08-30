using EdgeDetection.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EdgeDetection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ImageViewModel imageViewModel = new ImageViewModel();
            ChartViewModel chartViewModel = new ChartViewModel();
            MainViewModel mainViewModel = new MainViewModel(imageViewModel, chartViewModel);

            MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };
            MainWindow.Show();


            base.OnStartup(e);
        }
    }
}
