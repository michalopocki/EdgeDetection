using EdgeDetectionLib.Histogram;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using Color = System.Windows.Media.Color;
using EdgeDetectionApp.Messages;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System;
using System.Windows;

namespace EdgeDetectionApp.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
        #region Fields
        private readonly IHistogramFactory _histogramFactory;
        private readonly IMessenger _messenger;
        private readonly LinearGradientBrush _gradientBrush;
        private SeriesCollection _series;
        private double _threshold1;
        private double _threshold2;
        private bool _threshold2Visibility;
        #endregion

        #region Properties
        public SeriesCollection Series { get => _series; set => SetField(ref _series, value); }
        public double Threshold1 { get => _threshold1; set => SetField(ref _threshold1, value); }
        public double Threshold2 { get => _threshold2; set => SetField(ref _threshold2, value); }
        public bool Threshold2Visibility { get => _threshold2Visibility; set => SetField(ref _threshold2Visibility, value); }
        #endregion

        #region Constructor
        public ChartViewModel(IHistogramFactory histogramFactory, IMessenger messenger)
        {
            _gradientBrush = InitializeLinearGradientBrush();
            _histogramFactory = histogramFactory;
            _messenger = messenger;
            _messenger.Subscribe<HistogramDataChangedMessage>(this, HistogramDataChanged);
            _messenger.Subscribe<ThresholdChangedMessage>(this, UpdateThreshold);
        }
        #endregion

        #region Private methods
        private void UpdateThreshold(object obj)
        {
            var message = (ThresholdChangedMessage)obj;

            Threshold1 = message.Threshold1;
            Threshold2 = message.Threshold2;
            Threshold2Visibility = message.Threshold2Visibility;  
        }

        private void HistogramDataChanged(object obj)
        {
            var message = (HistogramDataChangedMessage)obj;
            bool isGrayscale = message.Bitmap.PixelFormat == PixelFormat.Format8bppIndexed;

            IHistogram histogram = _histogramFactory.Create(message.Bitmap);
            HistogramResults histogramResults = histogram.Calculate();

            //if (Series is not null)
            //{
            //    Series.Clear();
            //    Series = null;
            //}
            //GC.Collect();

            if (isGrayscale == false)
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
                       StrokeThickness = 2.5,
                      // DataLabelsTemplate = CreateDataLabelTemplate()
                    },
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.B_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(0, 102, 204)),
                       Title = "BLUE",
                       PointGeometry = null,
                       Fill = new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5,
                    },
                    new LineSeries
                    {
                       Values = new ChartValues<int>(histogramResults.R_Series),
                       Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                       Title = "RED",
                       PointGeometry = null,
                       Fill = _gradientBrush,
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5,
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
                       StrokeThickness = 2.5,
                    }
                };
            }

        }

        private DataTemplate CreateDataLabelTemplate()
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
