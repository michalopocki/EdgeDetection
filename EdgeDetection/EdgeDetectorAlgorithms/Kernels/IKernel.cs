using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeDetectionApp.EdgeDetectorAlgorithms.Kernels
{
    public interface IKernel
    {
        public int M { get; }
        public int N { get; }
        double[][] Create();

    }
}
