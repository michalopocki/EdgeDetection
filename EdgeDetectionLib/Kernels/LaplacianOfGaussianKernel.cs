using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Kernels
{
    public class LaplacianOfGaussianKernel : KernelBase
    {
        public double Sigma { get; set; }
        public LaplacianOfGaussianKernel(int M, int N, double sigma) : base(M, N)
        {
            Sigma = sigma;
        }
        public LaplacianOfGaussianKernel(int MxN, double sigma) : base(MxN)
        {
            Sigma = sigma;
        }
        public LaplacianOfGaussianKernel(double sigma)
        {
            Sigma = sigma;
            M = N = (int)Math.Ceiling(sigma) * 5;
        }
        public override double[][] Create()
        {
            double[][] meshGridX = CreateMeshGrid(MeshType.X);
            double[][] meshGridY = CreateMeshGrid(MeshType.Y);

            double[][] A = CalculateMatrixA(meshGridX, meshGridY);
            double[][] B = CalculateMatrixB(meshGridX, meshGridY);
            double c = 1d;
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
