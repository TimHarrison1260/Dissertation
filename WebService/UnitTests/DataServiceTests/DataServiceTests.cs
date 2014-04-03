using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Model;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Services;
using Infrastructure.ServiceModel;
using Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.DataServiceTests
{
    [TestClass]
    public class DataServiceTests
    {
        private Mock<IDataSourceRepository> _mockDatasourceRepository;  // = null;
        private Mock<IAggregateRepository> _mockAggregateRepository;    // = null;
        private Mock<IDataTypeRepository> _mockDataTypeRepository;      // = null; 
        private Mock<IImportServiceResolver> _mockImportServiceResolver; // = null;
        private Mock<IDataSourceResolver> _mockDataSourceResolver;      // = null; 

        private IEnumerable<DataSource> _datasources;   // = null;
        private DataSource _dataSource1;                // = null;
        private DataSource _dataSource2;                // = null;
        private IQueryable<string> _dataTypes;          // = null;
        private IQueryable<Aggregate> _aggregates;      // = null;
        private IQueryable<Aggregate> _snhAggregates;   // = null;
        private IQueryable<Aggregate> _renUkAggregates; // = null;
        private Aggregate _achanyEstate;                // = null;
        private Aggregate _whiteleeWindfarm;            // = null;
        private Aggregate _blacklawWindfarm;            // = null;

            
        [TestInitialize]
        public void Initialise()
        {
            //  Initialise some test data
            //  DataSources
            _dataSource1 = new DataSource
            {
                Id = 1,
                AccessPath = string.Empty,
                CopyRight = string.Empty,
                Description = "Test Service 1 (Internal)",
                LastImported = DateTime.Now,
                SourceData = new Collection<AggregateData>(),
                SourceType = SourceTypeEnum.Dataset,
                Title = "Test Service 1"
            };
            _dataSource2 = new DataSource
            {
                Id = 2,
                AccessPath = string.Empty,
                CopyRight = string.Empty,
                Description = "Test Service 2 (External)",
                LastImported = DateTime.Now,
                SourceData = new Collection<AggregateData>(),
                SourceType = SourceTypeEnum.Scraper,
                Title = "Test Service 2"
            };

            _datasources = new List<DataSource>
            {
                _dataSource1,
                _dataSource2
            };

            //  DataTypes
            _dataTypes = new string[]
            {
                "Footprint",
                "Status"
            }.AsQueryable();

            //  Aggregates
            _achanyEstate = new Aggregate
            {
                Id = 1,
                Name = "Achany Estate",
                Data = new Collection<AggregateData>
                        {
                            new AggregateData
                                {
                                    Id = 1,
                                    Name = "Achany Estate",
                                    AggregateId = 1,
                                    DataSourceId = 1,
                                    Data = "{achany estate}",
                                    DataType = DataTypeEnum.FootPrint,
                                    LastUpdated = DateTime.Now
                                }
                        }
            };
            _whiteleeWindfarm = new Aggregate
            {
                Id = 2,
                Name = "Whitelee Windfarm",
                Data = new Collection<AggregateData>
                        {
                            new AggregateData
                                {
                                    Id = 2,
                                    Name = "Whitelee Windfarm",
                                    AggregateId = 2,
                                    DataSourceId = 1,
                                    Data = "{whitelee windfarm}",
                                    DataType = DataTypeEnum.FootPrint,
                                    LastUpdated = DateTime.Now
                                },
                            new AggregateData
                                {
                                    Id = 3,
                                    Name = "Whitelee Windfarm",
                                    AggregateId = 3,
                                    DataSourceId = 2,
                                    Data = "{URL to Whiltelee Windfarm}",
                                    DataType = DataTypeEnum.Statistics,
                                    LastUpdated = DateTime.Now
                                }
                        }
            };
            _blacklawWindfarm = new Aggregate
            {
                Id = 4,
                Name = "Blacklaw Windfarm",
                Data = new Collection<AggregateData>
                        {
                            new AggregateData
                                {
                                    Id = 4,
                                    Name = "Blacklaw Windfarm",
                                    AggregateId = 4,
                                    DataSourceId = 2,
                                    Data = "{URL to Blacklaw Windfarm}",
                                    DataType = DataTypeEnum.Statistics,
                                    LastUpdated = DateTime.Now
                                }
                        }
            };

            _aggregates = new List<Aggregate>
            {
                _achanyEstate,
                _whiteleeWindfarm,
                _blacklawWindfarm
            }.AsQueryable();

            _snhAggregates = new List<Aggregate>
            {
                _achanyEstate,
                _whiteleeWindfarm
            }.AsQueryable();

            _renUkAggregates = new List<Aggregate>
            {
                _whiteleeWindfarm,
                _blacklawWindfarm
            }.AsQueryable();

            //  Mock the dependencies
            _mockDatasourceRepository = new Mock<IDataSourceRepository>();
            _mockAggregateRepository = new Mock<IAggregateRepository>();
            _mockDataTypeRepository = new Mock<IDataTypeRepository>();
            _mockImportServiceResolver = new Mock<IImportServiceResolver>();
            _mockDataSourceResolver = new Mock<IDataSourceResolver>();
        }


        [TestMethod]
        [TestCategory("DataService")]
        public void GetDataSources_Successful_Returns2()
        {

            //  Arrange
            _mockDatasourceRepository.Setup(r => r.GetAll()).Returns(_datasources.AsQueryable());

            var dataService = new DataService(_mockAggregateRepository.Object, 
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GeDataSources().ToArray();

            //  Assert
            Assert.AreEqual(2, results.Count(), "Expected 2 results to be returned.");
            Assert.AreEqual("Test Service 1", results[0].Title, "Expected first datasource title to be 'Test Service 1'.");
            Assert.AreEqual("Test Service 2", results[1].Title, "Expected second datasource title to be 'Test Service 2'.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetDataSource_Successful_ReturnsDatasource2()
        {
            //  Arrange
            _mockDatasourceRepository.Setup(r => r.Get(It.IsAny<int>())).Returns(_dataSource2);

            var dataService = new DataService(_mockAggregateRepository.Object, 
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var result = dataService.GetDataSource(2);

            //  Assert
            Assert.AreEqual("Test Service 2", result.Title, "Expected datasource title to be 'Test Service 2'.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void ImportDataSource_Successful_ReturnsTRUE()
        {
            //  Arrange
            var mockImportService = new Mock<ISnhImportService>();
            mockImportService.Setup(i => i.ImportData(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _mockImportServiceResolver.Setup(r => r.Resolve(It.IsAny<int>())).Returns(mockImportService.Object);

            var dataService = new DataService(_mockAggregateRepository.Object, 
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var result = dataService.ImportDataSource(1, string.Empty);

            //  Assert
            Assert.IsTrue(result, "Expected Import to be successfull and return TRUE.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void ImportDataSource_UnSuccessful_ReturnsFALSE()
        {
            //  Arrange
            var mockImportService = new Mock<IRenUkImportService>();
            mockImportService.Setup(i => i.ImportData(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            _mockImportServiceResolver.Setup(r => r.Resolve(It.IsAny<int>())).Returns(mockImportService.Object);

            var dataService = new DataService(_mockAggregateRepository.Object, 
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var result = dataService.ImportDataSource(2, string.Empty);

            //  Assert
            Assert.IsFalse(result, "Expected Import to be unsuccessfull and return FALSE.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetDataTypes_Successful_ReturnsCollection()
        {
            //  Arrange
            var mockImportService = new Mock<IRenUkImportService>();
            mockImportService.Setup(i => i.ImportData(It.IsAny<int>(), It.IsAny<string>())).Returns(false);
            _mockDataTypeRepository.Setup(t => t.GetAll()).Returns(_dataTypes);

            var dataService = new DataService(_mockAggregateRepository.Object, 
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetDataTypes().ToArray();

            //  Assert
            Assert.AreEqual(2, results.Count(), "Expected 2 datatypes to be returned.");
            Assert.AreEqual("Footprint", results[0], "Expected first DataType to be 'Footprint'.");
            Assert.AreEqual("Status", results[1], "Expected second DataType to be 'Status'.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregates_Successful_ReturnsCollection_InternalDataStoreValues()
        {

            //////IUnitOfWork uow = new AggregateContext();
            //////IAggregateRepository aggregateRepository = new AggregateRepository(uow);

            //  Arrange
            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.GetAll()).Returns(_snhAggregates);
            //  Configure the DataSourceRepository
            _mockDatasourceRepository.Setup(d => d.Get(1)).Returns(_dataSource1);
            _mockDatasourceRepository.Setup(d => d.Get(2)).Returns(_dataSource2);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);
            //////var dataService = new DataService(aggregateRepository,
            //////    _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
            //////    _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetAggregates(string.Empty).ToArray();

            //  Assert
            Assert.AreEqual(2, results.Count(), "Expected 2 aggregates to be returned.");
            Assert.AreEqual("Achany Estate", results[0].Name, "Expected 'Achany Estate' to be returned as first object.");
            Assert.AreEqual("{achany estate}", results[0].Data.FirstOrDefault(t => t.DataType == DataTypeEnum.FootPrint).Data, "Expected original data '{achany estate}' to be returned");
            Assert.AreEqual("Whitelee Windfarm", results[1].Name, "Expected 'Whitelee Windfarm' to be returned as second object.");
            Assert.AreEqual("{whitelee windfarm}", results[1].Data.FirstOrDefault(t => t.DataType == DataTypeEnum.FootPrint).Data, "Expected original data '{whitelee windfarm}' to be returned");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregates_UnSuccessful_ReturnsEmpty_NoSourceValues()
        {
            //  Arrange
            _mockAggregateRepository.Setup(r => r.GetAll()).Returns(new List<Aggregate>().AsQueryable);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetAggregates(string.Empty).ToArray();

            //  Assert
            Assert.AreEqual(0, results.Count(), "Expected zero aggregates to be returned.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregates_Successful_ReturnsUnaltered_InvalidDataSource()
        {
            //  Arrange
            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.GetAll()).Returns(_snhAggregates);
            //  Configure the DataSourceRepository
            _mockDatasourceRepository.Setup(d => d.Get(It.IsAny<int>())).Returns(() => null);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetAggregates(string.Empty).ToArray();

            //  Assert
            Assert.AreEqual(2, results.Count(), "Expected 2 aggregates to be returned.");
            Assert.AreEqual("Achany Estate", results[0].Name, "Expected 'Achany Estate' to be returned as first object.");
            Assert.AreEqual("{achany estate}", results[0].Data.FirstOrDefault(t => t.DataType == DataTypeEnum.FootPrint).Data, "Expected original data '{achany estate}' to be returned");
            Assert.AreEqual("Whitelee Windfarm", results[1].Name, "Expected 'Whitelee Windfarm' to be returned as second object.");
            Assert.AreEqual("{whitelee windfarm}", results[1].Data.FirstOrDefault(t => t.DataType == DataTypeEnum.FootPrint).Data, "Expected original data '{whitelee windfarm}' to be returned");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregates_Successful_ReturnsUnaltered_InvalidResolver()
        {
            //  Arrange
            //  Configure the DataSourceResolver
            _mockDataSourceResolver.Setup(r => r.Resolve(It.IsAny<int>())).Returns(() => null);
            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.GetAll()).Returns(_renUkAggregates);
            //  Configure the DataSourceRepository
            _mockDatasourceRepository.Setup(d => d.Get(1)).Returns(_dataSource1);
            _mockDatasourceRepository.Setup(d => d.Get(2)).Returns(_dataSource2);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetAggregates(string.Empty).ToArray();

            //  Assert
            Assert.AreEqual(2, results.Count(), "Expected 2 aggregates to be returned.");
            Assert.AreEqual("Whitelee Windfarm", results[0].Name, "Expected 'Whitelee Windfarm' to be returned as first object.");
            Assert.AreEqual("{URL to Whiltelee Windfarm}", results[0].Data.FirstOrDefault(t => t.DataType == DataTypeEnum.Statistics).Data, "Expected the original data '{URL to Whiltelee Windfarm}' to be returned");
            Assert.AreEqual("Blacklaw Windfarm", results[1].Name, "Expected 'Blacklaw Windfarm' to be returned as first object.");
            Assert.AreEqual("{URL to Blacklaw Windfarm}", results[1].Data.FirstOrDefault(t => t.DataType == DataTypeEnum.Statistics).Data, "Expected the original data '{URL to Blacklaw Windfarm}' to be returned");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregate_Successful_ReturnsAggregate1()
        {
            //  Arrange
            var importAggregate = new ImportAggregate
            {
                Identifier = "Whitelee Windfarm",
                Data = new Collection<ImportData>
                {
                    new ImportData
                    {
                        DataType = DataTypeEnum.Statistics,
                        Data = "External JSON Object"
                    }
                },
                ImportDate = DateTime.Now,
                SourceId = 2,
            };

            //  Setup the mock datasource instance
            var mockDataSource = new Mock<IRenUkDataSource>();
            mockDataSource.Setup(d => d.Get(It.IsAny<string>())).Returns(importAggregate);
            //  Configure the DataSourceResolver
            _mockDataSourceResolver.Setup(r => r.Resolve(It.IsAny<int>())).Returns(mockDataSource.Object);
            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.Get(2)).Returns(_whiteleeWindfarm);
            //  Configure the DataSourceRepository
            _mockDatasourceRepository.Setup(d => d.Get(1)).Returns(_dataSource1);
            _mockDatasourceRepository.Setup(d => d.Get(2)).Returns(_dataSource2);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var result = dataService.GetAggregate(_whiteleeWindfarm.Id);

            //  Assert
            Assert.AreEqual("Whitelee Windfarm", result.Name, "Expected 'Whitelee Windfarm' to be returned as first object.");
            Assert.AreEqual("{whitelee windfarm}", result.Data.FirstOrDefault(t => t.DataType == DataTypeEnum.FootPrint).Data, "Expected original data '{whitelee windfarm}' to be returned");
            Assert.AreEqual("External JSON Object", result.Data.FirstOrDefault(t => t.DataType == DataTypeEnum.Statistics).Data, "Expected the data to have been overridden as external source");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregate_UnSuccessful_NoAggregateFound()
        {
            //  Arrange
            //  Setup the mock datasource instance
            var mockDataSource = new Mock<IRenUkDataSource>();
            mockDataSource.Setup(d => d.Get(It.IsAny<string>())).Returns(() => null);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var result = dataService.GetAggregate(_whiteleeWindfarm.Id);

            //  Assert
            Assert.IsNull(result, "Expected a NULL object to be returned");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregates_UnSuccessful_NoAggregatesWithDataType_MixedSourceValues()
        {
            //  Arrange
            var aggregatesNone = new List<Aggregate>().AsQueryable();

            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.GetAllWithDataType(It.IsAny<DataTypeEnum>())).Returns(aggregatesNone);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetAggregatesWithDataType(DataTypeEnum.Statistics).ToArray();

            //  Assert
            Assert.AreEqual(0, results.Count(), "Expected empty collection of aggregates to be returned.");
        }

        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregate_Successful_ReturnsDataTypeStatistics_MixedSourceValues()
        {
            //  Arrange
            var importAggregate = new ImportAggregate
            {
                Identifier = "Whitelee Windfarm",
                Data = new Collection<ImportData>
                {
                    new ImportData
                    {
                        DataType = DataTypeEnum.Statistics,
                        Data = "External JSON Object"
                    }
                },
                ImportDate = DateTime.Now,
                SourceId = 2,
            };

            //  Setup the mock datasource instance
            var mockDataSource = new Mock<IRenUkDataSource>();
            mockDataSource.Setup(d => d.Get(It.IsAny<string>())).Returns(importAggregate);
            //  Configure the DataSourceResolver
            _mockDataSourceResolver.Setup(r => r.Resolve(It.IsAny<int>())).Returns(mockDataSource.Object);
            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.Get(It.IsAny<int>(), It.IsAny<DataTypeEnum>())).Returns(_blacklawWindfarm);
            //  Configure the DataSourceRepository
            _mockDatasourceRepository.Setup(d => d.Get(1)).Returns(_dataSource1);
            _mockDatasourceRepository.Setup(d => d.Get(2)).Returns(_dataSource2);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var results = dataService.GetAggregateWithDataType(4, DataTypeEnum.Statistics);

            //  Assert
            Assert.AreEqual("Blacklaw Windfarm", results.Name, "Expected 'Blacklaw Windfarm' to be returned as first object.");
            Assert.AreEqual("External JSON Object", results.Data.FirstOrDefault(t => t.DataType == DataTypeEnum.Statistics).Data, "Expected the data to have been overridden as external source");
            Assert.AreEqual(DataTypeEnum.Statistics, results.Data.FirstOrDefault().DataType, "Expected original data '{achany estate}' to be returned");
            Assert.AreEqual(1, results.Data.Count, "Expected only one data segment");

        }


        [TestMethod]
        [TestCategory("DataService")]
        public void GetAggregate_UnSuccessful_NoSuchAggregate_MixedSourceValues()
        {
            //  Arrange
            //  Configure the AggregateRepository
            _mockAggregateRepository.Setup(r => r.Get(It.IsAny<int>(), It.IsAny<DataTypeEnum>())).Returns(() => null);

            var dataService = new DataService(_mockAggregateRepository.Object,
                _mockDatasourceRepository.Object, _mockDataTypeRepository.Object,
                _mockImportServiceResolver.Object, _mockDataSourceResolver.Object);

            //  Act
            var result = dataService.GetAggregateWithDataType(4, DataTypeEnum.Statistics);

            //  Assert
            Assert.IsNull(result, "Expected NULL reference to be returned, Aggregate does not exist.");
        }

    }
}
