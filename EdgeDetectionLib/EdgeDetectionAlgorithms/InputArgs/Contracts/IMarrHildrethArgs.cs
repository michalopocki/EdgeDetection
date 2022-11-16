namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    /// <summary>
    /// Interface abstracting the Marr-Hildreth edge detector arguments.
    /// </summary>
    public interface IMarrHildrethArgs : IEdgeDetectorArgs
    {
        /// <summary>Size of Laplacian of Gaussian kernel.</summary>
        int KernelSize { get; set; }

        /// <summary> Gaussian standard deviation of Laplacian of Gaussian kernel</summary>
        double Sigma { get; set; }
    }
}