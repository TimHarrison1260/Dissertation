using System.Collections.Generic;
using Core.Model;

namespace Infrastructure.Interfaces.Algorithms
{
    /// <summary>
    /// Interface <c>ImatchingService</c> defines the contract for the
    /// overall matching algorithm, used in the data aggregation process.
    /// </summary>
    public interface IMatchingAlgorithm
    {
        int Match(string source, IList<Aggregate> aggregates);
    }
}
