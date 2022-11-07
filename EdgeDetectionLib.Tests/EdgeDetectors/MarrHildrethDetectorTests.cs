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
    public class MarrHildrethDetectorTests
    {
        private readonly MarrHildrethDetector _sut;
        public MarrHildrethDetectorTests()
        {
            var mock = new Mock<IMarrHildrethArgs>();
            mock.SetupGet(x => x.KernelSize).Returns(3);
            mock.SetupGet(x => x.Sigma).Returns(2.0);
            _sut = new MarrHildrethDetector(mock.Object);
        }

        [Fact]
        public void PropertyName_ShouldBe_Roberts()
        {
            string expected = "MarrHildreth";
            string actual = _sut.Name;

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
    }
}
