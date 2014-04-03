using System;
using System.Collections.Generic;
using AggregateWebService.Interfaces;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Models;
using Core.Model;

namespace AggregateWebService.Generators
{
    /// <summary>
    /// Class <c>AggregateLinkGenerator</c> has the responsibility
    /// to generate the Hypermedia Links for the aggregate entry
    /// being returned through the web service.
    /// </summary>
    public class AggregateLinkGenerator : IAggregateLinkGenerator
    {
        /// <summary>
        /// Generates the Links for each aggregate, when a collection 
        /// of aggregates are being returned
        /// </summary>
        /// <param name="aggregate">The aggregate for which the links are generated</param>
        /// <returns>Collection of links</returns>
        public IEnumerable<Models.Link> GenerateCollectionLinks(Core.Model.Aggregate aggregate)
        {
            var links = new List<Link>()
            {
                new Link()
                {
                    Href = "/api/windfarm/" + aggregate.Id,
                    Rel = "self",
                    Title = string.Empty,
                    Type = "method=\"GET\""
                }
            };
            return links;
        }

        /// <summary>
        /// Generates the links for an aggregate when a specific
        /// aggregate is being returned
        /// </summary>
        /// <param name="aggregate">the aggregate for which the links are generated</param>
        /// <returns>Collectino of Links</returns>
        public IEnumerable<Models.Link> GenerateItemLinks(Core.Model.Aggregate aggregate)
        {
            var links = new List<Link>();
            links.Add(new Link()
            {
                Href = "/api/windfarm/" + aggregate.Id,
                Rel = "self",
                Title = string.Empty,
                Type = "method=\"GET\""
            });

            foreach (var d in aggregate.Data)
            {
                links.Add(new Link()
                {
                    Href = "/api/" + Enum.GetName(typeof(DataTypeEnum), d.DataType) + "/" + aggregate.Id,
                    Rel = Enum.GetName(typeof(DataTypeEnum), d.DataType),
                    Title = string.Empty,
                    Type = "method=\"GET\""
                });
                links.Add(new Link()
                {
                    Href = "/api/windfarm/" + aggregate.Id + "/" + Enum.GetName(typeof(DataTypeEnum), d.DataType),
                    Rel = Enum.GetName(typeof(DataTypeEnum), d.DataType),
                    Title = string.Empty,
                    Type = "method=\"GET\""
                });
            }
            //  Add one linking back to the DataTypes
            links.Add(new Link()
            {
                Href = "/api/datatype/",
                Rel = "datatypes",
                Title = string.Empty,
                Type = "method=\"GET\""
            });
            //  Add a link back to the DataSources
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