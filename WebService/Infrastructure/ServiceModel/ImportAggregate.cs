using System;
using System.Collections.Generic;

namespace Infrastructure.ServiceModel
{
    /// <summary>
    /// Class <c>ImportAggregate</c> describes the aggregate
    /// information from each source.  It contains the raw
    /// data for each <see cref="Core.Model.DataTypeEnum"/>.
    /// </summary>
    /// <remarks>
    /// Implements IComparable(T), to allow the import aggregates
    /// to be sorted on the aggregate identifier (the Name).
    /// </remarks>
    public class ImportAggregate : IComparable<ImportAggregate>, IComparer<ImportAggregate>
    {
        /// <summary>
        /// Gets or sets the Identifier for the Aggregate, the Wind Farm Name
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Data Source
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets the date of the Import
        /// </summary>
        public DateTime ImportDate { get; set; }

        /// <summary>
        /// Gets or sets the Data being imported
        /// </summary>
        public ICollection<ImportData> Data { get; set; }

        #region IComparable Interface

        /// <summary>
        /// Implements the <see cref="System.IComparable"/> interface, 
        /// to allow equality to be determined
        /// </summary>
        /// <param name="other">The aggregate to compare to this one.</param>
        /// <returns>Numeric value representing the order of the two oabjects</returns>
        public int CompareTo(ImportAggregate other)
        {
            var result = Compare(this, other);
            return result;
        }

        #endregion

        #region IComparer Interface

        /// <summary>
        /// Implements the <see cref="IComparer{T}"/> interface.
        /// </summary>
        /// <param name="x">Aggregate 1</param>
        /// <param name="y">Aggregate 2</param>
        /// <returns>Numeric value representing the order of the two oabjects</returns>
        public int Compare(ImportAggregate x, ImportAggregate y)
        {
            var result = String.Compare(x.Identifier, y.Identifier, StringComparison.Ordinal);
            return result;
        }

        #endregion
    }
}
