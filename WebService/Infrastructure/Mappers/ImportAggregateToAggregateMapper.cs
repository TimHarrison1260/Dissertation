using System;
using System.Collections.ObjectModel;
using Core.Model;
using Infrastructure.Interfaces.Mappers;
using Infrastructure.ServiceModel;

namespace Infrastructure.Mappers
{
    /// <summary>
    /// Class <c>ImportAggregateToAggregateMapper</c> provides the functionality
    /// to map ImportAggregate class to Aggregate class.
    /// </summary>
    public class ImportAggregateToAggregateMapper : IMapper<ImportAggregate, Aggregate>
    {
        /// <summary>
        /// Maps the contents of the ImportAggregate class to a new instance of 
        /// the Aggregate class.
        /// </summary>
        /// <param name="importAggregate">The class to be mapped</param>
        /// <returns>New Aggregate instance</returns>
        public Aggregate Map(ImportAggregate importAggregate)
        {
            if (importAggregate == null)
            {
                return new Aggregate()
                {
                    Name = string.Empty,
                    LastUpdated = new DateTime(),
                    Data = new Collection<AggregateData>()
                };
            }

            //  TODO: refactor this to use the AggregateFactory to create the instance
            var newAggregate = new Aggregate
                {
                    Name = importAggregate.Identifier ?? string.Empty, // if null, empty string
                    LastUpdated = importAggregate.ImportDate,
                    Data = new Collection<AggregateData>()
                };
            
            //  Add the data to the aggregate
            if (importAggregate.Data != null && importAggregate.Data.Count > 0)
            {
                foreach (var importData in importAggregate.Data)
                {
                    var newAggregatedata = new AggregateData
                    {
                        //  TODO: Refactor to use converter from XML to JSON
                        Data = importData.Data,
                        DataType = importData.DataType,
                        Name = importAggregate.Identifier,
                        //  TODO: Refactor to use the datasource id from the import service
                        DataSourceId = importAggregate.SourceId,
                        LastUpdated = importAggregate.ImportDate
                    };
                    newAggregate.Data.Add(newAggregatedata);
                }
            }
            return newAggregate;
        }
    }
}
