using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EdgeDetectionApp.Bahaviors
{
    public class CloseWindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty ButtonProperty =
            DependencyProperty.Register(
                "Button",
                typeof(Button),
                typeof(CloseWindowBehavior),
                new PropertyMetadata(null, CloseButton)
                );

        public Button Button
        {
            get { return (Button)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
        }

        private static void CloseButton(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (d as CloseWindowBehavior).AssociatedObject;

            RoutedEventHandler buttonClick = (object sender, RoutedEventArgs _e) => { window.Close(); };

            if (e.OldValue != null) ((Button)e.OldValue).Click -= buttonClick;
            if (e.NewValue != null) ((Button)e.NewValue).Click += buttonClick;
        }
    }
}
