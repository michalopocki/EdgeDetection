﻿using CommunityToolkit.Mvvm.DependencyInjection;
using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using EdgeDetectionLib.Histogram;
using LiveCharts.Wpf;
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
        //public ViewModelLocator ViewModelLocator { get { return (ViewModelLocator)Current.TryFindResource("ViewModelLocator"); } }
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
                    .AddSingleton<INavigationService, FrameNavigationService>()
                    .AddSingleton<IEdgeDetectorFactory>(new EdgeDetectorFactory())
                    .AddSingleton<IHistogramFactory>(new HistogramFactory())
                    .AddViewModels<ViewModelBase>()
                    .BuildServiceProvider()
            );
        }
    }
}
