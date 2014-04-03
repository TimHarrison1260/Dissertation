using System.Collections.ObjectModel;
using Core.Model;

namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.Data.AggregateContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.Data.AggregateContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //  Add the entries to the DataSources Table.  These are configured, the others are loaded
            //  from the data imported and aggregated.
            var snhDataSource = new Core.Model.DataSource()
                {
                    Id = 1,
                    Title = "Snh KML Source",
                    Description = "The description should to here",
                    CopyRight = "The SnH copyright statement goes here",
                    AccessPath = string.Empty,
                    SourceType = Core.Model.SourceTypeEnum.Dataset,
                    LastImported = new DateTime(2013, 12, 01),
                };
            var renUkDataSource = new Core.Model.DataSource()
                {
                    Id = 2,
                    Title = "RenewableUk WebSite",
                    Description = "The description should to here",
                    CopyRight = "Put a copyright statement here",
                    AccessPath = string.Empty,
                    SourceType = Core.Model.SourceTypeEnum.Scraper,
                    LastImported = new DateTime(2013, 12, 02),
                };
            var TurbineDataSource = new Core.Model.DataSource()
                {
                    Id = 3,
                    Title = "Turbine Data",
                    Description = "Manually Entered Turbine Data",
                    CopyRight = "N/A",
                    AccessPath = string.Empty,
                    SourceType = Core.Model.SourceTypeEnum.Manual,
                    LastImported = new DateTime(2013, 12, 3)
                };

            //  Add some Fake Turbine data
            var whiteleeTurbineData = new Core.Model.AggregateData()
                {
                    Id = 1,
                    Name = "Whitelee Wind Farm",
                    DataSourceId = 3,
                    AggregateId = 1,
                    Data = "These would be turbine coordinates in JSON format",
                    LastUpdated = new DateTime(2013, 12,3),
                    DataType = Core.Model.DataTypeEnum.Turbine
                };
            
            var whiteleeAggregate = new Core.Model.Aggregate()
                {
                    Id = 1,
                    Name = "Whitelee Wind Farm",
                    LastUpdated = new DateTime(2013, 12,3),
                    Data = new Collection<AggregateData>()
                };
            whiteleeAggregate.Data.Add(whiteleeTurbineData);

            //  Add to the datacontext
            context.DataSources.AddOrUpdate(snhDataSource);
            context.DataSources.AddOrUpdate(renUkDataSource);
            context.DataSources.AddOrUpdate(TurbineDataSource);
            context.Aggregates.AddOrUpdate(whiteleeAggregate);

        }
    }
}
