﻿using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using MvvmDialogs;
using System.Windows.Input;

namespace EdgeDetectionApp.Bahaviors
{
    public class CloseWindowBehavior2 : Behavior<Window>
    {
        private Window? _window;

        public static readonly DependencyProperty ButtonProperty =
             DependencyProperty.Register(
             "UIElement",
             typeof(UIElement),
             typeof(CloseWindowBehavior2));

        public UIElement UIElement
        {
            get { return (UIElement)GetValue(ButtonProperty); }
            set { SetValue(ButtonProperty, value); }
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

            switch(UIElement)
            {
                case Button button:
                    {
                        button.Click += CloseWindow2_Click;
                        break;
                    }
                case MenuItem menuItem:
                    {
                        menuItem.Click += CloseWindow2_Click;
                        break;
                    }
                default:
                    {
                        UIElement.MouseUp += CloseWindow2_Click;
                        break;
                    }
            }
        }

        private void CloseWindow2_Click(object sender, RoutedEventArgs e)
        {
            if (_window is not null)
            {
                _window.Close();
            }
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            UIElement.MouseLeftButtonDown -= CloseWindow2_Click;
        }
    }
}
