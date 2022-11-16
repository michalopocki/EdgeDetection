namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    /// <summary>
    /// Interface abstracting the gradient edge detector arguments.
    /// </summary>
    public interface IGradientArgs : IEdgeDetectorArgs
    {
        /// <summary>
        /// Specifies if apply thresholding.
        /// </summary>
        bool Thresholding { get; set; }

        /// <summary>
        /// Value of threshold in range 0-255.
        /// </summary>
        int Threshold { get; set; }

        /// <summary>
        /// Specifies if apply smoothing filtration utilising Gaussian blur.
        /// </summary>
        bool Prefiltration { get; set; }

        /// <summary>
        /// Size of Gaussian kernel.
        /// </summary>
        int KernelSize { get; set; }

        /// <summary> Gaussian standard deviation of gaussian kernel.</summary>
        double Sigma { get; set; }
    }
}