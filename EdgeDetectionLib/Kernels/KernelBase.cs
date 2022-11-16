using System;


namespace EdgeDetectionLib.Kernels
{
    /// <summary>
    /// Base abstract class for kernalr containing basic alghoritms.
    /// </summary>
    public abstract class KernelBase : IKernel
    {
        /// <inheritdoc />
        public int M { get; init; }

        /// <inheritdoc />
        public int N { get; init; }

        /// <summary>
        /// Base constructor initializing kernel.
        /// </summary>
        /// <param name="M"> Number of rows. </param>
        /// <param name="N"> Number of columns. </param>
        public KernelBase(int M, int N)
        {
            this.M = M; this.N = N;
        }

        /// <summary>
        /// Base constructor initializing kernel.
        /// </summary>
        /// <param name="MxN"> Number of rows and columns. </param>
        public KernelBase(int MxN)
        {
            M = MxN; N = MxN;
        }

        /// <summary>
        /// Base constructor initializing kernel.
        /// </summary>
        public KernelBase() { }

        /// <inheritdoc />
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
