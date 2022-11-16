namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    /// <summary>
    /// Interface abstracting the Canny edge detector arguments.
    /// </summary>
    public interface ICannyArgs : IEdgeDetectorArgs
    {
        /// <summary>
        /// Specifies if apply smoothing filtration utilising Gaussian blur.
        /// </summary>
        bool Prefiltration { get; set; }

        /// <summary>
        /// Size of Gaussian kernel.
        /// </summary>
        int KernelSize { get; set; }

        /// <summary> Gaussian standard deviation of Gaussian kernel</summary>
        double Sigma { get; set; }

        /// <summary>
        /// Higher threshold value in range 0-255. Sholud be greater than TLow (low threshold).
        /// </summary>
        int THigh { get; set; }

        /// <summary>
        /// Lower threshold value in range 0-255. Sholud be lower than THigh (high threshold).
        /// </summary>
        int TLow { get; set; }
    }
}