using System.Collections.Generic;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Models;
using Core.Model;

namespace AggregateWebService.Interfaces.Mappers
{
    /// <summary>
    /// Interface <c>IAggregateMapper</c> is provides the mapping
    /// of the Windfarm information from the dataservice
    /// to the UI Windfarm class
    /// </summary>
    public interface IAggregateMapper
    {
        /// <summary>
        /// Responsibile for mapping a collection of Aggregates
        /// to the set of links to be returned by the web service
        /// </summary>
        /// <param name="aggregates">Collection of Windfarm Data</param>
        /// <returns>Collection of Links to the individual aggregate data sources</returns>
        //IEnumerable<Windfarm> MapAggregatesToLinks(IEnumerable<Core.Model.Windfarm> aggregates, IAggregateLinkGenerator generator);
        //IEnumerable<WindfarmInfo> MapAggregatesToLinks(IEnumerable<Core.Model.Aggregate> aggregates, IAggregateLinkGenerator generator);
        IEnumerable<WindfarmInfo> MapAggregatesToLinks(IEnumerable<Core.Model.Aggregate> aggregates, ILinkGenerator<Core.Model.Aggregate> generator);

        /// <summary>
        /// Maps a single aggregate to the UI datasource
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="aggregate">the Windfarm Data</param>
        /// <param name="generator">Instance of the Link generator class</param>
        /// <returns>UI data source and links</returns>
        //Windfarm MapAggregate(Core.Model.Aggregate aggregate, IAggregateLinkGenerator generator);
        Windfarm MapAggregate(Core.Model.Aggregate aggregate, ILinkGenerator<Core.Model.Aggregate> generator);

        /// <summary>
        /// Maps a single aggregate to the UI datasource, and only includes the specified datatype segment
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="aggregate">the Windfarm Data</param>
        /// <param name="includeOnlyDataType">Include only the specified datatype segment</param>
        /// <param name="generator">Instance of the Link generator class</param>
        /// <returns>UI data source and links</returns>
        //Windfarm MapAggregate(Core.Model.Aggregate aggregate, DataTypeEnum includeOnlyDataType, IAggregateLinkGenerator generator);
        Windfarm MapAggregate(Core.Model.Aggregate aggregate, DataTypeEnum includeOnlyDataType, ILinkGenerator<Core.Model.Aggregate> generator);
    }
}
