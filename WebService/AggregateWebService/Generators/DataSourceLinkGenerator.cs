using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Models;

namespace AggregateWebService.Generators
{
    /// <summary>
    /// Class <c>DatasourceLinkGenerator</c> has the responsibility
    /// to generate the Hypermedia Links for the aggregate Data Source
    /// being returned through the web service.
    /// </summary>
    public class DataSourceLinkGenerator : IDatasourceLinkGenerator
    {
        /// <summary>
        /// Generates the Hypermedia Links for each data source, when a collection 
        /// of data sources are being returned
        /// </summary>
        /// <param name="dataSource">The data source for which the links are generated</param>
        /// <returns>links</returns>
        public IEnumerable<Link> GenerateCollectionLinks(Core.Model.DataSource dataSource)
        {
            var links = new List<Link>
            {
                new Link
                {
                    Rel = "datasource",
                    Href = "/api/datasource/" + dataSource.Id,
                    Title = string.Empty,
                    Type = "method=\"GET\""
                }
            };
            return links;
        }

        /// <summary>
        /// Generates the Data source links when a specific data source
        /// is being returned
        /// </summary>
        /// <param name="dataSource">the data source for which the links are generated</param>
        /// <returns>Collectino of Links</returns>
        public IEnumerable<Link> GenerateItemLinks(Core.Model.DataSource dataSource)
        {
            //  Add links if the datasource needs and import to update the aggregated data.
            var links = new List<Link>();
            if (dataSource.RequiresImport())
            {
                links.Add(new Link
                {
                    Rel = "import",
                    Href = "/api/import/" + dataSource.Id,
                    Title = string.Empty,
                    Type = "method=\"PUT\""
                });
                links.Add(new Link
                {
                    Rel = "import",
                    Href = "/api/datasource/" + dataSource.Id + "/import/",
                    Title = string.Empty,
                    Type = "method=\"PUT\""
                });
            }
            //  Add a link back to the DataSources
            links.Add(new Link()
            {
                Href = "/api/windfarm/",
                Rel = "windfarms",
                Title = string.Empty,
                Type = "method=\"GET\""
            });
            //  Add one linking back to the DataTypes
            links.Add(new Link()
            {
                Href = "/api/datatype/",
                Rel = "datatypes",
                Title = string.Empty,
                Type = "method=\"GET\""
            });
            return links;
        }
    }
}