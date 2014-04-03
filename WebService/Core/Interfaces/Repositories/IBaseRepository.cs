using System.Collections.Generic;
using System.Linq;

namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface <c>IBaseRepository</c> is a generic base
    /// interface designed to provide a Facade for the
    /// Read operations requireed by an implementing
    /// repopsitory class
    /// </summary>
    /// <typeparam name="T">The concrete Type the Repository is using.</typeparam>
    public interface IBaseRepository<T> where T: class 
    {
        /// <summary>
        /// Gets all instances of type T
        /// </summary>
        /// <returns>A collection of T</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets the specific instance of type T
        /// </summary>
        /// <param name="id">The specific Id of type T</param>
        /// <returns>The instance of T</returns>
        T Get(int id);

        /// <summary>
        /// Gets all Aggregates, including the data, sorted into
        /// Name sequence as defined by the <c>AggregateNameComparer</c>.
        /// </summary>
        /// <returns>A collection of all Aggregates</returns>
        IEnumerable<T> GetAllSortedByName();

    }
}
