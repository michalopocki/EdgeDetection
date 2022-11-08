using EdgeDetectionApp.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EdgeDetectionApp.Commands
{
    public class SaveSnapshotCommand : CommandBase
    {
        private readonly VideoViewModel _videoViewModel;

        public SaveSnapshotCommand(VideoViewModel videoViewModel)
        {
            _videoViewModel = videoViewModel;
        }

        public override void Execute(object? parameter)
        {
            SaveSnapshot();
        }

        private void SaveSnapshot()
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Snapshot1";
            dialog.DefaultExt = ".png";
            var dialogresult = dialog.ShowDialog();
            if (dialogresult != true)
            {
                return;
            }
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_videoViewModel.CurrentImage));
            using (var filestream = new FileStream(dialog.FileName, FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }
    }
}
