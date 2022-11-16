using EdgeDetectionApp.ViewModel;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdgeDetectionAppTests.ViewModels
{
    public class ViewModelLocatorTests
    {
        ViewModelLocator _sut = new ViewModelLocator();

        [Fact]
        public void MainViewModels_ShouldBeNotNull()
        {
            var result = _sut.MainViewModel;
            Assert.NotNull(result);

            //SobelDetector sob = new SobelDetector()
        }

        [Fact]
        public void ImageViewModels_ShouldBeNotNull()
        {
            var result = _sut.ImageViewModel;
            Assert.NotNull(result);
        }

        [Fact]
        public void VideoViewModels_ShouldBeNotNull()
        {
            var result = _sut.VideoViewModel;
            Assert.NotNull(result);
        }

        [Fact]
        public void OptionsViewModels_ShouldBeNotNull()
        {
            var result = _sut.OptionsViewModel;
            Assert.NotNull(result);
        }

        [Fact]
        public void ChartViewModels_ShouldBeNotNull()
        {
            var result = _sut.ChartViewModel;
            Assert.NotNull(result);
        }
    }
}
