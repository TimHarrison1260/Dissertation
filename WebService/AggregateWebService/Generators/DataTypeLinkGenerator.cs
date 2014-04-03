using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Models;

namespace AggregateWebService.Generators
{
    /// <summary>
    /// Class <c>DataTypeLinkGenerator</c> has the responsibility
    /// to generate the Hypermedia Links for the DataType entry
    /// being returned through the web service.
    /// </summary>
    public class DataTypeLinkGenerator : IDataTypeLinkGenerator
    {
        /// <summary>
        /// Generates the Footprint Links for each aggregate, when a collection 
        /// of aggregates are being returned
        /// </summary>
        /// <param name="dataType">The DataType</param>
        /// <returns>Collection of links</returns>
        public IEnumerable<Models.Link> GenerateCollectionLinks(string dataType)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href = "/api/" + dataType,
                    Rel = "Windfarms",
                    Title = string.Empty,
                    Type = "method=\"GET\""
                }
            };
            return links;
        }


        public IEnumerable<Link> GenerateItemLinks(string dataType)
        {
            throw new System.NotImplementedException();
        }
    }
}