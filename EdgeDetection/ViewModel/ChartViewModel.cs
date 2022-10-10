using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace EdgeDetectionApp.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
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
