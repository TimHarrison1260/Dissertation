USE [Infrastructure.Data.AggregateContext]
GO
/****** Object:  ForeignKey [FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]    Script Date: 03/14/2014 10:18:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas] DROP CONSTRAINT [FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]
GO
/****** Object:  ForeignKey [FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]    Script Date: 03/14/2014 10:18:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas] DROP CONSTRAINT [FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]
GO
/****** Object:  Table [dbo].[AggregateDatas]    Script Date: 03/14/2014 10:18:28 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas] DROP CONSTRAINT [FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas] DROP CONSTRAINT [FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AggregateDatas]') AND type in (N'U'))
DROP TABLE [dbo].[AggregateDatas]
GO
/****** Object:  Table [dbo].[Aggregates]    Script Date: 03/14/2014 10:18:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Aggregates]') AND type in (N'U'))
DROP TABLE [dbo].[Aggregates]
GO
/****** Object:  Table [dbo].[DataSources]    Script Date: 03/14/2014 10:18:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSources]') AND type in (N'U'))
DROP TABLE [dbo].[DataSources]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 03/14/2014 10:18:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
DROP TABLE [dbo].[__MigrationHistory]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 03/14/2014 10:18:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__MigrationHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataSources]    Script Date: 03/14/2014 10:18:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataSources]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataSources](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[CopyRight] [nvarchar](max) NULL,
	[AccessPath] [nvarchar](max) NULL,
	[SourceType] [int] NOT NULL,
	[LastImported] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.DataSources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Aggregates]    Script Date: 03/14/2014 10:18:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Aggregates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Aggregates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Aggregates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[AggregateDatas]    Script Date: 03/14/2014 10:18:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AggregateDatas]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AggregateDatas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataSourceId] [int] NOT NULL,
	[AggregateId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Data] [nvarchar](max) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[DataType] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AggregateDatas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  ForeignKey [FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]    Script Date: 03/14/2014 10:18:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId] FOREIGN KEY([AggregateId])
REFERENCES [dbo].[Aggregates] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas] CHECK CONSTRAINT [FK_dbo.AggregateDatas_dbo.Aggregates_AggregateId]
GO
/****** Object:  ForeignKey [FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]    Script Date: 03/14/2014 10:18:28 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId] FOREIGN KEY([DataSourceId])
REFERENCES [dbo].[DataSources] ([Id])
ON DELETE CASCADE
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]') AND parent_object_id = OBJECT_ID(N'[dbo].[AggregateDatas]'))
ALTER TABLE [dbo].[AggregateDatas] CHECK CONSTRAINT [FK_dbo.AggregateDatas_dbo.DataSources_DataSourceId]
GO
