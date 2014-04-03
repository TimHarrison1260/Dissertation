using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Model;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>DataSourceRepository</c> is responsible for providing
    /// access to the underlying database for the DataSources that are
    /// defined to the service.
    /// </summary>
    public class DataSourceRepository : IDataSourceRepository
    {
        private readonly IUnitOfWork _uow;      

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="unitOfWork">Injected Unit of Work</param>
        public DataSourceRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "Invalid Unit of work specified for Data Source Repository");
            _uow = unitOfWork;
        }

        /// <summary>
        /// Gets all instances of datasources defined to the service
        /// </summary>
        /// <returns>Collection of defined datasources</returns>
        public IQueryable<DataSource> GetAll()
        {
            var results = _uow.DataSources;
            return results;
        }

        /// <summary>
        /// Gets a specific datasource for the specified Id
        /// </summary>
        /// <param name="sourceId">The Id of the datasource</param>
        /// <returns>The datasource class</returns>
        public DataSource Get(int sourceId)
        {
            var result = _uow.DataSources.FirstOrDefault(d => d.Id == sourceId);
            return result;
        }

        /// <summary>
        /// Gets a list of DataSources sorted by datasource Name
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataSource> GetAllSortedByName()
        {
            var results = _uow.DataSources.OrderBy(d => d.Title);
            return results.AsEnumerable();
        }

        /// <summary>
        /// Validates whether the supplied Id represents a valid datasource
        /// </summary>
        /// <param name="sourceId">the Id to be validated</param>
        /// <returns>TRUE if the id represents a datasource otherwise FALSE.</returns>
        public bool IsValidDataSource(int sourceId)
        {
            //var resultSelect = _uow.DataSources.Select(d => d.Id == sourceId).ToArray();  //.First();
            //var result = _uow.DataSources.Where(d => d.Id == sourceId).ToArray();  //.First();
            //var resulFirst = _uow.DataSources.FirstOrDefault(d => d.Id == sourceId);  //.First();

            return (_uow.DataSources.FirstOrDefault(d => d.Id == sourceId) != null) ? true : false;

//            var result2 = result.FirstOrDefault();
  //          return resultSelect.FirstOrDefault();
        }

    }
}
