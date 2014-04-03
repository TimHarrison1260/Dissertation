using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using AggregateWebService.Models;

namespace AggregateWebService.Mappers
{
    /// <summary>
    /// Interface <c>IDataTypeMapper</c> is provides the mapping
    /// of the data type information from the dataservice
    /// to the UI DataType class
    /// </summary>
    public class DataTypeMapper : IDataTypeMapper
    {
        /// <summary>
        /// Responsibile for mapping a collection of Windfarm Data types
        /// to the set of links to be returned by the web service
        /// </summary>
        /// <param name="dataTypes">Collection of Windfarm DataTypes</param>
        /// <returns>Collection of available aggregate data types, including links</returns>
        public IEnumerable<DataType> MapTypesToLinks(IEnumerable<string> dataTypes, IDataTypeLinkGenerator generator)
        {
            var types = new List<DataType>();
            foreach (var type in dataTypes)
            {
                var uiType = new DataType()
                {
                    Type = type,
                    Links = generator.GenerateCollectionLinks(type)
                };
                types.Add(uiType);
            }
            return types;
        }
    }
}