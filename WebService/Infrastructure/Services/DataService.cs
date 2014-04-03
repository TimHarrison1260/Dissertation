using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Model;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Services
{
    /// <summary>
    /// Class <C>DataService</C> provides the implementation of the
    /// IDataService interface and provides the data to support the
    /// requrests from the Service layer (Web Api)
    /// </summary>
    public class DataService : IDataService
    {
        private readonly IAggregateRepository _aggregateRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IDataTypeRepository _dataTypeRepository;
        private readonly IImportServiceResolver _importServiceResolver;
        private readonly IDataSourceResolver _dataSourceResolver;

        /// <summary>
        /// Ctor: Accepts instances via constructor injection
        /// </summary>
        /// <param name="aggregateRepository">Instance of the AggregateRepository</param>
        /// <param name="dataSourceRepository">Instance of the DataSourceRepository</param>
        /// <param name="dataTypeRepository">Instance of the DataType Repository</param>
        /// <param name="importServiceResolver">Instance of the ImportServiceResolver</param>
        /// <param name="dataSourceResolver">Instance of the DataSourceResolver</param>
        public DataService(IAggregateRepository aggregateRepository, IDataSourceRepository dataSourceRepository, IDataTypeRepository dataTypeRepository, IImportServiceResolver importServiceResolver , IDataSourceResolver dataSourceResolver)
        {
            if (aggregateRepository == null)
                throw new ArgumentNullException("aggregateRepository", "No valid AggregateRepository supplied to DataService.");
            if (dataSourceRepository == null)
                throw new ArgumentNullException("dataSourceRepository", "No valid DataSourceRepository supplied to DataService.");
            if (dataTypeRepository == null)
                throw new ArgumentNullException("dataTypeRepository", "No valid DataTypeRepository supplied to DataService.");
            if (importServiceResolver == null)
                throw new ArgumentNullException("importServiceResolver", "No valid ImportServiceResolver supplied to DataService.");
            if (dataSourceResolver == null)
                throw new ArgumentNullException("dataSourceResolver", "No valid DataSourceResolver supplied to DataService.");
            _aggregateRepository = aggregateRepository;
            _dataSourceRepository = dataSourceRepository;
            _dataTypeRepository = dataTypeRepository;
            _importServiceResolver = importServiceResolver;
            _dataSourceResolver = dataSourceResolver;
        }


        /// <summary>
        /// Get all datasource defined to the aggregation.
        /// </summary>
        /// <returns>A collection of the data sources</returns>
        public IEnumerable<DataSource> GeDataSources()
        {
            return _dataSourceRepository.GetAll().AsEnumerable();
        }

        /// <summary>
        /// Get the datasource specified
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <returns>The datasource instance</returns>
        public DataSource GetDataSource(int id)
        {
            return _dataSourceRepository.Get(id);
        }

        /// <summary>
        /// Triggers the import service for the specified data source
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <param name="source">Optionally contains the source to be imported</param>
        /// <returns>Returns TRUE if the import is successfull, otherwise FALSE</returns>
        public bool ImportDataSource(int id, string source = null)
        {
            //  Get the instance of the datasource component for this datasource
            var importService = _importServiceResolver.Resolve(id);
            if (importService == null)
                return false;

            //  Call the import
            var result = importService.ImportData(id, source);

            return result;
        }

        /// <summary>
        /// Determines if the import requires the source data to be passed in
        /// </summary>
        /// <param name="id">Id of the Data source being imported</param>
        /// <returns>True if source data is required otherwise False</returns>
        public bool ImportRequiresSourceData(int id)
        {
            var source = _dataSourceRepository.Get(id);
            if (source == null)
                return false;
            return source.ImportRequiresSourceData();
        }


        /// <summary>
        /// Gets a string representation of all Datatypes held within the aggregate
        /// </summary>
        /// <returns>Collection of strings representing data types</returns>
        public IEnumerable<string> GetDataTypes()
        {
            return _dataTypeRepository.GetAll();
        }

        /// <summary>
        /// Gets a collection of all aggregates matching the 
        /// search criteria (an empty string will return ALL
        /// aggregate instances).
        /// </summary>
        /// <returns>Collection of Aggregate classes</returns>
        public IEnumerable<Aggregate> GetAggregates(string criteria)
        {
            IEnumerable<Aggregate> aggregates;
            if (string.IsNullOrEmpty(criteria))
                aggregates = _aggregateRepository.GetAll();                
            else
                aggregates = _aggregateRepository.Search(criteria);
            return aggregates;
        }

        /// <summary>
        /// Gets the specified Aggregate
        /// </summary>
        /// <param name="id">The Id of the Aggregate</param>
        /// <returns>The Aggregate instance</returns>
        public Aggregate GetAggregate(int id)
        {
            var aggregate = _aggregateRepository.Get(id);
            if (aggregate == null) return null;
            
            foreach (var data in aggregate.Data)
            {
                data.Data = ApplyExternalData(data);
            }

            return aggregate;
        }

        /// <summary>
        /// Gets all aggregates that contain the specified data type
        /// </summary>
        /// <param name="type">The Datatype</param>
        /// <returns>Collection of Aggregate classes</returns>
        public IEnumerable<Aggregate> GetAggregatesWithDataType(DataTypeEnum type)
        {
            var aggregates = _aggregateRepository.GetAllWithDataType(type);

            return aggregates;
        }

        /// <summary>
        /// Gets the datatype for the specified aggregate
        /// </summary>
        /// <param name="id">Id of the Aggregate</param>
        /// <param name="type">The Datatype</param>
        /// <returns>Returns the Aggregate, with the specified data type attached</returns>
        public Aggregate GetAggregateWithDataType(int id, DataTypeEnum type)
        {
            var aggregate = _aggregateRepository.Get(id, type);
            if (aggregate == null) return null;

            foreach (var data in aggregate.Data.Where(data => data.DataType == type))
            {
                data.Data = ApplyExternalData(data);
            }

            return aggregate;
        }

        /// <summary>
        /// Returns the data component of the AggregateData, updated if the source
        /// it refers to is external, ie it holds a URL instead of actual data, so
        /// it calls the URL and updates the data with the results of the call.
        /// </summary>
        /// <param name="data">AggregateData to be updated if necessary</param>
        /// <returns>Value of Data element</returns>
        private string ApplyExternalData(AggregateData data)
        {
            var source = _dataSourceRepository.Get(data.DataSourceId);
            //  Do external processing only if the source is valid and its an external source
            //  If the source in invalid, leave things alone, just as if the source is internal
            //  This situation shouldn't happen.
            if (source == null || !source.IsDataExternal()) return data.Data ;   // if invalid source or internal, return unaltered

            //  Resolve the instance of the IDataSource module, that accesses this external data source
            var externalSource = _dataSourceResolver.Resolve(data.DataSourceId);
            if (externalSource == null) return data.Data;   //  If invalid datasource, return unaltered

            //  Call into the source
            var externalData = externalSource.Get(data.Data);
            return externalData == null ? "Data currently not available for this resource"
                : externalData.Data.FirstOrDefault(d => d.DataType == data.DataType).Data;
        }
    }
}
