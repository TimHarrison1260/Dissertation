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
    public interface IDatasourceMapper
    {
        /// <summary>
        /// Responsibile for mapping a collection of Windfarm Data sources
        /// to the set of links to be returned by the web service
        /// </summary>
        /// <param name="source">Collection of Windfarm Data sources</param>
        /// <param name="generator">Generator class for generating the hyermedia links</param>
        /// <returns>Collection of Links to the individaul aggregate data sources</returns>
        IEnumerable<DataSourceInfo> MapSourcesToLinks(IEnumerable<Core.Model.DataSource> source, IDatasourceLinkGenerator generator);

        /// <summary>
        /// Maps a single aggregate data source to the UI datasource
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="source">the Windfarm Data Source</param>
        /// <param name="generator">Generator class for generating the hyermedia links</param>
        /// <returns>UI data source and links</returns>
        DataSource MapSource(Core.Model.DataSource source, IDatasourceLinkGenerator generator);
    }
}
