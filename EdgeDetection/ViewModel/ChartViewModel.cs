using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BuildYourOwnMessenger.Services;
using EdgeDetectionApp.Messages;
using LiveCharts;
using LiveCharts.Wpf;

namespace EdgeDetectionApp.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
        private readonly IMessenger _Messenger;

        public ChartViewModel(IMessenger messenger)
        {
            _Messenger = messenger;
            _Messenger.Subscribe<HistogramDataChangedMessage>(this, HistogramDataChanged);
        }
        private void HistogramDataChanged(object obj)
        {
            var message = (HistogramDataChangedMessage)obj;

            Series = new SeriesCollection
            {
                new LineSeries
                {
                   Values = (IChartValues)message.HistogramResults.R_Series,
                   Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                },
                new LineSeries
                {
                   Values = (IChartValues)message.HistogramResults.G_Series,
                   Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
                },
                new LineSeries
                {
                   Values = (IChartValues)message.HistogramResults.B_Series,
                   Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
                }
            };
        }

        public SeriesCollection Series { get; set; }
            = new SeriesCollection
            {
                new LineSeries
                {
                        Values = new ChartValues<double> { 3, 5, 7, 4 },
                        Fill = new SolidColorBrush(Color.FromRgb(255, 51, 153)),
                },
                new LineSeries
                {
                        Values = new ChartValues<decimal> { 5, 6, 2, 7 }
                }
            };
    }
}
