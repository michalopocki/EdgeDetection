using CommunityToolkit.Mvvm.DependencyInjection;
using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using EdgeDetectionLib.Histogram;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System;
using System.Windows;

namespace EdgeDetectionApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ViewModelLocator ViewModelLocator { get { return (ViewModelLocator)Current.TryFindResource("ViewModelLocator"); } }
        public App()
        {
            SetupDependencyInjection();
        }
        private void SetupDependencyInjection()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddSingleton<IMessenger, Messenger>()
                    .AddSingleton<IEdgeDetectorFactory>(new EdgeDetectorFactory())
                    .AddSingleton<IHistogramFactory>(new HistogramFactory())
                    .AddViewModels<ViewModelBase>()
                    .BuildServiceProvider()
            );
        }
    }
}
