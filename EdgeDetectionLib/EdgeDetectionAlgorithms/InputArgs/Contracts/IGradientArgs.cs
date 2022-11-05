namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    public interface IGradientArgs : IEdgeDetectorArgs
    {
        bool Thresholding { get; set; }
        int Threshold { get; set; }
        bool Prefiltration { get; set; }
        int KernelSize { get; set; }
        double Sigma { get; set; }
    }
}