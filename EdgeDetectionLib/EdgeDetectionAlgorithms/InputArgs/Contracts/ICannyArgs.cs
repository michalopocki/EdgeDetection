namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    public interface ICannyArgs : IEdgeDetectorArgs
    {
        bool Prefiltration { get; set; }
        int KernelSize { get; set; }
        double Sigma { get; set; }
        int THigh { get; set; }
        int TLow { get; set; }
    }
}