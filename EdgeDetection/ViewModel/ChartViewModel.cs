using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using BuildYourOwnMessenger.Services;
using EdgeDetectionApp.Commands;
using EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram;
using EdgeDetectionApp.Messages;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Components;
using MvvmDialogs;

namespace EdgeDetectionApp.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
        private readonly IHistogramFactory _HistogramFactory;
        private readonly IMessenger _Messenger;

        public SeriesCollection _series;
        public SeriesCollection Series
        {
            get => _series;
            set => SetField(ref _series, value);
        }
        //private LinearGradientBrush gradientBrush;
        public ChartViewModel(IHistogramFactory histogramFactory, IMessenger messenger, IDialogService dialogService)
        {
            _HistogramFactory = histogramFactory;
            _Messenger = messenger;
            _Messenger.Subscribe<HistogramDataChangedMessage>(this, HistogramDataChanged);

            //gradientBrush = new LinearGradientBrush
            //{
            //    StartPoint = new System.Windows.Point(0, 0),
            //    EndPoint = new System.Windows.Point(0, 1)
            //};
            //gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(100,255, 0, 0), 0.7));
            //gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.95));
        }

        private void HistogramDataChanged(object obj)
        {
            var message = (HistogramDataChangedMessage)obj;

            IHistogram histogram = _HistogramFactory.Create(message.bitmap, message.isGrayscale);
            HistogramResults histogramResults = histogram.Calculate();


            if (message.isGrayscale == false)
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.R_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                       Title = "RED",
                       PointGeometry = null,
                       Fill = new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                       //PointGeometrySize = 0,
                       //Fill = new SolidColorBrush(Color.FromArgb(100,255,0,0))
                    },
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.G_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(0, 179, 0)),
                       Title = "GREEN",
                       PointGeometry = null,
                       Fill = new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                       //Fill = new SolidColorBrush(Colors.Transparent),
                       //Fill = new SolidColorBrush(Color.FromArgb(100,0,179,0))
                   
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
                       //Fill = new SolidColorBrush(Color.FromArgb(100,0, 102, 204))
                    }
                };
            }
            else
            {
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.R_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(72,72,72)),
                       Title = "GRAY",
                       PointGeometry = null,
                       Fill = new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                       //PointGeometrySize = 0,
                       //Fill = new SolidColorBrush(Color.FromArgb(100,255,0,0))
                    }
                };
            }

        }
    }
}
