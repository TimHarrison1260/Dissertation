using System.Collections.Generic;

namespace AggregateWebService.Models
{
    /// <summary>
    /// Class <c>Windfarm</c> represents the Windfarm
    /// entity retrieved from the action.
    /// </summary>
    public class Windfarm
    {
        /// <summary>
        /// The internal Id of the Windfarm
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Name of the aggregate
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A collection of data types and data for the aggregate
        /// </summary>
        public IEnumerable<WindfarmData> Data { get; set; }

        /// <summary>
        /// A collection of links related to the aggregate
        /// for navigating through the service
        /// </summary>
        public IEnumerable<Link> Links { get; set; }
    }
}