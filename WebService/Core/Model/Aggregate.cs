using System;
using System.Collections.Generic;

namespace Core.Model
{
    /// <summary>
    /// Class <c>Aggregate</c> defines the Root of the aggregated data.
    /// It contains a numeric Id and the Aggregate Identifier, a string.
    /// </summary>
    public class Aggregate
    {
        /// <summary>
        /// Gets or sets the Aggregate Root Id of the Aggregate
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the aggregate identifier
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the date/time last imported into the service
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="AggregateData"/> held
        /// for the aggregate root.
        /// </summary>
        public virtual ICollection<AggregateData> Data { get; set; }

    }
}
