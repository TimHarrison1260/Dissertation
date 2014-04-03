using System.Collections.Generic;
using Infrastructure.ServiceModel;

namespace Infrastructure.Interfaces.Data
{
    /// <summary>
    /// Interface <c>IDataSource</c> defines the contract
    /// required for each specific datasource component to
    /// implement so that it can be used by the ImportService.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Loads the from the source
        /// </summary>
        bool LoadSource(string source);

        /// <summary>
        /// Gets all the aggregate names from the data source
        /// </summary>
        /// <returns>A collection of aggregate names</returns>
        IEnumerable<string> GetAllNames();
        
        /// <summary>
        /// Gets the instances of all source records
        /// </summary>
        /// <returns>Returns a collection of ImportAggregate objects</returns>
        IEnumerable<ImportAggregate> GetAll();

        /// <summary>
        /// Get the Aggregate for the specified Id
        /// </summary>
        /// <param name="url">The Url of the aggregate requested</param>
        /// <returns>The requested Aggregate</returns>
        /// <remarks>
        /// <para>
        /// This method is for use when the data held in the internal
        /// data store is a URL to the external source, which then 
        /// has to be called to get the actual data.
        /// </para>
        /// </remarks>
        ImportAggregate Get(string url);
        
        /// <summary>
        /// Gets the alternate source records containing the
        /// pertinent name.
        /// </summary>
        /// <param name="name">The pertinent name</param>
        /// <returns>Collection of all possible source records</returns>
        IEnumerable<string> GetAltIndices(string name);
    }
}
