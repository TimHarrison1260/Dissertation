using System;
using System.Collections.Generic;

namespace AggregateWebService.Models
{
    /// <summary>
    /// Class <c>DataSource</c> defines the information for the
    /// aggregated data sources available through the service.
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// The Id of the aggregate data source
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The title of the aggregate data source
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A Description of the agregate data source contents
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The copyright statement of the owner of the data
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// The path to the data source
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The date / time the data was last imported into the service
        /// </summary>
        public DateTime LastUpdateed { get; set; }

        /// <summary>
        /// A collection of Links for navigating through the resources
        /// relevant to the data source.
        /// </summary>
        public IEnumerable<Link> Links { get; set; } 
    }
}