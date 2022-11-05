namespace EdgeDetectionLib.EdgeDetectionAlgorithms.InputArgs.Contracts
{
    public interface ILaplacianArgs : IGradientArgs
    {
        double Alpha { get; set; }
    }
}