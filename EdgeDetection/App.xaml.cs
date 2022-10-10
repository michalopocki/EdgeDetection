using BuildYourOwnMessenger.Services;
using CommunityToolkit.Mvvm.DependencyInjection;
using EdgeDetectionApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                    .AddSingleton<IMessenger, Messenger>()
                    .AddViewModels<ViewModelBase>()
                    .BuildServiceProvider()
            );
        }
    }
}
