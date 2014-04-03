using System;
using System.Linq;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Model;
using Infrastructure.Interfaces.Algorithms;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Mappers;
using Infrastructure.ServiceModel;

namespace Infrastructure.Services
{
    /// <summary>
    /// Class <c>AbstractImportService</c> implements the <see cref="IImportService"/>
    /// contract.  It imports the data from the supplied data source and
    /// updates the <see cref="Infrastructure.Data.AggregateContext"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a general routine that defines the process and is implemented
    /// as an abstract class.  This ensures the the import services for each 
    /// specific datasource are implemented in such a way as to follow a string
    /// pattern.  The resources used by the import service are injected using
    /// the relevant interfaces, with the IDataSource being the only one 
    /// implemented for each source, the rest are common to all.
    /// </para>
    /// <list type="table">
    /// <listheader>
    /// <term>Interface</term>
    /// <description>General description of Interface</description>
    /// </listheader>
    /// <item>
    /// <term>IDataSource</term>
    /// <description>Defines the data source to be imported into the service.  This is unique
    /// to each datasource being aggregated within the overall service.</description>
    /// </item>
    /// <item>
    /// <term>IAggregateRepository(Aggregate)</term>
    /// <description>Defines the repository used to access the aggregates already aggregated 
    /// within the overall service.</description>
    /// </item>
    /// <item>
    /// <term>IDataSourceRepository</term>
    /// <description>Controls access to the Datasources already defined to the overall 
    /// service.</description>
    /// </item>
    /// <item>
    /// <term>IMatchingAlgorithm</term>
    /// <description>Defines the matching algorithm used to match the aggregates
    /// being imported to those already defined within the overall service</description>
    /// </item>
    /// <item>
    /// <term>IMapper(ImportAggregate, Aggregate)</term>
    /// <description>Defines the mapper that converts the <see cref="ImportAggregate"/> class to
    /// the <see cref="Core.Model.Aggregate"/> class.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class AbstractImportService : IImportService
    {
        private readonly IDataSource _datasource;                           //  Import data
        private readonly IAggregateRepository _repository;       //  Repository to Aggregate model
        private readonly IDataSourceRepository _dataSourceRepository;       //  Repository to DataSource model
        private readonly IMatchingAlgorithm _algorithm;                     //  The string matching algorithm
        private readonly IMapper<ImportAggregate,Aggregate> _mapper;        //  Mapper class for importing data to Aggregate model

        protected AbstractImportService(IAggregateRepository repository, 
                                        IDataSourceRepository dataSourceRepository, 
                                        IDataSource dataSource, 
                                        IMatchingAlgorithm matchingAlgorithm,
                                        IMapper<ImportAggregate, Aggregate> mapper)
        {
            if (repository == null)
                throw new ArgumentNullException("repository", "Invalid repository specified for import Service");
            if (dataSourceRepository == null)
                throw new ArgumentNullException("dataSourceRepository", "Invalid Datasource repository specified for import Service");
            if (dataSource == null)
                throw new ArgumentNullException("dataSource","Invalid datasource supplied to import Service");
            if (matchingAlgorithm == null)
                throw new ArgumentNullException("matchingAlgorithm", "Invalid matching algorithm supplied to import Service");
            if (mapper == null)
                throw new ArgumentNullException("mapper", "No valid mapper supplied to import service");

            _repository = repository;
            _dataSourceRepository = dataSourceRepository;
            _datasource = dataSource;
            _algorithm = matchingAlgorithm;
            _mapper = mapper;
            //_mapper = new ImportAggregateToAggregateMapper();
        }

        /// <summary>
        /// Import the SNH data from the SNH Data source
        /// </summary>
        /// <param name="sourceId">Id of the SNH Data source</param>
        /// <param name="source">The source data. optional for sources that use a configured datasource</param>
        /// <returns>True if successfully imported otherwise False</returns>
        public bool ImportData(int sourceId, string source = null)
        {
            //  Validate sourceId
            if (!_dataSourceRepository.IsValidDataSource(sourceId))
                return false;

            //  If source passed in, use it.
            if (!string.IsNullOrEmpty(source)) 
                return ImportDataSource(sourceId, source);

            //  If source is null, get sourceURL from Source configuration
            var dataSource = _dataSourceRepository.Get(sourceId);
            return !string.IsNullOrEmpty(dataSource.AccessPath) && ImportDataSource(sourceId, dataSource.AccessPath);
        }

        /// <summary>
        /// Import the Data from the data source, the Object
        /// should be a string here.
        /// </summary>
        /// <param name="sourceId">Id of the Data source</param>
        /// <param name="source">The data source</param>
        /// <returns>True if successfully imported otherwise False</returns>
        public bool ImportData(int sourceId, object source)
        {
            if (source == null || source.GetType() != typeof(string))
                return false;
            return ImportData(sourceId, (string)source);
        }

        /// <summary>
        /// Import the Data source.
        /// </summary>
        /// <param name="sourceId">The ID of the SNH Datasource</param>
        /// <param name="source">The data source, uploaded to the Web Service</param>
        /// <returns>True if the data is successfully imported otherwise FALSE</returns>
        private bool ImportDataSource(int sourceId, string source)
        {

            //  Load the datasource, return if nothing loaded.
            if (!_datasource.LoadSource(source))
                return false;

            //  Get the timestamp for marking the updated or added records.
            //  TODO: Refactor this to retrieve the import date from the dataset.  Update the ISNHDataSource to include GetFileCreationDate method and inject Repository for DataSouces.
            var importDate = DateTime.Now;      //  This should really be obtained from the "issue date" from the source for SNH.

            //  get the aggregates we are going to match with.
            var aggregates = _repository.GetAllSortedByName().ToList(); //  Iterate the collection, only once

            //  Get the import data and aggregated it. 
            var importAggregates = _datasource.GetAll();
            foreach (var importAggregate in importAggregates)
            {
                importAggregate.ImportDate = importDate;
                importAggregate.SourceId = sourceId;
                //  Match each input against the aggregate store, update if found, add if not.
                //  Matching against an SNH record will achieve an exact match with the first algorithm, so will be fairly quick
                //  Matching against a record from another source will perform the aggregation process fully.
                //  Uodating the aggregate will involve either adding the data type or updating the data type in the Aggregate data table
                //  Updating will update the last imported date on the aggregate record.  Additionally, the relevant data type in the data
                //      store table will also have the same last updated date.

                //  Match against each of the existing names, >= 0 => Match
                var matchingId = _algorithm.Match(importAggregate.Identifier, aggregates);

                //  Map the importAggregate class to a Domain Model Aggregate class
                var aggregate = _mapper.Map(importAggregate);

                if(matchingId >= 0)
                {
                    //  Apply changes to the matched aggregate
                    aggregate.Id = matchingId;
                    _repository.Update(aggregate);
                }
                else
                {
                    //  Create a new aggregate
                    var id = _repository.Add(aggregate);
                }

            }
            return true;
        }
    }
}
