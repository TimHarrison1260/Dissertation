using System.Collections.Generic;

namespace AggregateWebService.Models
{
    /// <summary>
    /// Class <c>Datatype</c> describes the model for the 
    /// available datatypes, including the links.
    /// </summary>
    public class DataType
    {
        /// <summary>
        /// Gets or sets the name of the DataType
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A collection of links, representing the hypermedia 
        /// for the DataType
        /// </summary>
        public IEnumerable<Link> Links { get; set; }
    }
}