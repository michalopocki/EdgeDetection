using EdgeDetectionLib.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EdgeDetectionLib.Tests.Kernels
{
    public class GaussianKernelTests : KernelTests
    {
        [Theory]
        [InlineData(5, 5, 1.2)]
        [InlineData(9, 3, 2.3)]
        [InlineData(20, 13, 0.1)]
        public void Create_Sum_ShouldBe_One(int M, int N, double sigma)
        {
            var kernel = GenerateKernel(M, N, sigma);
            var mask = kernel.Create();

            var sum = mask.SelectMany(item => item).Sum();

            Assert.Equal(1, sum, 8);
        }
        protected override IKernel GenerateKernel(int M, int N, double sigma)
        {
            return new GaussianKernel(M, N, sigma);
        }
    }
}
