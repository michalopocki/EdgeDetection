using EdgeDetection.EdgeDetectorAlgorithms;
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

namespace EdgeDetection.ViewModel
{
    public class ImageViewModel :BaseViewModel
    {
        #region Properties
        private Bitmap _originalImage;
        public Bitmap OriginalImage
        {
            get { return _originalImage; }
            set
            {
                if (value.Equals(_originalImage)) { return; }
                _originalImage = value;
                OnPropertyChanged(nameof(OriginalImage));
            }
        }
        private Bitmap _processedImage;
        public Bitmap ProcessedImage
        {
            get { return _processedImage; }
            set
            {
                if (value.Equals(_processedImage)) { return; }
                _processedImage = value;
                OnPropertyChanged(nameof(ProcessedImage));
            }
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
        private UICommand _process;
        public UICommand Process
        {
            get { return _process; }
            set
            {
                if (value.Equals(_process)) { return; }
                _process = value;
                OnPropertyChanged(nameof(Process));
            }
        }
        private UICommand _load;
        public UICommand Load
        {
            get { return _load; }
            set
            {
                if (value.Equals(_load)) { return; }
                _load = value;
                OnPropertyChanged(nameof(Load));
            }
        }
        private UICommand _saveAs;
        public UICommand SaveAs
        {
            get { return _saveAs; }
            set
            {
                if (value.Equals(_saveAs)) { return; }
                _saveAs = value;
                OnPropertyChanged(nameof(SaveAs));
            }
        }
        #endregion
        #region Constructor
        public ImageViewModel()
        {
            OriginalImage = new Bitmap(@"E:\VS202022Projects\EdgeDetection\EdgeDetection\bin\Debug\net6.0-windows\ptak3.jpg");
            ProcessedImage = OriginalImage;
            ChartView = new Views.ChartView();
            SetupCommands();
        }
        #endregion
        #region Methods
        private void SetupCommands()
        {
            //Process = UICommand.Regular(SetBrightness, () => { return false; });
            //Process = UICommand.Thread(SetBrightness);
            Process = UICommand.Regular(Processing);
            Load = UICommand.Regular(LoadImage);
            SaveAs = UICommand.Regular(SaveBitmapAs);
        }
        private void Processing()
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //Bitmap bitmap = BitmapExtensions.MakeGrayscale(ProcessedImage);
            //Pixel[,] pixels = BitmapExtensions.BitmapToDoubleArray(bitmap);
            //ProcessedImage = bitmap;
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //Pixel[,] pixAr = BitmapExtensions.BitmapToDoubleArray(OriginalImage);

            // var watch = System.Diagnostics.Stopwatch.StartNew();
            // TestDetector testDetector = new TestDetector(pixAr);

            // var watch = System.Diagnostics.Stopwatch.StartNew();
            //ProcessedImage = testDetector.MakeGreyscale();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            RobertsDetector robertsDetector = new RobertsDetector(OriginalImage);
            // robertsDetector.MakeGreyscale();
            // var watch = System.Diagnostics.Stopwatch.StartNew();
            ProcessedImage = robertsDetector.DetectEdges();
            watch.Stop();
            //var img = robertsDetector.DoNothing();
            //ProcessedImage = robertsDetector.ApplyThresholding(100);
            // ProcessedImage = robertsDetector.MakeNegative();
            //watch.Stop();
            System.Diagnostics.Trace.WriteLine("Roberts detector:" + watch.ElapsedMilliseconds + " ms");
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
