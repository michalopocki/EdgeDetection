using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System;

namespace EdgeDetectionApp.Bahaviors
{
    public class WindowMoveBehavior : Behavior<Window>
    {
        private Window? _window;

        public static readonly DependencyProperty GridProperty =
                DependencyProperty.Register(
                "Grid",
                typeof(Grid),
                typeof(WindowMoveBehavior));
        public Grid Grid
        {
            get { return (Grid)GetValue(GridProperty); }
            set { SetValue(GridProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;

            var window = (Window)AssociatedObject;
            _window = window;
        }
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            Grid.MouseLeftButtonDown += Window_MouseDown;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            Grid.MouseLeftButtonDown -= Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_window is not null)
            {
                _window.DragMove();
            }
        }
    }
}
