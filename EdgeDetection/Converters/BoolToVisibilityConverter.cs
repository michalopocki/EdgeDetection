using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EdgeDetectionApp.Converters
{
    public class BoolToVisibilityConverter : OneWayConverter , IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool)
                return null;
            return (bool)value ? TrueValue : FalseValue;
        }

    }
}
