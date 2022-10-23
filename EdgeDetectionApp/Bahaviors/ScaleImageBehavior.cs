using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace EdgeDetectionApp.Bahaviors
{
    public class ScaleImageBehavior : Behavior<Image>
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private double _actualScale = 1;
        private readonly double _step = 0.05;
        private int _hashcode;

        public ScaleImageBehavior()
        {
            _timer.Interval = TimeSpan.FromSeconds(0.005);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _timer.Stop();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            var image = (Image?)sender;
            if (image is not null)
            {
                _hashcode = image.Source.GetHashCode();
            }
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.PreviewMouseWheel += Image_PreviewMouseWheel;
            AssociatedObject.PreviewMouseLeftButtonDown += Image_PreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            AssociatedObject.PreviewMouseWheel -= Image_PreviewMouseWheel;
            AssociatedObject.PreviewMouseLeftButtonDown -= Image_PreviewMouseLeftButtonDown;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            if (e.ClickCount == 2)
            {
                _timer.Stop();
                _actualScale = 1;
                image.LayoutTransform = new ScaleTransform(_actualScale, _actualScale, 0.5, 0.5);
            }
            else
            {
                _timer.Start();
            }
        }

        private void Image_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var image = (Image)sender;

            if (e.Delta > 0)
            {
                _actualScale += _step;
                image.LayoutTransform = new ScaleTransform(_actualScale, _actualScale, 0.5, 0.5);
            }
            else if (e.Delta < 0)
            {
                if (_actualScale > 2 * _step)
                    _actualScale -= _step;
                image.LayoutTransform = new ScaleTransform(_actualScale, _actualScale, 0.5, 0.5);
            }
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            var image = (Image?)sender;
            if (image is not null)
            {
                int newHashcode = image.Source.GetHashCode();
                if (newHashcode != _hashcode)
                {
                    _actualScale = 1;
                    image.LayoutTransform = new ScaleTransform(_actualScale, _actualScale, 0.5, 0.5);
                    _hashcode = newHashcode;
                }
            }
        }
    }
}
