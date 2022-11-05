namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    public interface IMarrHildrethArgs : IEdgeDetectorArgs
    {
        int KernelSize { get; set; }
        double Sigma { get; set; }
    }
}