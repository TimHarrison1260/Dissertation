using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Core.Interfaces;
using Core.Model;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Data
{
    /// <summary>
    /// Class <c>AggregateContext</c> represents the Entity Framework (V6.0)
    /// DbContext to the underlying database.
    /// </summary>
    public class AggregateContext : DbContext, IUnitOfWork
    {
        public AggregateContext()
        {
            //  Switch off lazy loading: AggregateData is always needed when Aggregates are selected
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
        /// <param name="modelBuilder">Instande of the Database Model Builder class</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  



        }



    }
}
