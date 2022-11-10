using EdgeDetectionLib.Histogram;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using EdgeDetectionApp.Messages;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using System;
using System.Windows;
using LiveCharts.Wpf;
using LiveCharts;

namespace EdgeDetectionApp.ViewModel
{
    public class ChartViewModel : ViewModelBase
    {
        #region Fields
        private readonly IHistogramFactory _histogramFactory;
        private readonly IMessenger _messenger;
        private ChartValues<int> r_values = new ChartValues<int>();
        private ChartValues<int> g_values = new ChartValues<int>();
        private ChartValues<int> b_values = new ChartValues<int>();
        private ChartValues<int> gray_values = new ChartValues<int>();
        private bool _rgbVisibility;
        private bool _grayVisibility;
        private double _threshold1;
        private double _threshold2;
        private bool _threshold2Visibility;
        #endregion

        #region Properties
        public ChartValues<int> R_Values {
            get => r_values;
            set => SetField(ref r_values, value); 
        }
        public ChartValues<int> G_Values {
            get => g_values;
            set => SetField(ref g_values, value); 
        }
        public ChartValues<int> B_Values {
            get => b_values;
            set => SetField(ref b_values, value); 
        }
        public ChartValues<int> Gray_Values 
        {
            get => gray_values;
            set => SetField(ref gray_values, value); 
        }

        public double Threshold1 { get => _threshold1; set => SetField(ref _threshold1, value); }
        public double Threshold2 { get => _threshold2; set => SetField(ref _threshold2, value); }
        public bool Threshold2Visibility { get => _threshold2Visibility; set => SetField(ref _threshold2Visibility, value); }
        public bool RGBVisibility { get => _rgbVisibility; set => SetField(ref _rgbVisibility, value); }
        public bool GrayVisibility { get => _grayVisibility; set => SetField(ref _grayVisibility, value); }
        #endregion

        #region Constructor
        public ChartViewModel(IHistogramFactory histogramFactory, IMessenger messenger)
        {
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

            if (isGrayscale == false)
            {
                RGBVisibility = true; GrayVisibility = false;
                R_Values = new ChartValues<int>(histogramResults.R_Series);
                G_Values = new ChartValues<int>(histogramResults.G_Series);
                B_Values = new ChartValues<int>(histogramResults.B_Series);
            }
            else
            {
                RGBVisibility = false; GrayVisibility = true;
                Gray_Values = new ChartValues<int>(histogramResults.Gray_Series);
            }
        }

        public override void Dispose()
        {
            _messenger.Unsubscribe<HistogramDataChangedMessage>(this);
            _messenger.Unsubscribe<ThresholdChangedMessage>(this);
            base.Dispose();
        }
        #endregion
    }
}
