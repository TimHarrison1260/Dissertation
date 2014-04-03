using System;
using System.Linq;
using System.Data.Entity;
using Core.Interfaces.Repositories;
using Core.Model;
using Infrastructure.Helpers;

using System.Collections.Generic;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>AggregateRepository</c> is responsible for managing 
    /// access to the <see cref="Aggregate"/> Domain Entity.
    /// </summary>
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Ctor: Accepts the UnitOfWork, injected through the IOC.
        /// </summary>
        /// <param name="unitOfWork">Db Context instance</param>
        public AggregateRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "Invalid Unit of work specified for DataRepository");
            _uow = unitOfWork;
        }


        #region IBaseRepository methods

        /// <summary>
        /// Gets all Aggregates in the model sorted into ascending Name sequence
        /// </summary>
        /// <returns>A collection of all Aggregates</returns>
        public IQueryable<Aggregate> GetAll()
        {
            //  Remove the .Include, NOT required for REST web sevice that
            //  will return only a collection of links, therefore this information
            //  is simply not required at this stage.  It must only be retrieved
            //  If a specific resource is required.
            var results = _uow.Aggregates
                                //.Include(a => a.Data)
                                .OrderBy(n => n.Name);
            return results;
        }

        /// <summary>
        /// Get aggregate for specified Id
        /// </summary>
        /// <param name="id">Aggregate Id</param>
        /// <returns>Aggregate if it exists otherwise NULL</returns>
        public Aggregate Get(int id)
        {
            var result = _uow.Aggregates
                                .Where(a => a.Id == id)
                                .Include(a => a.Data)
                                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Gets all Aggregates, including the data, sorted into
        /// Name sequence as defined by the <see cref="Infrastructure.Helpers.AggregateNameComparer"/>
        /// </summary>
        /// <returns>A collection of all Aggregates</returns>
        /// <remarks>
        /// <para>
        /// This version is implemeted for use primarily with the Import services.
        /// The <c>AggregateNameComparer</c> is used to allow Linq to use a custom
        /// comparer where the StringComparer.Ordinal option is specified.
        /// </para>
        /// <para>
        /// The AsEnumerable() method must be included, before the OrderBy() method
        /// as using the custom comparer is not supported by IQueryable, it throws a 
        /// "Not supported" exception.  
        /// </para>
        /// </remarks>
        public IEnumerable<Aggregate> GetAllSortedByName()
        {
            //  Remove the .Include, NOT required for REST web sevice that
            //  will return only a collection of links, therefore this information
            //  is simply not required at this stage.  It must only be retrieved
            //  If a specific resource is required.
            var results = _uow.Aggregates
                            //.Include(a => a.Data)
                            .AsEnumerable()
                            .OrderBy(n => n.Name, new AggregateNameComparer());
            return results;
        }

        #endregion

        #region IAggregateRepository specific methods


        /// <summary>
        /// Gets all Aggregates that contain the search criteria
        /// </summary>
        /// <param name="criteria">search criteria</param>
        /// <returns>Collection of Aggregates</returns>
        public IEnumerable<Aggregate> Search(string criteria)
        {
            var results = _uow.Aggregates
                .Where(a => a.Name.Contains(criteria))
                .AsEnumerable()
                .OrderBy(n => n.Name, new AggregateNameComparer());
            return results;
        }


        /// <summary>
        /// Gets all Aggregates that have AggregateData of the type specified
        /// </summary>
        /// <param name="dataType">The DataType</param>
        /// <returns>Collection of Aggregates</returns>
        public IQueryable<Aggregate> GetAllWithDataType(DataTypeEnum dataType)
        {
            var aggregatesContainingDataType = _uow.AggregatedData
                .Include(d => d.Aggregate)
                .Where(d => d.DataType == dataType)
                .Select(d => d.Aggregate)
                .Distinct();

            return aggregatesContainingDataType;
        }

        /// <summary>
        /// Gets the Aggregate for the specified Id, and the specified
        /// datatype.
        /// </summary>
        /// <param name="id">The specified Id</param>
        /// <param name="dataType">The specified DataType</param>
        /// <returns>An Aggregate</returns>
        public Aggregate Get(int id, DataTypeEnum dataType)
        {
            var aggregate = _uow.AggregatedData
                .Include(d => d.Aggregate)
                .Where(d => d.DataType == dataType && d.Aggregate.Id == id)
                .Select(d => d.Aggregate).FirstOrDefault();

            return aggregate;
        }


        #endregion

        #region IUpdateRepository methods
        /// <summary>
        /// Adds a new aggregate object to the data store
        /// </summary>
        /// <param name="aggregate">The new Aggregate object</param>
        /// <returns>The assigned Id of the new Aggregate</returns>
        public int Add(Aggregate aggregate)
        {
            _uow.Aggregates.Add(aggregate);
            var newId = _uow.SaveChanges();
            return newId;
        }

        /// <summary>
        /// Updates an existing aggregate
        /// </summary>
        /// <param name="aggregate">The Aggregate containing the aggregate to be applied</param>
        /// <remarks>
        /// The method combines the datatypes on the input aggregate with 
        /// those on the aggregate already in existance.  If the datatype
        /// on the input matches one of those on the aggregate it updates
        /// it, otherwise it treats it as a new datatype for the aggregate
        /// and adds it.
        /// </remarks>
        public void Update(Aggregate aggregate)
        {
            //  Retrieve from Datastore
            var agg = _uow.Aggregates.First(a => a.Id == aggregate.Id);

            //  Apply aggregate to the retrieved 
            //  Apply the aggregate to the references Aggregate object
            agg.LastUpdated = aggregate.LastUpdated;

            //  Combine the datatypes.
            //  Loop each input datatype and update if there is one already
            //  Otherwise, add the new datatype to the aggregate.
            foreach (var inputData in aggregate.Data)
            {
                bool updated = false;

                foreach (var aggregateData in agg.Data)
                {
                    if (inputData.DataType == aggregateData.DataType)
                    {
                        //  Existing AggregateData, update it
                        aggregateData.Data = inputData.Data;
                        aggregateData.LastUpdated = inputData.LastUpdated;
                        //  Set as updated
                        updated = true;
                    }
                }
                if (!updated)
                {
                    //  No update taken place, must be a new data type
                    agg.Data.Add(inputData);
                }
            }
            _uow.SaveChanges();
        }

        #endregion

    }
}
