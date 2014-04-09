using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Models;

namespace AggregateWebService.Interfaces.Mappers
{
    /// <summary>
    /// Interface <c>IDataSourceMapper</c> is provides the mapping
    /// of the data source information from the dataservice
    /// to the UI DataSources class
    /// </summary>
    public interface IImportMapper
    {
        /// <summary>
        /// Maps a single aggregate data source to the UI datasource
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="source">the Windfarm Data Source</param>
        /// <param name="generator">Generator class for generating the hyermedia links</param>
        /// <returns>UI data source and links</returns>
        ImportInfo MapSource(int id, bool result, IImportLinkGenerator generator);
    }
}
