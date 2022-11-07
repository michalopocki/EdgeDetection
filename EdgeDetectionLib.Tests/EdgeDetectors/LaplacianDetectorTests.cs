using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using EdgeDetectionLib.EdgeDetectionAlgorithms;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace EdgeDetectionLib.Tests.EdgeDetectors
{
    public class LaplacianDetectorTests
    {
        private LaplacianDetector _sut;
        public LaplacianDetectorTests()
        {
            var mock = new Mock<ILaplacianArgs>();
            _sut = new LaplacianDetector(mock.Object);
        }

        [Fact]
        public void PropertyName_ShouldBe_Roberts()
        {
            string expected = "Laplacian";
            string actual = _sut.Name;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(1)]
        public void FieldKernel_SumOfAllElements_ShouldBeZero(double alpha)
        {
            var mock = new Mock<ILaplacianArgs>();
            mock.SetupGet(x => x.Alpha).Returns(alpha);
            _sut = new LaplacianDetector(mock.Object);

            double expected = 0;
            double actual = _sut._mask.SelectMany(item => item).Sum();

            Assert.Equal(expected, actual, 10);
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

            var mock = new Mock<ILaplacianArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            mock.SetupGet(x => x.Alpha).Returns(0.1);

            var detector = new LaplacianDetector(mock.Object);

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

            var mock = new Mock<ILaplacianArgs>();
            mock.SetupGet(m => m.ImageToProcess).Returns(bitmap);
            mock.SetupGet(m => m.Thresholding).Returns(thresholding);
            mock.SetupGet(x => x.Alpha).Returns(0.1);

            var detector = new LaplacianDetector(mock.Object);
            var result = detector.DetectEdges();

            bool sameBitmaps = BitmapExtensions.CompareMemCmp(result.ProcessedImage, result.ImageBeforeThresholding);

            if (thresholding == false)
                Assert.True(sameBitmaps == true);
            else
                Assert.True(sameBitmaps == false);
        }
    }
}
