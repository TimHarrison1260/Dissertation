using System;
using System.Collections.Generic;
using AggregateWebService.Interfaces;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using AggregateWebService.Models;
using Core.Model;

namespace AggregateWebService.Mappers
{
    /// <summary>
    /// Class <c>AggregateMapper</c> is provides the mapping
    /// of the Windfarm information from the dataservice
    /// to the UI Windfarm class
    /// </summary>
    public class AggregateMapper : IAggregateMapper
    {
        /// <summary>
        /// Responsibile for mapping a collection of Aggregates
        /// to the set of links to be returned by the web service
        /// </summary>
        /// <param name="aggregates">Collection of Windfarm Data</param>
        /// <param name="generator">The instance of the LinkGenerator to generate the Links</param>
        /// <returns>Collection of Links to the individual aggregate data sources</returns>
        //public IEnumerable<WindfarmInfo> MapAggregatesToLinks(IEnumerable<Aggregate> aggregates, IAggregateLinkGenerator> generator)
        public IEnumerable<WindfarmInfo> MapAggregatesToLinks(IEnumerable<Aggregate> aggregates, ILinkGenerator<Core.Model.Aggregate> generator)
        {
            var uiAggregates = new List<WindfarmInfo>();
            foreach (var aggregate in aggregates)
            {
                var uiAggregate = new WindfarmInfo()
                {
                    Name = aggregate.Name,
                    Links = generator.GenerateCollectionLinks(aggregate)
                };
                uiAggregates.Add(uiAggregate);
            }
            return uiAggregates;
        }

        /// <summary>
        /// Maps a single aggregate to the UI datasource
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="aggregate">the Windfarm Data</param>
        /// <param name="generator">The instance of the LinkGenerator to generate the Links</param>
        /// <returns>UI data source and links</returns>
        //public Windfarm MapAggregate(Aggregate aggregate, IAggregateLinkGenerator generator)
        public Windfarm MapAggregate(Aggregate aggregate, ILinkGenerator<Core.Model.Aggregate> generator)
        {
            if (aggregate == null)
                return  new Windfarm();

            //  Create a data segment and a link for each datatype
            var data = new List<WindfarmData>();
            foreach (var d in aggregate.Data)
            {
                data.Add(new WindfarmData()
                {
                    Type = Enum.GetName(typeof(DataTypeEnum), d.DataType),
                    Data = d.Data
                });
            }
            //  Create the UiAggregate
            var uiAggregate = new Windfarm()
            {
                Id = aggregate.Id,
                Name = aggregate.Name,
                Data = data,
                Links = generator.GenerateItemLinks(aggregate)
            };
            return uiAggregate;
        }

        /// <summary>
        /// Maps a single aggregate to the UI datasource, and only includes the specified datatype segment
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="aggregate">the Windfarm Data</param>
        /// <param name="includeOnlyDataType">Include only the specified datatype segment</param>
        /// <param name="generator">Instance of the Link generator class</param>
        /// <returns>UI data source and links</returns>
        //public Windfarm MapAggregate(Core.Model.Aggregate aggregate, DataTypeEnum includeOnlyDataType, IAggregateLinkGenerator generator)
        public Windfarm MapAggregate(Core.Model.Aggregate aggregate, DataTypeEnum includeOnlyDataType, ILinkGenerator<Core.Model.Aggregate> generator)
        {
            if (aggregate == null)
                return new Windfarm();

            //  Create a data segment and a link for each datatype
            var data = new List<WindfarmData>();
            foreach (var d in aggregate.Data)
            {
                if (d.DataType == includeOnlyDataType)
                {
                    data.Add(new WindfarmData()
                    {
                        Type = Enum.GetName(typeof(DataTypeEnum), d.DataType),
                        Data = d.Data
                    });
                    break;      //  Found, now leave immediately
                }
            }
            //  Create the UiAggregate
            var uiAggregate = new Windfarm()
            {
                Id = aggregate.Id,
                Name = aggregate.Name,
                Data = data,
                Links = generator.GenerateItemLinks(aggregate)      //  Include links for all other datatypes
            };
            return uiAggregate;
        }
    }
}