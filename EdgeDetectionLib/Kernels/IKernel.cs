using System;

namespace EdgeDetectionLib.Kernels
{
    /// <summary>
    /// Interface abstracting the kernel.
    /// </summary>
    public interface IKernel
    {
        /// <summary>
        /// Number of kernel rows
        /// </summary>
        public int M { get; }

        /// <summary>
        /// Number of kernel columns
        /// </summary>
        public int N { get; }

        /// <summary>
        /// Creates kernel (mask).
        /// </summary>
        /// <returns>
        /// Two-dimensional array that represents kernal (mask).
        /// </returns>
        double[][] Create();

    }
}
