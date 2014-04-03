using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces.Repositories;
using Core.Model;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Class <c>IDataTypeRepository</c> is responsible for
    /// managing the access to the Datatypes used by the 
    /// aggregate domain model.  
    /// </summary>
    /// <remarks>
    /// The Datatype is currently an enumeration, and it is complete
    /// overkill to service the information this way, but it should
    /// really be a class in the model, but it's too late to refactor
    /// it at the moment.  Providing access via this repository will
    /// simplify the refactoring in the future.
    /// </remarks>
    public class DataTypeRepository : IDataTypeRepository
    {
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Ctor: Accepts the UnitOfWork, injected through the IOC.
        /// </summary>
        /// <param name="unitOfWork">Db Context instance</param>
        public DataTypeRepository(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "Invalid Unit of work specified for DataRepository");
            _uow = unitOfWork;
        }

        /// <summary>
        /// Gets all available DataTypes, represented as string
        /// </summary>
        /// <returns>Collection of all DataTypes, represented as strings</returns>
        public IQueryable<string> GetAll()
        {
            //  Select Enum only if data actually exists.
            //  (Necessary because DataType is an enumeration - BAD decision, Not refactoring this yet.)
            var results = (from DataTypeEnum type in Enum.GetValues(typeof(Core.Model.DataTypeEnum))
                where _uow.AggregatedData.Any(d => d.DataType == type)
                select Enum.GetName(typeof (DataTypeEnum), type));
                           //.ToList();
            return results.AsQueryable();

            //var results = new List<string>();
            //results.AddRange(Enum.GetNames(typeof(Core.Model.DataTypeEnum)));
            //return results.AsQueryable();
        }

        /// <summary>
        /// Gets the string representation of the DataType for the specified type
        /// </summary>
        /// <param name="id">the Id of the type</param>
        /// <returns>String representation of the Type</returns>
        public string Get(int id)
        {
            var name = Enum.GetName(typeof (DataTypeEnum), id);
            return name;
        }

        /// <summary>
        /// Gets all dataTypes in Name sequence
        /// </summary>
        /// <returns>Collection of all DataTypes, represented as strings</returns>
        public IEnumerable<string> GetAllSortedByName()
        {
            throw new NotImplementedException();
        }
    }
}
