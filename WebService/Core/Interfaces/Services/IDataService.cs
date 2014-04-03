using System.Collections.Generic;
using Core.Model;

namespace Core.Interfaces.Services
{
    /// <summary>
    /// Interface <c>IDataService</c> privdes the Facade
    /// for the DataService that applies the application
    /// logic and services the UI with the data from the
    /// aggregation model
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Get all datasource defined to the aggregation.
        /// </summary>
        /// <returns>A collection of the data sources</returns>
        IEnumerable<DataSource> GeDataSources();

        /// <summary>
        /// Get the datasource specified
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <returns>The datasource instance</returns>
        /// 
        DataSource GetDataSource(int id);

        /// <summary>
        /// Triggers the import service for the specified data source
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <param name="source">Optionally contains the data to be imported</param>
        /// <returns>Returns TRUE if the import is successfull, otherwise FALSE</returns>
        bool ImportDataSource(int id, string source = null);

        /// <summary>
        /// Determines if the import requires the source data to be passed in
        /// </summary>
        /// <param name="id">Id of the Data source being imported</param>
        /// <returns>True if source data is required otherwise False</returns>
        bool ImportRequiresSourceData(int id);

        /// <summary>
        /// Gets a string representation of all Datatypes held within the aggregate
        /// </summary>
        /// <returns>Collection of strings representing data types</returns>
        IEnumerable<string> GetDataTypes();

        ///// <summary>
        ///// Gets a string representation of all Statuses held within the aggregate
        ///// </summary>
        ///// <returns>Collection of string representing data types</returns>
        //IEnumerable<string> GetStatuses();

        /// <summary>
        /// Gets a collection of all aggregates matching the 
        /// search criteria (an empty string will return ALL
        /// aggregate instances).
        /// </summary>
        /// <returns>Collection of Aggregate classes</returns>
        IEnumerable<Aggregate> GetAggregates(string criteria);

        /// <summary>
        /// Gets the specified Aggregate
        /// </summary>
        /// <param name="id">The Id of the Aggregate</param>
        /// <returns>The Aggregate instance</returns>
        Aggregate GetAggregate(int id);

        /// <summary>
        /// Gets all aggregates that contain the specified data type
        /// </summary>
        /// <param name="type">The Datatype</param>
        /// <returns>Collection of Aggregate classes</returns>
        IEnumerable<Aggregate> GetAggregatesWithDataType(DataTypeEnum type);

        /// <summary>
        /// Gets the datatype for the specified aggregate
        /// </summary>
        /// <param name="id">Id of the Aggregate</param>
        /// <param name="type">The Datatype</param>
        /// <returns>Returns the Aggregate, with the specified data type attached</returns>
        Aggregate GetAggregateWithDataType(int id, DataTypeEnum type);

    }
}
