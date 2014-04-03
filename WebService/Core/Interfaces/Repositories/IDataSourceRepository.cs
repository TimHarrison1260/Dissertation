using System.Linq;
using Core.Model;

namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface <c>IDataSourceRepository</c> defines the contract
    /// for the DataSourceRepository.  It contains additional methods 
    /// specific to the DataSource Repository
    /// </summary>
    /// <remarks>
    /// It requires only read operations at this implementation so
    /// only implements the <see cref="IBaseRepository{T}"/> interface
    /// </remarks>
    public interface IDataSourceRepository : IBaseRepository<DataSource>
    {
        /// <summary>
        /// Validates whether the supplied Id represents a valid datasource
        /// </summary>
        /// <param name="sourceId">the Id to be validated</param>
        /// <returns>TRUE if the id represents a datasource otherwise FALSE.</returns>
        bool IsValidDataSource(int sourceId);
    }
}
