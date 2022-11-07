using EdgeDetectionLib.Kernels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Tests.Kernels
{
    public abstract class KernelTests
    {
        [Theory]
        [InlineData(5, 5)]
        [InlineData(9, 3)]
        public void Create_KerenlDimensions_ShouldBe_TheSameAsConstructorInput(int M, int N)
        {
            IKernel kernel = GenerateKernel(M, N, 2.0);
            var mask = kernel.Create();

            int expectedM = M;
            int actualM = mask.Length;

            Assert.Equal(expectedM, actualM);

            int expectedN = N;
            for (int i = 0; i < M; i++)
            {
                int actualN = mask[i].Length;

                Assert.Equal(expectedN, actualN);
            }
        }

        protected abstract IKernel GenerateKernel(int M, int N, double sigma);

    }
}
