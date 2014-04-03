using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using AggregateWebService.Models;

namespace AggregateWebService.Mappers
{
    /// <summary>
    /// Class <c>DataSourceMapper</c> is responsible for 
    /// mapping the data source information from the dataservice
    /// to the UI DataSources class
    /// </summary>
    public class DataSourcesMapper : IDatasourceMapper
    {
        /// <summary>
        /// Responsibile for mapping a collection of Windfarm Data sources
        /// to the set of links to be returned by the web service
        /// </summary>
        /// <param name="sources">Collection of Windfarm Data sources</param>
        /// <param name="generator">The instance of the Link Generator</param>
        /// <returns>Collection of Links to the individaul aggregate data sources</returns>
        public IEnumerable<DataSourceInfo> MapSourcesToLinks(IEnumerable<Core.Model.DataSource> sources, IDatasourceLinkGenerator generator)
        {
            var results = new List<DataSourceInfo>();
            foreach (var dataSource in sources)
            {
                var uiDataSource = new DataSourceInfo()
                {
                    Title = dataSource.Title,
                    Links = generator.GenerateCollectionLinks(dataSource)
                };
                results.Add(uiDataSource);
            }
            return results;
        }

        /// <summary>
        /// Maps a single aggregate data source to the UI datasource
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="source">the Windfarm Data Source</param>
        /// <param name="generator">The instance of the Link Generator</param>
        /// <returns>UI data source and links</returns>
        public DataSource MapSource(Core.Model.DataSource source, IDatasourceLinkGenerator generator)
        {
            if (source == null)
                return new DataSource();

            var datasource = new DataSource()
            {
                Id = source.Id,
                Title = source.Title,
                Description = source.Description,
                Copyright = source.CopyRight,
                Path = source.AccessPath,
                LastUpdateed = source.LastImported,
                Links = generator.GenerateItemLinks(source)
            };
            return datasource;
        }
    }
}