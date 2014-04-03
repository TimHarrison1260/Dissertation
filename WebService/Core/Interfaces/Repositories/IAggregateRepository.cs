using System.Collections.Generic;
using System.Linq;
using Core.Model;

namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface <c>IAggregateRepository</c> defines the contract
    /// for the AggregateRepository
    /// </summary>
    /// <remarks>
    /// It requires both read and write operations at this implementation so
    /// implements the <see cref="IBaseRepository{T}"/> and 
    /// <see cref="IUpdateRepository{T}"/> interfaces
    /// </remarks>
    public interface IAggregateRepository : IBaseRepository<Aggregate>, IUpdateRepository<Aggregate>
    {
        #region IAggregateRepository specific methods

        /// <summary>
        /// Gets all Aggregates that contain the search criteria
        /// </summary>
        /// <param name="criteria">search criteria</param>
        /// <returns>Collection of Aggregates</returns>
        IEnumerable<Aggregate> Search(string criteria);

        /// <summary>
        /// Gets all Aggregates that have AggregateData of the type specified
        /// </summary>
        /// <param name="dataType">The DataType</param>
        /// <returns>Collection of Aggregates</returns>
        IQueryable<Aggregate> GetAllWithDataType(DataTypeEnum dataType);

        /// <summary>
        /// Gets the Aggregate for the specified Id, with only the specified
        /// datatype included
        /// </summary>
        /// <param name="id">The specified Id</param>
        /// <param name="dataType">The specified DataType</param>
        /// <returns>An Aggregate</returns>
        Aggregate Get(int id, DataTypeEnum dataType);

        #endregion  

    }   
}
