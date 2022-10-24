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
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
            SetButtons(); 
        }

        public static readonly DependencyProperty ButtonCloseProperty =
                 DependencyProperty.Register(
                 name: "ButtonClose",
                 propertyType: typeof(Button),
                 ownerType: typeof(TitleBar),
                typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty ButtonMinimizeProperty =
                 DependencyProperty.Register(
                 name: "ButtonMinimize",
                 propertyType: typeof(Button),
                 ownerType: typeof(TitleBar),
                 typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static readonly DependencyProperty ButtonRestoreDownProperty =
                 DependencyProperty.Register(
                 name: "ButtonRestoreDown",
                 propertyType: typeof(Button),
                 ownerType: typeof(TitleBar),
                 typeMetadata: new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public Button ButtonMinimize
        {
            get => (Button)GetValue(ButtonMinimizeProperty);
            set => SetValue(ButtonMinimizeProperty, value);
        }
        public Button ButtonClose
        {
            get => (Button)GetValue(ButtonCloseProperty);
            set => SetValue(ButtonCloseProperty, value);
        }
        public Button ButtonRestoreDown
        {
            get => (Button)GetValue(ButtonRestoreDownProperty);
            set => SetValue(ButtonRestoreDownProperty, value);
        }
        private void SetButtons()
        {
            ButtonMinimize = btnMinimizeWindow;
            ButtonRestoreDown = btnRestoreDownWindow;
            ButtonClose = btnCloseWindow;
        }
    }
}
