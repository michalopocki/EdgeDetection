using EdgeDetectionLib.Histogram;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests.Histogram
{
    public class RGBHistogramTests
    {
        private readonly RGBHistogram _sut;
        private readonly int _width;
        private readonly int _height;

        public RGBHistogramTests()
        {
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage);
            _sut = new RGBHistogram(bitmap);
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
            Assert.NotNull(actual.R_Series);
            Assert.NotNull(actual.G_Series);
            Assert.NotNull(actual.B_Series);
            Assert.Null(actual.Gray_Series);
            Assert.Equal(256, actual.R_Series.Count);
            Assert.Equal(256, actual.G_Series.Count);
            Assert.Equal(256, actual.B_Series.Count);
        }

        [Fact]
        public void Calculate_SumOfHistogramData_ShouldEqual_BitmapDimension()
        {
            var expected = _width * _height * 3;

            var histogramResults = _sut.Calculate();

            var actual = histogramResults.R_Series!.Sum() +
                         histogramResults.G_Series!.Sum() +
                         histogramResults.B_Series!.Sum();

            Assert.Equal(expected, actual);
        }
    }
}
