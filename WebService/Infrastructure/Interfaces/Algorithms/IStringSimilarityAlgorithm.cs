
namespace Infrastructure.Interfaces.Algorithms
{
    /// <summary>
    /// Interface <c>IPercentageMatchAlgorithm</c> allows us to
    /// inject the appropriate Percentage and Exact Match type algorithm.
    /// </summary>
    public interface IStringSimilarityAlgorithm : IAlgorithm<double>
    {
    }
}
