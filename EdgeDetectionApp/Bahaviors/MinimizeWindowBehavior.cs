using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EdgeDetectionApp.Bahaviors
{
    public class MinimizeWindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty ButtonProperty =
                DependencyProperty.Register(
                "Button",
                typeof(Button),
                typeof(MinimizeWindowBehavior),
                new PropertyMetadata(null, MinimizeButton)
        );
        public Button Button
        {
            get { return (Button)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }
        private static void MinimizeButton(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (d as MinimizeWindowBehavior).AssociatedObject;

            RoutedEventHandler buttonClick = (object sender, RoutedEventArgs _e) => { window.WindowState = WindowState.Minimized; };

            if (e.OldValue != null) ((Button)e.OldValue).Click -= buttonClick;
            if (e.NewValue != null) ((Button)e.NewValue).Click += buttonClick;
        }
    }
}
