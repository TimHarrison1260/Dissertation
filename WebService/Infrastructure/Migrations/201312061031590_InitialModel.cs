namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AggregateDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataSourceId = c.Int(nullable: false),
                        AggregateId = c.Int(nullable: false),
                        Name = c.String(),
                        Data = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                        DataType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Aggregates", t => t.AggregateId, cascadeDelete: true)
                .ForeignKey("dbo.DataSources", t => t.DataSourceId, cascadeDelete: true)
                .Index(t => t.AggregateId)
                .Index(t => t.DataSourceId);
            
            CreateTable(
                "dbo.Aggregates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        CopyRight = c.String(),
                        AccessPath = c.String(),
                        SourceType = c.Int(nullable: false),
                        LastImported = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AggregateDatas", "DataSourceId", "dbo.DataSources");
            DropForeignKey("dbo.AggregateDatas", "AggregateId", "dbo.Aggregates");
            DropIndex("dbo.AggregateDatas", new[] { "DataSourceId" });
            DropIndex("dbo.AggregateDatas", new[] { "AggregateId" });
            DropTable("dbo.DataSources");
            DropTable("dbo.Aggregates");
            DropTable("dbo.AggregateDatas");
        }
    }
}
