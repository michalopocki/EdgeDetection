using System;

namespace EdgeDetectionLib.Kernels
{
    /// <summary>
    /// Class that creates Laplacian of Guassian kernel.
    /// </summary>
    public class LaplacianOfGaussianKernel : KernelBase
    {
        ///<summary> Gaussian standard deviation of Laplacian of Gaussian kernel.</summary>
        public double Sigma { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaplacianOfGaussianKernel"/> class.
        /// </summary>
        /// <param name="M"> Number of rows. </param>
        /// <param name="N"> Number of columns. </param>
        /// <param name="sigma"> Gaussian standard deviation of laplacian of gaussian kernel. </param>
        public LaplacianOfGaussianKernel(int M, int N, double sigma) : base(M, N)
        {
            Sigma = sigma;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaplacianOfGaussianKernel"/> class.
        /// </summary>
        /// <param name="MxN"> Number of rows and columns. </param>
        /// <param name="sigma"> Gaussian standard deviation of laplacian of gaussian kernel. </param>
        public LaplacianOfGaussianKernel(int MxN, double sigma) : base(MxN)
        {
            Sigma = sigma;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaplacianOfGaussianKernel"/> class.
        /// </summary>
        /// <param name="sigma"> Gaussian standard deviation of laplacian of gaussian kernel.</param>
        public LaplacianOfGaussianKernel(double sigma)
        {
            Sigma = sigma;
            M = N = (int)Math.Ceiling(sigma) * 5;
        }

        /// <summary>
        /// Creates Laplacian of Gaussian kernel.
        /// </summary>
        /// <returns>
        /// Two-dimensional array that represents kernal (mask).
        /// </returns>
        public override double[][] Create()
        {
            double[][] meshGridX = CreateMeshGrid(MeshType.X);
            double[][] meshGridY = CreateMeshGrid(MeshType.Y);

            double[][] A = CalculateMatrixA(meshGridX, meshGridY);
            double[][] B = CalculateMatrixB(meshGridX, meshGridY);
            double c = 1.0;
            double[][] LoGKernel = CalculateKernelElements(A, B, c);

            return LoGKernel;
        }

        private double[][] CalculateKernelElements(double[][] A, double[][] B, double c)
        {
            double[][] LoG = InitializeJaggedArray(M, N);
            double sum = 0;
            double mean2;

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    LoG[m][n] = c * A[m][n] * B[m][n];
                    sum += LoG[m][n];
                }
            }
            mean2 = sum / (M * N);

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    LoG[m][n] -= mean2;
                }
            }

            return LoG;
        }

        private double[][] CalculateMatrixA(double[][] meshX, double[][] meshY)
        {
            double[][] A = InitializeJaggedArray(M, N);
            double sigma2 = Sigma * Sigma;

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    A[m][n] = (meshX[m][n] * meshX[m][n] + meshY[m][n] * meshY[m][n] - sigma2) / (sigma2 * sigma2);
                }
            }
            return A;
        }

        private double[][] CalculateMatrixB(double[][] meshX, double[][] meshY)
        {
            double[][] B = InitializeJaggedArray(M, N);
            double sigma2 = Sigma * Sigma;
            double sum = 0;

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    B[m][n] = Math.Exp(-(meshX[m][n] * meshX[m][n] + meshY[m][n] * meshY[m][n]) / (2 * sigma2));
                    sum += B[m][n];
                }
            }
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    B[m][n] /= sum;
                }
            }
            return B;
        }
    }
}
