using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EdgeDetectionApp.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
            SetUIElements();
        }
        public static readonly DependencyProperty CloseProperty =
                DependencyProperty.Register(
                name: "CloseMenuItem",
                propertyType: typeof(MenuItem),
                ownerType: typeof(MenuBar),
                typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public MenuItem CloseMenuItem
        {
            get => (MenuItem)GetValue(CloseProperty);
            set => SetValue(CloseProperty, value);
        }
        private void SetUIElements()
        {
            CloseMenuItem = closeMenuItem;
        }
    }
}
