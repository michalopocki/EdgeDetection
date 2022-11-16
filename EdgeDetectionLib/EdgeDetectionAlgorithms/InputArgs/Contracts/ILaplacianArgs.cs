namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    /// <summary>
    /// Interface abstracting the Laplacian edge detector arguments.
    /// </summary>
    public interface ILaplacianArgs : IGradientArgs
    {
        /// <summary>
        /// Factor of Laplacian kernel in range [0; 1].
        /// </summary>
        double Alpha { get; set; }
    }
}