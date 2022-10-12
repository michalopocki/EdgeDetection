using System.Windows.Media;
using EdgeDetectionApp.EdgeDetectorAlgorithms.Histogram;
using EdgeDetectionApp.Messages;
using LiveCharts;
using LiveCharts.Wpf;
using MvvmDialogs;
using Color = System.Windows.Media.Color;

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
        private LinearGradientBrush gradientBrush;
        public ChartViewModel(IHistogramFactory histogramFactory, IMessenger messenger, IDialogService dialogService)
        {
            _HistogramFactory = histogramFactory;
            _Messenger = messenger;
            _Messenger.Subscribe<HistogramDataChangedMessage>(this, HistogramDataChanged);

            gradientBrush = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(100, 255, 0, 0), 0.85));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
        }

        private void HistogramDataChanged(object obj)
        {
            var message = (HistogramDataChangedMessage)obj;

            IHistogram histogram = _HistogramFactory.Create(message.Bitmap, message.IsGrayscale);
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
                       Fill = gradientBrush,  // new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                       //PointGeometrySize = 0,
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
                       Fill = gradientBrush, //new SolidColorBrush(Colors.Transparent),
                       LineSmoothness = 0.4,
                       StrokeThickness = 2.5
                       //PointGeometrySize = 0,
                    }
                };
            }

        }
    }
}
