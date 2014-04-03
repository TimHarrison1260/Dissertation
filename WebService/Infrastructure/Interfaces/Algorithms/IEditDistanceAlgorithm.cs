
namespace Infrastructure.Interfaces.Algorithms
{
    /// <summary>
    /// Interface <c>IEditDistanceAlgorithm</c> allows us to
    /// inject the appropriate Edit Distance type algorithm.
    /// </summary>
    public interface IEditDistanceAlgorithm : IAlgorithm<int>
    {
    }
}
