using CommunityToolkit.Mvvm.DependencyInjection;
using EdgeDetectionApp.Stores;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using EdgeDetectionLib.Histogram;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System;

namespace EdgeDetectionApp.ViewModel
{
    public class ViewModelLocator
    {
        private readonly ServiceProvider _serviceProvider;
        public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
        public ImageViewModel ImageViewModel => _serviceProvider.GetRequiredService<ImageViewModel>();
        public VideoViewModel VideoViewModel => _serviceProvider.GetRequiredService<VideoViewModel>();
        public ChartViewModel ChartViewModel => _serviceProvider.GetRequiredService<ChartViewModel>();
        public OptionsViewModel OptionsViewModel => _serviceProvider.GetRequiredService<OptionsViewModel>();

        public ViewModelLocator()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDetectionParamsStore, DetectionParamsStore>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<ImageViewModel>();
            services.AddSingleton<VideoViewModel>();
            services.AddTransient<ChartViewModel>();
            services.AddTransient<OptionsViewModel>();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IMessenger, Messenger>();
            services.AddSingleton<IEdgeDetectorFactory>(new EdgeDetectorFactory());
            services.AddSingleton<IHistogramFactory>(new HistogramFactory());
        }
    }
}
