using EdgeDetectionLib.Histogram;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using MvvmDialogs;
using Color = System.Windows.Media.Color;
using EdgeDetectionApp.Messages;
using System.Collections.ObjectModel;
using EdgeDetectionApp.Models;
using System.Windows.Documents;
using System.Collections.Generic;
using Xceed.Wpf.AvalonDock.Properties;
using System.Windows;
using System.Windows.Controls;
using System;

namespace EdgeDetectionApp.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
        private readonly IHistogramFactory _histogramFactory;
        private readonly IMessenger _messenger;
        private readonly LinearGradientBrush _gradientBrush;
        private SeriesCollection _series;
        private double _threshold1;
        private double _threshold2;
        private bool _threshold2Visibility;
        public SeriesCollection Series { get => _series; set => SetField(ref _series, value); }
        public double Threshold1 { get => _threshold1; set => SetField(ref _threshold1, value); }
        public double Threshold2 { get => _threshold2; set => SetField(ref _threshold2, value); }
        public bool Threshold2Visibility { get => _threshold2Visibility; set => SetField(ref _threshold2Visibility, value); }
        public ChartViewModel(IHistogramFactory histogramFactory, IMessenger messenger)
        {
            _gradientBrush = InitializeLinearGradientBrush();
            _histogramFactory = histogramFactory;
            _messenger = messenger;
            _messenger.Subscribe<HistogramDataChangedMessage>(this, HistogramDataChanged);
            _messenger.Subscribe<ThresholdChangedMessage>(this, UpdateThreshold);
        }
        private void UpdateThreshold(object obj)
        {
            var message = (ThresholdChangedMessage)obj;

            Threshold1 = message.Threshold1;
            Threshold2 = message.Threshold2;
            Threshold2Visibility = message.Threshold2Visibility;
            //if(message.Threshold2 is null)
            //{
            //    Threshold2Visibility = false;
            //    Threshold2 = 0;
            //    Threshold1 = message.Threshold1 is null ? 0 : message.Threshold1;
            //}
            //else
            //{
            //    Threshold2Visibility = true;
            //    Threshold2 = message.Threshold2;
            //    Threshold1 = message.Threshold1;
            //}   
        }
        private void HistogramDataChanged(object obj)
        {
            var message = (HistogramDataChangedMessage)obj;

            IHistogram histogram = _histogramFactory.Create(message.Bitmap, message.IsGrayscale);
            HistogramResults histogramResults = histogram.Calculate();

            if (message.IsGrayscale == false)
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.G_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(0, 179, 0)),
                       Title = "GREEN",
                       PointGeometry = null,
                       Fill = new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                    },
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.B_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(0, 102, 204)),
                       Title = "BLUE",
                       PointGeometry = null,
                       Fill = new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                    },
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.R_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                       Title = "RED",
                       PointGeometry = null,
                       Fill = _gradientBrush, 
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                    }
                };
            }
            else
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.Gray_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(72,72,72)),
                       Title = "GRAY",
                       PointGeometry = null,
                       Fill = _gradientBrush, 
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                    }
                };
            }

        }
        private static LinearGradientBrush InitializeLinearGradientBrush()
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(100, 255, 0, 0), 0.85));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            return gradientBrush;
        }
    }
}
