using System.Data.Entity;
using Core.Model;

namespace Infrastructure.Interfaces.Data
{
    /// <summary>
    /// Interface <c>IUnitOfWork</c> allows us to implement
    /// the Unit Of Work pattern for the Data context and
    /// inject the correct instance into the repositories
    /// that will access it.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets or sets the Aggregates context
        /// </summary>
        IDbSet<Aggregate> Aggregates { get; set; }

        /// <summary>
        /// Gets or sets the DataSource context
        /// </summary>
        IDbSet<DataSource> DataSources { get; set; }

        /// <summary>
        /// Gets or sets the AggregateData context
        /// </summary>
        IDbSet<AggregateData> AggregatedData { get; set; }

        /// <summary>
        /// Saves any changes to the underlying data store
        /// </summary>
        /// <returns>An Integer representing the Id of the updated or added object.</returns>
        int SaveChanges();  // implemented by IDbSet.
    }
}
