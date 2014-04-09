using System;
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
    public class ImportLinkGenerator : IImportLinkGenerator
    {
        /// <summary>
        /// Generates the Hypermedia Links for each data source, when a collection 
        /// of data sources are being returned
        /// </summary>
        /// <param name="dataSourceId">The data source for which the links are generated</param>
        /// <returns>links</returns>
        public IEnumerable<Link> GenerateCollectionLinks(int dataSourceId)
        {
            throw new NotImplementedException();
            //var links = new List<Link>
            //{
            //    new Link
            //    {
            //        Rel = "datasource",
            //        Href = "/api/datasource/" + dataSource.Id,
            //        Title = string.Empty,
            //        Type = "method=\"GET\""
            //    }
            //};
            //return links;
        }

        /// <summary>
        /// Generates the Data source links when a specific data source
        /// is being returned
        /// </summary>
        /// <param name="dataSourceId">the data source for which the links are generated</param>
        /// <returns>Collectino of Links</returns>
        public IEnumerable<Link> GenerateItemLinks(int dataSourceId)
        {
            //  Add links to get back to datasource and windfarms.
            var links = new List<Link>();

            links.Add(new Link
            {
                Rel = "datasource",
                Href = "/api/datasource/" + dataSourceId,
                Title = string.Empty,
                Type = "method=\"GET\""
            });
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
            //  Add one linking back to the DataSources
            links.Add(new Link()
            {
                Href = "/api/datasource/",
                Rel = "datasources",
                Title = string.Empty,
                Type = "method=\"GET\""
            });
            return links;
        }
    }
}