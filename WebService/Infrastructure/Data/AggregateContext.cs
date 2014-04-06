using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using Core.Model;
using Infrastructure.Interfaces.Data;
using Infrastructure.Migrations;

namespace Infrastructure.Data
{
    /// <summary>
    /// Class <c>AggregateContext</c> represents the Entity Framework (V6.0)
    /// DbContext to the underlying database.
    /// </summary>
    public class AggregateContext : DbContext, IUnitOfWork
    {
        public AggregateContext() 
            : base("AggregateContext")
        {
            //  Switch on lazy loading: AggregateData is only needed when a specific Aggregate are selected,
            //  Never when a collection is needed, as the links are returned to the specific aggregates.
            //this.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Gets or sets the Aggregates context
        /// </summary>
        public IDbSet<Aggregate> Aggregates { get; set; }

        /// <summary>
        /// Gets or sets the DataSource context
        /// </summary>
        public IDbSet<DataSource> DataSources { get; set; }

        /// <summary>
        /// Gets or sets the AggregateData context
        /// </summary>
        public IDbSet<AggregateData> AggregatedData { get; set; }

        /// <summary>
        /// Overriding this method allows us to configure the way EF creates the underlying database.
        /// </summary>
        /// <param name="modelBuilder">Instance of the Database Model Builder class</param>
        /// <remarks>
        ///  Set the initializer for migration - Create and Seed the database, which runs every time 
        ///  the application is started, which is a performance issue, but ensures the database is
        ///  created correctly and migrated to the latest version, without any user intervention for
        ///  the first run.  The seed method, in <see cref="Configuration"/> uses AddOrUpdate method 
        /// so that duplicate data is not added.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Infrastructure.Data.AggregateContext, Infrastructure.Migrations.Configuration>());

        }



    }
}
