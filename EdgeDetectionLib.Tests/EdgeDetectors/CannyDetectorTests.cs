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
    public class CannyDetectorTests
    {

        private readonly CannyDetector _sut;
        public CannyDetectorTests()
        {
            var mock = new Mock<ICannyArgs>();
            mock.SetupGet(x=>x.THigh).Returns(40);
            mock.SetupGet(x => x.TLow).Returns(15);
           // mock.SetupGet(x => x.TLow).Returns(15);
            _sut = new CannyDetector(mock.Object);
        }

        [Fact]
        public void PropertyName_ShouldBe_Roberts()
        {
            string expected = "Canny";
            string actual = _sut.Name;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void FieldGx_SumOfAllElements_ShouldBeZero()
        {
            double expected = 0;
            double actual = _sut._Gx.SelectMany(item => item).Sum();

            Assert.Equal(expected, actual, 10);
        }

        [Fact]
        public void FieldGy_SumOfAllElements_ShouldBeZero()
        {
            double expected = 0;
            double actual = _sut._Gy.SelectMany(item => item).Sum();

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
        public void RoundGradientDirectionAngle_ShouldReturn_0_45_90_135()
        {
            var expectedValues = new List<double>() { 0.0, 45.0, 90.0, 135.0 };
            for (double i = -360; i <= 360; i += 3.13)
            {
                double value = CannyDetector.RoundGradientDirectionAngle(i);
                Assert.Contains(value, expectedValues);
            }
        }

        //[Fact]
        //public void CannyDetector_ShouldBeNotNull()
        //{

        //}

    }
}
