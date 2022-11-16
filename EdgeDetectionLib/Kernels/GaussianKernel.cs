using System;

namespace EdgeDetectionLib.Kernels
{
    /// <summary>
    /// Class that creates Guassian kernel.
    /// </summary>
    public class GaussianKernel : KernelBase
    {
        ///<summary> Gaussian standard deviation of gaussian kernel.</summary>
        public double Sigma { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianKernel"/> class.
        /// </summary>
        /// <param name="M"> Number of rows. </param>
        /// <param name="N"> Number of columns. </param>
        /// <param name="sigma"> Gaussian standard deviation of gaussian kernel. </param>
        public GaussianKernel(int M, int N, double sigma) : base(M, N)
        {
            Sigma = sigma;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GaussianKernel"/> class.
        /// </summary>
        /// <param name="MxN"> Number of rows and columns. </param>
        /// <param name="sigma"> Gaussian standard deviation of gaussian kernel. </param>
        public GaussianKernel(int MxN, double sigma) : base(MxN)
        {
            Sigma = sigma;
        }

        /// <summary>
        /// Creates Gaussian kernel.
        /// </summary>
        /// <returns>
        /// Two-dimensional array that represents kernal (mask).
        /// </returns>
        public override double[][] Create()
        {
            double[][] meshGridX = CreateMeshGrid(MeshType.X);
            double[][] meshGridY = CreateMeshGrid(MeshType.Y);
            double[][] gaussianKernel = CalculateKernelElements(meshGridX, meshGridY);

            return gaussianKernel;
        }

        private double[][] CalculateKernelElements(double[][] meshX, double[][] meshY)
        {
            double[][] gaussianKernel = InitializeJaggedArray(M, N);
            double sigma2 = 2 * Sigma * Sigma;
            double a = 1.0 / (Math.PI * sigma2);
            double sum = 0;

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    gaussianKernel[m][n] = a * Math.Exp(-(meshX[m][n] * meshX[m][n] + meshY[m][n] * meshY[m][n]) / sigma2);
                    sum += gaussianKernel[m][n];
                }
            }

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    gaussianKernel[m][n] /= sum;
                }
            }

            return gaussianKernel;
        }
    }
}
