using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionLib.Kernels
{
    public class GaussianKernel : KernelBase
    {
        public double Sigma { get; set; }

        public GaussianKernel(int M, int N, double sigma) : base(M, N)
        {
            Sigma = sigma;
        }
        public GaussianKernel(int MxN, double sigma) : base(MxN)
        {
            Sigma = sigma;
        }

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
