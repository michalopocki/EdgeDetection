using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests
{
    public class PixelMatrixTests
    {
        PixelMatrix _sut = new PixelMatrix(new Bitmap(@"test.jpg"));
        PixelMatrix _sutGray = new PixelMatrix(new Bitmap(@"test.jpg").ToGrayscale());

        [Fact]
        public void PixelMatrix_Dimension_ShouldBe_One()
        {
            var expected = 1;
            var actual = _sutGray.Dimensions;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void PixelMatrix_Dimension_ShouldBe_Three()
        {
            var expected = 3;
            var actual = _sut.Dimensions;

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(77, 55, 0)]
        [InlineData(10, 20, 1)]
        [InlineData(40, 76, 2)]
        public void GetPixel_ShouldBe_TheSameAsGetPixelFromBitmap(int x, int y, int dimension)
        {
            using var colorBmp = new Bitmap(@"test.jpg");
            Color bmpPixel = colorBmp.GetPixel(x, y);
            double expectedPixel = 0;

            if (dimension == 0)
                expectedPixel = bmpPixel.B;
            else if (dimension == 1)
                expectedPixel = bmpPixel.G;
            else if (dimension == 2)
                expectedPixel = bmpPixel.R;


            double acturalPixel = _sut.GetPixel(x, y, dimension);

            Assert.Equal(expectedPixel, acturalPixel);
        }

        [Theory]
        [InlineData(-1, 0, 0)]
        [InlineData(77, int.MaxValue, 1)]
        [InlineData(10, 10, 4)]
        public void GetPixel_ShouldThrow_ArgumentOutOfRangeException_IfInvalidInput(int x, int y, int dimension)
        {
            Action act = () => _sut.GetPixel(x, y, dimension);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Theory]
        [InlineData(0, -10, 0)]
        [InlineData(int.MaxValue, 0, 1)]
        [InlineData(0, 0, 4)]
        public void SetPixel_ShouldThrow_ArgumentOutOfRangeException_IfInvalidInput(int x, int y, int dimension)
        {
            using var colorBmp = new Bitmap(@"test.jpg");
            var pixelMatrix = new PixelMatrix(colorBmp);

            Action act = () => pixelMatrix.SetPixel(x, y, dimension, 50);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.Throws<ArgumentOutOfRangeException>(act);
        }

        [Theory]
        [InlineData(0, 0, 0, 5.3)]
        [InlineData(77, 55, 0, -5.1)]
        [InlineData(10, 20, 1, double.MaxValue)]
        [InlineData(40, 76, 2, double.MinValue)]
        public void SetPixel_ShouldBe_TheSameAsGetPixel(int x, int y, int dimension, double value)
        {
            using var colorBmp = new Bitmap(@"test.jpg");
            var pixelMatrix = new PixelMatrix(colorBmp);
            pixelMatrix.SetPixel(x, y, dimension, value);

            double actualPixel = pixelMatrix.GetPixel(x, y, dimension);

            Assert.Equal(value, actualPixel);
        }

        [Fact]
        public void Abs_AllValuesShouldBeGreaterThanZero()
        {
            using var colorBmp = new Bitmap(@"test.jpg");
            var pixelMatrix = new PixelMatrix(colorBmp);

            pixelMatrix.Abs();

            var lowerThanZero = pixelMatrix.Bits.Where(x => x < 0).ToList();

            Assert.Empty(lowerThanZero);
        }

        [Fact]
        public void Mean_LinqValue_ShouldEquals_MeanValue()
        {
            double expetectedMean = _sut.Bits.Select(x => Math.Abs(x)).Average();
            double actualMean = _sut.Mean();

            Assert.Equal(expetectedMean, actualMean, 8);
        }

        [Fact]
        public void Normalize_ValuesSholudBeBeetween0and255()
        {
            using var colorBmp = new Bitmap(@"test.jpg");
            var pixelMatrix = new PixelMatrix(colorBmp);
            pixelMatrix.Normalize();

            bool anyIncorrectValue = pixelMatrix.Bits.Any(x => x < 0 || x > 255);
            Assert.False(anyIncorrectValue);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(70)]
        [InlineData(255)]
        public void Thresholding_ValuesShouldBeZeroOr255(int threshold)
        {
            using var colorBmp = new Bitmap(@"test.jpg");
            var pixelMatrix = new PixelMatrix(colorBmp);
            pixelMatrix.Thresholding(threshold);

            bool anyIncorrectValue = pixelMatrix.Bits.Any(x => x != 0 && x != 255);
            Assert.False(anyIncorrectValue);
        }

        [Fact]
        public void ToBitmap_ColorscaleBmp_ShouldReturnBitmapAndNotNull()
        {
            var bitmap = _sut.ToBitmap();

            Assert.IsType<Bitmap>(bitmap);
            Assert.NotNull(bitmap);
            Assert.Equal(PixelFormat.Format24bppRgb, bitmap.PixelFormat);
        }

        [Fact]
        public void ToBitmap_GrayscaleBmp_ShouldReturnBitmapAndNotNull()
        {
            var bitmap = _sutGray.ToBitmap();

            Assert.IsType<Bitmap>(bitmap);
            Assert.NotNull(bitmap);
            Assert.Equal(PixelFormat.Format8bppIndexed, bitmap.PixelFormat);
        }

        [Fact]
        public void LoadBitmapData_BitsLengthShouldBeAsBitmapDimensions()
        {
            using var bitmap = new Bitmap(@"test.jpg");
            int width = bitmap.Width;
            int height = bitmap.Height;
            int dimemsions = BitmapExtensions.GetBytesPerPixel(bitmap.PixelFormat);
            int expected = width * height * dimemsions;

            var pixelMatrix = new PixelMatrix(width, height, dimemsions);
            pixelMatrix.LoadBitmapData(bitmap);

            int actual = pixelMatrix.Bits.Length;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AdditionOperatorOverload_BitsTableShouldBeEqualToSumTable()
        {
            int width = 10;
            int height = 20;
            int dimemsions = 3;
            int lowerBound = -20;
            int upperBound = 30;
            int count = width * height * dimemsions;

            var pixelMatrix1 = new PixelMatrix(width, height, dimemsions);
            var pixelMatrix2 = new PixelMatrix(width, height, dimemsions);

            double[] numbers1 = RandomGenerator.GenereateDoubleNumbers(count, lowerBound, upperBound);
            double[] numbers2 = RandomGenerator.GenereateDoubleNumbers(count, lowerBound, upperBound);
            double[] sumNumbers = numbers1.Zip(numbers2, (x, y) => x + y).ToArray();

            pixelMatrix1.Bits = numbers1;
            pixelMatrix2.Bits = numbers2;
            PixelMatrix sum = pixelMatrix1 + pixelMatrix2;

            Assert.Equal(sumNumbers, sum.Bits);
        }

    }
}
