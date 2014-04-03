using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Models;

namespace AggregateWebService.Interfaces.Mappers
{
    /// <summary>
    /// Interface <c>IDataTypeMapper</c> is provides the mapping
    /// of the data type information from the dataservice
    /// to the UI DataType class
    /// </summary>
    public interface IDataTypeMapper
    {
        /// <summary>
        /// Responsibile for mapping a collection of Windfarm Data types
        /// to the set of links to be returned by the web service
        /// </summary>
        /// <param name="dataTypes">Collection of Windfarm DataTypes</param>
        /// <returns>Collection of Links to the individaul aggregate data types</returns>
        IEnumerable<DataType> MapTypesToLinks(IEnumerable<string> dataTypes, IDataTypeLinkGenerator generator);
    }
}
