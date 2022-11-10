using EdgeDetectionLib.Histogram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests.Histogram
{
    public class GrayHistogramTests
    {
        private readonly GrayHistogram _sut;
        private readonly int _width;
        private readonly int _height;

        public GrayHistogramTests()
        {
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage).ToGrayscale();
            _sut = new GrayHistogram(bitmap);
            _width = bitmap.Width;
            _height = bitmap.Height;
        }

        [Fact]
        public void Calculate_ShouldReturn_HistogramResults()
        {
            var expected = typeof(HistogramResults);

            var actual = _sut.Calculate();

            Assert.NotNull(actual);
            Assert.Equal(expected, actual.GetType());
            Assert.Null(actual.R_Series);
            Assert.Null(actual.G_Series);
            Assert.Null(actual.B_Series);
            Assert.NotNull(actual.Gray_Series);
            Assert.Equal(256, actual.Gray_Series.Count);
        }

        [Fact]
        public void Calculate_SumOfHistogramData_ShouldEqual_BitmapDimension()
        {
            var expected = _width * _height;

            var histogramResults = _sut.Calculate();

            var actual = histogramResults.Gray_Series!.Sum();

            Assert.Equal(expected, actual);
        }

    }
}
