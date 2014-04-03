using System.Collections.Generic;

namespace AggregateWebService.Models
{
    /// <summary>
    /// Class <c>WindfarmInfo</c> represents the Windfarm
    /// entity retrieved from the action but includes only
    /// the Name as summary information and the Links
    /// </summary>
    public class WindfarmInfo
    {
        /// <summary>
        /// The Name of the aggregate
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A collection of links related to the aggregate
        /// for navigating through the service
        /// </summary>
        public IEnumerable<Link> Links { get; set; }
    }
}