using EdgeDetectionLib.Histogram;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests.Histogram
{
    public class HistogramFactoryTests
    {
        HistogramFactory _sut = new HistogramFactory();

        [Fact]
        public void Create_ReturnType_ShouldBe_GrayHistogram_IfGrayscaleBitmap()
        {
            using var grayBitmap = new Bitmap(TestingConstants.TestJpgImage).ToGrayscale();

            var expected = typeof(GrayHistogram);

            var actual = _sut.Create(grayBitmap);

            Assert.Equal(expected, actual.GetType());
            Assert.NotNull(actual);
        }

        [Fact]
        public void Create_ReturnType_ShouldBe_RGBHistogram_IfRGBeBitmap()
        {
            using var rgbBitmap = new Bitmap(TestingConstants.TestJpgImage);

            var expected = typeof(RGBHistogram);

            var actual = _sut.Create(rgbBitmap).GetType();

            Assert.Equal(expected, actual);
            Assert.NotNull(actual);
        }

    }
}
