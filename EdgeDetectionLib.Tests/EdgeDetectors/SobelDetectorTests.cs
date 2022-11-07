using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests.EdgeDetectors
{
    public class SobelDetectorTests
    {
        private readonly SobelDetector _sut;
        public SobelDetectorTests()
        {
            var mock = new Mock<IGradientArgs>();
            _sut = new SobelDetector(mock.Object);
        }

        [Fact]
        public void PropertyName_ShouldBe_Roberts()
        {
            string expected = "Sobel";
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
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage);

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
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage);

            var mock = new Mock<IGradientArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);

            var detector = new SobelDetector(mock.Object);

            var result = detector.DetectEdges();

            Assert.IsType<EdgeDetectionResult>(result);
            Assert.NotNull(result.ProcessedImage);
            Assert.NotNull(result.ImageBeforeThresholding);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DetectEdges_ResultsShouldBeTheSameOrNotDependingOnThresholding(bool thresholding)
        {
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage);

            var mock = new Mock<IGradientArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            mock.SetupGet(m => m.Thresholding).Returns(thresholding);

            var detector = new SobelDetector(mock.Object);
            var result = detector.DetectEdges();

            bool sameBitmaps = BitmapExtensions.CompareMemCmp(result.ProcessedImage, result.ImageBeforeThresholding);

            if (thresholding == false)
                Assert.True(sameBitmaps == true);
            else
                Assert.True(sameBitmaps == false);
        }
    }
}
