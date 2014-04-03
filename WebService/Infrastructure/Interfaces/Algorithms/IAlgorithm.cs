
namespace Infrastructure.Interfaces.Algorithms
{
    /// <summary>
    /// Interface <c>IAlgorithm</c> provides the contract
    /// for the individual String Matching Alrogithms.
    /// </summary>
    /// <typeparam name="T">The return type from the algorithm</typeparam>
    public interface IAlgorithm<T>
    {
        /// <summary>
        /// Matches two strings using the pre-processor and String Similarity
        /// algorithms and returns a type of T, that indicates a match or no-match.
        /// </summary>
        /// <param name="s1">String 1</param>
        /// <param name="s2">String 2</param>
        /// <returns>Return value indicating a Match or No-Match</returns>
        T Match(string s1, string s2);
    }
}
