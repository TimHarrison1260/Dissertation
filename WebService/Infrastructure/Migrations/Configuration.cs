using System.Collections.ObjectModel;
using Core.Model;

namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

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
            //  to avoid creating duplicate seed data.


            //  Add the entries to the DataSources Table.  These are configured, the others are loaded
            //  from the data imported and aggregated.
            var snhDataSource = new Core.Model.DataSource()
                {
                    Id = 1,
                    Title = "Scottish Natural Heritage KML Source",
                    Description = "A KML file of Footprint information for all known wind farms in Scotland.",
                    CopyRight = "Copyright Scottish Natural Heritage contains Ordnance Survey data (C) Crown Copyright and database right (2014)",
                    AccessPath = string.Empty,
                    SourceType = Core.Model.SourceTypeEnum.Dataset,
                    LastImported = new DateTime(2013, 12, 01),
                };
            var renUkDataSource = new Core.Model.DataSource()
                {
                    Id = 2,
                    Title = "RenewableUk WebSite",
                    Description = "A Web Site containing the status and information about all know wind farms within the United Kingdon.",
                    CopyRight = "Copyright Renewable UK (c) 2014",
                    AccessPath = "http://www.renewableuk.com/en/renewable-energy/wind-energy/uk-wind-energy-database/index.cfm/",
                    SourceType = Core.Model.SourceTypeEnum.Scraper,
                    LastImported = new DateTime(2013, 12, 02),
                };
            var turbineDataSource = new Core.Model.DataSource()
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

            var aikengallTurbineData = new Core.Model.AggregateData()
            {
                Id = 2,
                Name = "Aikengall",
                DataSourceId = 3,
                AggregateId = 2,
                Data = "These would be turbine coordinates in JSON format",
                LastUpdated = new DateTime(2013, 12, 3),
                DataType = Core.Model.DataTypeEnum.Turbine
            };
            var aikengallStatusData = new Core.Model.AggregateData()
            {
                Id = 3,
                Name = "Aikengall",
                DataSourceId = 1,
                AggregateId = 2,
                Data = "This woudl be the Status record from SNH",
                LastUpdated = new DateTime(2013, 12, 3),
                DataType = Core.Model.DataTypeEnum.Status
            };

            var aikengallAggregate = new Core.Model.Aggregate()
                {
                    Id = 2,
                    Name = "Aikengall",
                    LastUpdated = new DateTime(2014, 12, 31),
                    Data = new Collection<AggregateData>()
                };
            aikengallAggregate.Data.Add(aikengallTurbineData);
            aikengallAggregate.Data.Add(aikengallStatusData);

            //  Add to the datacontext
            context.DataSources.AddOrUpdate(snhDataSource);
            context.DataSources.AddOrUpdate(renUkDataSource);
            context.DataSources.AddOrUpdate(turbineDataSource);
            context.Aggregates.AddOrUpdate(whiteleeAggregate);
            context.Aggregates.AddOrUpdate(aikengallAggregate);

        }
    }
}
