using System;


namespace EdgeDetectionLib.Kernels
{
    public abstract class KernelBase : IKernel
    {
        public int M { get; init; }
        public int N { get; init; }

        public KernelBase(int M, int N)
        {
            this.M = M; this.N = N;
        }

        public KernelBase(int MxN)
        {
            M = MxN; N = MxN;
        }
        public KernelBase() { }

        public abstract double[][] Create();

        protected double[][] CreateMeshGrid(MeshType type)
        {
            int Mval = (M - 1) / 2;
            int Nval = (N - 1) / 2;
            double[][] meshGrid = InitializeJaggedArray(M, N);

            for (int i = 0, m = -Mval; i < M; i++, m++)
            {
                for (int j = 0, n = -Nval; j < N; j++, n++)
                {
                    meshGrid[i][j] = type == MeshType.X ? n : m;
                }
            }
            return meshGrid;
        }

        protected static double[][] InitializeJaggedArray(int M, int N)
        {
            double[][] jaggedArray = new double[M][];

            for (int n = 0; n < M; n++)
            {
                jaggedArray[n] = new double[N];
            }
            return jaggedArray;
        }
    }
}
