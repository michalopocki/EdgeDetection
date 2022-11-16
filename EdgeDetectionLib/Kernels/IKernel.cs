using System;

namespace EdgeDetectionLib.Kernels
{
    public interface IKernel
    {
        public int M { get; }
        public int N { get; }
        double[][] Create();

    }
}
