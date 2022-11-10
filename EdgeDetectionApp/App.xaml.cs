using CommunityToolkit.Mvvm.DependencyInjection;
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
        public App()
        {
        }

    }
}
