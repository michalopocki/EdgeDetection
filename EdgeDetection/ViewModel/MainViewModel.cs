using EdgeDetectionApp.Commands;
using EdgeDetectionApp.EdgeDetectorAlgorithms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EdgeDetectionApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        public Bitmap OriginalImage { get; set; }

        private Bitmap _processedImage;
        public Bitmap ProcessedImage
        {
            get => _processedImage;
            set => SetField(ref _processedImage, value);
        }
        private FrameworkElement _chartView;
        public FrameworkElement ChartView
        {
            get { return _chartView; }
            set
            {
                if (value.Equals(_chartView)) { return; }
                _chartView = value;
                OnPropertyChanged(nameof(ChartView));
            }
        }
        #endregion
        #region Commands
        public ICommand Process { get; set; }

        #endregion
        #region Constructor
        public MainViewModel()
        {
            OriginalImage = new Bitmap(@"E:\VS202022Projects\EdgeDetection\EdgeDetection\bin\Debug\net6.0-windows\ptak3.jpg");
            ProcessedImage = (Bitmap)OriginalImage.Clone();
            ChartView = new Views.ChartView();
            SetupCommands();
        }
        #endregion
        #region Methods
        private void SetupCommands()
        {
            Process = new ProcessImageCommand(this);
        }

        private void LoadImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Image";
            dialog.RestoreDirectory = true;
            dialog.Title = "Load Image From File";
            dialog.Filter = "";

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                dialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", dialog.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }
            dialog.Filter = String.Format("{0}{1}{2} ({3})|{3}", dialog.Filter, sep, "All Files", "*.*");
            dialog.DefaultExt = ".png";

            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult.HasValue && dialogResult.Value)
            {
                OriginalImage = new Bitmap(dialog.FileName);
                ProcessedImage = OriginalImage;
            }
        }
        private void SaveBitmapAs()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|Bitmap Image|*.bmp",
                RestoreDirectory = true,
                Title = "Save Processed Image To File",
            };
            var dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var tmp = ProcessedImage;
                using (var bmp = new Bitmap(tmp))
                {
                    if (File.Exists(dialog.FileName))
                    {
                        File.Delete(dialog.FileName);
                    }

                    switch (dialog.FilterIndex)
                    {
                        case 0:
                            bmp.Save(dialog.FileName, ImageFormat.Png);
                            break;
                        case 1:
                            bmp.Save(dialog.FileName, ImageFormat.Bmp);
                            break;
                    }
                }
                MessageBox.Show("Image Saved Successfully!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion
    }
}
