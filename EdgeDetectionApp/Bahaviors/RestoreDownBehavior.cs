using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using WindowState = System.Windows.WindowState;

namespace EdgeDetectionApp.Bahaviors
{
    public class RestoreDownBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty ButtonProperty =
                DependencyProperty.Register(
                "Button",
                typeof(Button),
                typeof(RestoreDownBehavior),
                new PropertyMetadata(null, RestoreDownButton));

        public Button Button
        {
            get { return (Button)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        private static void RestoreDownButton(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (d as RestoreDownBehavior).AssociatedObject;

            RoutedEventHandler buttonClick = (object sender, RoutedEventArgs _e) => 
            {
                window.WindowState = window.WindowState == WindowState.Normal ? 
                                     WindowState.Maximized : WindowState.Normal;
             };

            if (e.OldValue != null) ((Button)e.OldValue).Click -= buttonClick;
            if (e.NewValue != null) ((Button)e.NewValue).Click += buttonClick;            
        }
    }
}
