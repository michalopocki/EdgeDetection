using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EdgeDetectionApp.Bahaviors
{
    public class StartStopButtonBehavior : Behavior<Button>
    {
        private readonly string _stop = "Stop";
        private readonly string _start = "Start";
        private bool _isStart = true;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.Click += ButtonClick;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            AssociatedObject.PreviewMouseWheel -= ButtonClick;
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if(_isStart)
            {
                button.Content = _stop;
                button.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                _isStart = false;
            }
            else
            {
                button.Content = _start;
                button.Background = new SolidColorBrush(Color.FromRgb(0, 179, 0));
                _isStart = true;
            }
        }
    }
}
