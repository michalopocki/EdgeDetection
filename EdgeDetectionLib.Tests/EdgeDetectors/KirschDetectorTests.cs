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
    public class KirschDetectorTests
    {
        private readonly KirschDetector _sut;
        public KirschDetectorTests()
        {
            var mock = new Mock<IGradientArgs>();
            _sut = new KirschDetector(mock.Object);
        }

        [Fact]
        public void PropertyName_ShouldBe_Roberts()
        {
            string expected = "Kirsch";
            string actual = _sut.Name;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FieldsMasks_SumOfAllElements_ShouldBeZero()
        {
            double expected = 0;
            var actualSums = new List<double>();

            actualSums.Add(_sut._E.SelectMany(item => item).Sum());
            actualSums.Add(_sut._N.SelectMany(item => item).Sum());
            actualSums.Add(_sut._NE.SelectMany(item => item).Sum());
            actualSums.Add(_sut._NW.SelectMany(item => item).Sum());
            actualSums.Add(_sut._S.SelectMany(item => item).Sum());
            actualSums.Add(_sut._SE.SelectMany(item => item).Sum());
            actualSums.Add(_sut._SW.SelectMany(item => item).Sum());
            actualSums.Add(_sut._W.SelectMany(item => item).Sum());

            Assert.All(actualSums, x => x.Equals(expected));
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

            var detector = new KirschDetector(mock.Object);

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

            var detector = new KirschDetector(mock.Object);
            var result = detector.DetectEdges();

            bool sameBitmaps = BitmapExtensions.CompareMemCmp(result.ProcessedImage, result.ImageBeforeThresholding);

            if (thresholding == false)
                Assert.True(sameBitmaps == true);
            else
                Assert.True(sameBitmaps == false);
        }
    }
}
