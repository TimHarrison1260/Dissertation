using System.Collections.Generic;

namespace AggregateWebService.Models
{
    /// <summary>
    /// Class <c>DataSourceInfo</c> defines the summary information 
    /// for the data source available through the service.  It includes 
    /// the Title only along with the relevant Links
    /// </summary>
    public class DataSourceInfo
    {
        /// <summary>
        /// The title of the aggregate data source
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A collection of Links for navigating through the resources
        /// relevant to the data source.
        /// </summary>
        public IEnumerable<Link> Links { get; set; }
    }
}