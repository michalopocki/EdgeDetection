using EdgeDetectionLib.EdgeDetectionAlgorithms;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdgeDetectionLib.Tests.EdgeDetectors
{
    public class RobertsDetectorTests
    {
        private readonly RobertsDetector _sut;
        public RobertsDetectorTests()
        {
            var mock = new Mock<IGradientArgs>();
            _sut = new RobertsDetector(mock.Object);
        }

        [Fact]
        public void PropertyName_ShouldBe_Roberts()
        {
            string expected = "Roberts";
            string actual = _sut.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FieldGx_SumOfAllElements_ShouldBeZero()
        {
            double expected = 0;
            double actual = _sut._Gx.SelectMany(item => item).Sum();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FieldGy_SumOfAllElements_ShouldBeZero()
        {
            double expected = 0;
            double actual = _sut._Gy.SelectMany(item => item).Sum();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DetectEdges_CallWithNullPixelArray_ThrowsArgumentNullException()
        {
            Action act = () => _sut.DetectEdges();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void SetBitmap_AndCall_DetectEdges_ShouldReturnEdgeDetectionResult()
        {
            using var bitmap = new Bitmap(@"test.jpg");

            _sut.SetBitmap(bitmap);
            var actual = _sut._pixelMatrix;
            var reslut = _sut.DetectEdges();

            Assert.IsType<EdgeDetectionResult>(reslut);
            Assert.NotNull(reslut.ProcessedImage);
            Assert.NotNull(reslut.ImageBeforeThresholding);
            Assert.IsType<PixelMatrix>(actual);
            Assert.NotNull(actual);
        }

        [Fact]
        public void DetectEdges_ShouldReturnEdgeDetectionResult()
        {
            using var bitmap = new Bitmap(@"test.jpg");

            var mock = new Mock<IGradientArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            
            var robertsDetector = new RobertsDetector(mock.Object);

            var result = robertsDetector.DetectEdges();

            Assert.IsType<EdgeDetectionResult>(result);
            Assert.NotNull(result.ProcessedImage);
            Assert.NotNull(result.ImageBeforeThresholding);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DetectEdges_Thresholding_ResultsShouldBeTheSameOrNot(bool thresholding)
        {
            using var bitmap = new Bitmap(@"test.jpg");

            var mock = new Mock<IGradientArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            mock.SetupGet(m => m.Thresholding).Returns(thresholding);

            var robertsDetector = new RobertsDetector(mock.Object);
            var result = robertsDetector.DetectEdges();

            bool sameBitmaps = BitmapExtensions.CompareMemCmp(result.ProcessedImage, result.ImageBeforeThresholding);
            
            if(thresholding == false)
                Assert.True(sameBitmaps == true);
            else
                Assert.True(sameBitmaps == false);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public void CutSides_ShouldCutBitmapEdges(int kernelSize)
        {
            using var bitmap = new Bitmap(@"test.jpg");
            var expectedWidth = bitmap.Width - 2 * Math.Ceiling((double)kernelSize / 2);
            var expectedHeight = bitmap.Height - 2 * Math.Ceiling((double)kernelSize / 2);

            var mock = new Mock<IGradientArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            mock.SetupGet(m => m.Prefiltration).Returns(true);
            mock.SetupGet(m => m.KernelSize).Returns(kernelSize);
            mock.SetupGet(m => m.Sigma).Returns(2.0);

            var robertsDetector = new RobertsDetector(mock.Object);
            var result = robertsDetector.DetectEdges();

            var actualWidth = result.ProcessedImage.Width;
            var actualHeight = result.ProcessedImage.Height;

            Assert.Equal(expectedWidth, actualWidth);
            Assert.Equal(expectedHeight, actualHeight);
        }

        [Fact]
        public void CutSides_OversizedKernel_ShouldThrowArgumentException()
        {
            using var bitmap = new Bitmap(@"test.jpg");
            int kernelSize = bitmap.Width / 2 + 10;

            var mock = new Mock<IGradientArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            mock.SetupGet(m => m.Prefiltration).Returns(true);
            mock.SetupGet(m => m.KernelSize).Returns(kernelSize);
            mock.SetupGet(m => m.Sigma).Returns(2.0);

            var robertDetector = new RobertsDetector(mock.Object);

            Action act = () => robertDetector.DetectEdges();
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.Throws<ArgumentException>(act);
        }
    }
}
