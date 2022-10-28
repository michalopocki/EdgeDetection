using CommunityToolkit.Mvvm.DependencyInjection;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using EdgeDetectionLib.Histogram;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System;

namespace EdgeDetectionApp.ViewModel
{
    public class ViewModelLocator
    {
        //public MainViewModel MainViewModel
        //{
        //    get { return Ioc.Default.GetRequiredService<MainViewModel>(); }
        //}
        //public ImageViewModel ImageViewModel
        //{
        //    get { return Ioc.Default.GetRequiredService<ImageViewModel>(); }
        //}
        //public VideoViewModel VideoViewModel
        //{
        //    get { return Ioc.Default.GetRequiredService<VideoViewModel>(); }
        //}
        //public ChartViewModel ChartViewModel
        //{
        //    get { return Ioc.Default.GetRequiredService<ChartViewModel>(); }
        //}
        //public OptionsViewModel OptionsViewModel
        //{
        //    get { return Ioc.Default.GetRequiredService<OptionsViewModel>(); }
        //}
        //private readonly ServiceProvider _serviceProvider;
        public MainViewModel MainViewModel => Ioc.Default.GetRequiredService<MainViewModel>();
        public ImageViewModel ImageViewModel => Ioc.Default.GetRequiredService<ImageViewModel>();
        public VideoViewModel VideoViewModel => Ioc.Default.GetRequiredService<VideoViewModel>();
        public ChartViewModel ChartViewModel => Ioc.Default.GetRequiredService<ChartViewModel>();
        public OptionsViewModel OptionsViewModel => Ioc.Default.GetRequiredService<OptionsViewModel>();
        //public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
        //public ImageViewModel ImageViewModel => _serviceProvider.GetRequiredService<ImageViewModel>();
        //public VideoViewModel VideoViewModel => _serviceProvider.GetRequiredService<VideoViewModel>();
        //public ChartViewModel ChartViewModel => _serviceProvider.GetRequiredService<ChartViewModel>();
        //public OptionsViewModel OptionsViewModel => _serviceProvider.GetRequiredService<OptionsViewModel>();

        //public ViewModelLocator()
        //{
        //    var serviceCollection = new ServiceCollection();
        //    ConfigureServices(serviceCollection);
        //    _serviceProvider = serviceCollection.BuildServiceProvider();
        //}

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddSingleton<INavigationService, FrameNavigationService>();

        //    services.AddSingleton<MainViewModel>();
        //    services.AddSingleton<ImageViewModel>();
        //    services.AddSingleton<VideoViewModel>();
        //    services.AddSingleton<ChartViewModel>();
        //    services.AddSingleton<OptionsViewModel>();

        //    services.AddSingleton<IDialogService, DialogService>();
        //    services.AddSingleton<IMessenger, Messenger>();
        //    services.AddSingleton<IEdgeDetectorFactory>(new EdgeDetectorFactory());
        //    services.AddSingleton<IHistogramFactory>(new HistogramFactory());
        //}
    }
}
