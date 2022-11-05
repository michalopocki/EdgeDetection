using EdgeDetectionLib.EdgeDetectionAlgorithms;
using EdgeDetectionLib.EdgeDetectionAlgorithms.Factory;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.ArgsBuilders;
using EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdgeDetectionLib.Tests.EdgeDetectors
{
    public class EdgeDetectorFactoryTests
    {
        EdgeDetectorFactory _sut = new EdgeDetectorFactory();

        [Fact]
        public void GetAll_ReturnsEightInstances_AfterInitialized()
        {
            int expected = 8;

            var actual = _sut.GetAll().Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAll_FirstDetectorNameShouldBeCanny()
        {
            string expected = "Canny";
            var actual = _sut.GetAll().First();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAll_LastDetectorNameShouldBeSobel()
        {
            string expected = "Sobel";
            var actual = _sut.GetAll().Last();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [ClassData(typeof(ArgsClassData))]
        public void Get_ShouldBe_DetectorType(string name, Type type, IEdgeDetectorArgs args)
        {
            Type expected = type;
            var actual = _sut.Get(name, args);

            Assert.IsType(expected, actual);
        }

        private class ArgsClassData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    "Roberts",
                    typeof(RobertsDetector),
                    new Mock<IGradientArgs>().Object
                };
                yield return new object[] {
                    "Sobel",
                    typeof(SobelDetector),
                    new Mock<IGradientArgs>().Object
                };
                yield return new object[] {
                    "Prewitt",
                    typeof(PrewittDetector),
                    new Mock<IGradientArgs>().Object
                };
                yield return new object[] {
                    "FreiChen",
                    typeof(FreiChenDetector),
                    new Mock<IGradientArgs>().Object
                };
                yield return new object[] {
                    "Kirsch",
                    typeof(KirschDetector),
                    new Mock<IGradientArgs>().Object
                };
                yield return new object[] {
                    "Canny",
                    typeof(CannyDetector),
                    new Mock<ICannyArgs>().Object
                };
                yield return new object[] {
                    "MarrHildreth",
                    typeof(MarrHildrethDetector),
                    new Mock<IMarrHildrethArgs>().Object
                };
                yield return new object[] {
                    "Laplacian",
                    typeof(LaplacianDetector),
                    new Mock<ILaplacianArgs>().Object
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

    }
}
