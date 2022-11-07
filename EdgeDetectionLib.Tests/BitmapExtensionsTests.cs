using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdgeDetectionLib.Tests
{
    public class BitmapExtensionsTests
    {
        [Fact]
        public void SetGrayscale_PixelFormat_ShouldBe_Format8bppIndexed()
        {
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage);

            var expected = PixelFormat.Format8bppIndexed;

            using var grayscaleBmp = bitmap.ToGrayscale();

            var actual = grayscaleBmp.PixelFormat;

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void SetGrayscale_PixelFormat_ShouldBe_TheSameAsInputImage()
        {
            using var bitmap = new Bitmap(TestingConstants.TestJpgImage);

            var expected = bitmap.PixelFormat;
            var actual = bitmap.PixelFormat;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MakeNegavite_NegativePixel_ShouldHasProperValue()
        {
            for (int i = 0; i < 20; i++) 
            {
                using var bitmap = new Bitmap(TestingConstants.TestJpgImage);

                int x = RandomGenerator.GetPseudoIntegerWithinRange(0, bitmap.Width - 1);
                int y = RandomGenerator.GetPseudoIntegerWithinRange(0, bitmap.Height - 1);

                byte pixR = (byte)Math.Abs(bitmap.GetPixel(x, y).R - 255);
                byte pixG = (byte)Math.Abs(bitmap.GetPixel(x, y).G - 255);
                byte pixB = (byte)Math.Abs(bitmap.GetPixel(x, y).B - 255);

                bitmap.MakeNegative();

                byte pixRneg = bitmap.GetPixel(x, y).R;
                byte pixGneg = bitmap.GetPixel(x, y).G;
                byte pixBneg = bitmap.GetPixel(x, y).B;

                Assert.Equal(pixR, pixRneg);
                Assert.Equal(pixG, pixGneg);
                Assert.Equal(pixB, pixBneg);
            }
        }
    }
}
