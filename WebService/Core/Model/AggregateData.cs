using System;
using System.Configuration;

namespace Core.Model
{
    /// <summary>
    /// Class <c>AggregateData</c> holds the instances of data
    /// either imported to the internal store, or deines the 
    /// path (usually a URL) that accesses the information 
    /// directly.
    /// </summary>
    public class AggregateData
    {
        /// <summary>
        /// Gets or sets a unique Id for the data
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Id or the DataSource
        /// </summary>
        public int DataSourceId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Aggregate root
        /// </summary>
        public int AggregateId { get; set; }

        /// <summary>
        /// Gets or sets the Aggregate Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the actual data of the aggregate
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the date/time last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the DataType of the data: eg. Location, Turbine, Statistical
        /// </summary>
        public virtual DataTypeEnum DataType { get; set; }

        /// <summary>
        /// Navigation property to Aggregate Root
        /// </summary>
        public virtual Aggregate Aggregate { get; set; }

    }
}
