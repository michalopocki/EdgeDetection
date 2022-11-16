using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace EdgeDetectionApp.Bahaviors
{
    public class WindowMoveBehavior : Behavior<Window>
    {
        private Window? _window;

        public static readonly DependencyProperty UserControlProperty =
                DependencyProperty.Register(
                "UserControl",
                typeof(UserControl),
                typeof(WindowMoveBehavior));
        public UserControl UserControl
        {
            get { return (UserControl)GetValue(UserControlProperty); }
            set { SetValue(UserControlProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;

            _window = (Window)AssociatedObject;
        }
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            UserControl.MouseLeftButtonDown += Window_MouseDown;
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            UserControl.MouseLeftButtonDown -= Window_MouseDown;
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
