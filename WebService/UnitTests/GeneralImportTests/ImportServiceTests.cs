using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Model;
using Infrastructure.Interfaces.Algorithms;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Mappers;
using Infrastructure.ServiceModel;
using Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.GeneralImportTests
{
    [TestClass]
    public class ImportServiceTests
    {
        private Mock<IAggregateRepository> _mockRepository = null;
        private Mock<IDataSourceRepository> _mockDataSourceRepository = null;
        private Mock<ISnhDataSource> _mockDatasource = null;
        private Mock<IMatchingAlgorithm> _mockAlgorithm = null;
        private Mock<IMapper<ImportAggregate, Aggregate>> _mockMapper = null;

        private IImportService _importService = null;

        private DateTime _timestamp = new DateTime();
        private IList<ImportAggregate> _importAggregates = null;
        private IList<Aggregate> _aggregates = null;
        private Aggregate _aggregateAchany = null;

        [TestInitialize]
        public void Initialise()
        {
            _mockRepository = new Mock<IAggregateRepository>();
            _mockDataSourceRepository = new Mock<IDataSourceRepository>();
            _mockDatasource = new Mock<ISnhDataSource>();
            _mockAlgorithm = new Mock<IMatchingAlgorithm>();
            _mockMapper = new Mock<IMapper<ImportAggregate, Aggregate>>();

            _importService = null;

            _importAggregates = new List<ImportAggregate>()
                {
                    new ImportAggregate()
                        {
                            Identifier = "Achany Estate",
                            ImportDate = _timestamp,
                            SourceId = 1,
                            Data = new List<ImportData>()
                                {
                                    new ImportData()
                                        {
                                            Data = "{achany estate}",
                                            DataType = DataTypeEnum.FootPrint
                                        }
                                }
                        },
                    new ImportAggregate()
                        {
                            Identifier = "Achlachan",
                            ImportDate = _timestamp,
                            SourceId = 1,
                            Data = new List<ImportData>()
                                {
                                    new ImportData()
                                        {
                                            Data = "{achlachan}",
                                            DataType = DataTypeEnum.FootPrint
                                        }
                                }
                        }
                };

            _aggregateAchany = new Aggregate()
                {
                    Id = 1,
                    Name = "Achany Estate",
                    Data = new Collection<AggregateData>()
                        {
                            new AggregateData()
                                {
                                    Id = 1,
                                    Name = "Achany Estate",
                                    AggregateId = 1,
                                    DataSourceId = 1,
                                    Data = "{achany estate}",
                                    DataType = DataTypeEnum.FootPrint,
                                    LastUpdated = _timestamp
                                }
                        }
                };

            _aggregates = new List<Aggregate>()
                {
                    _aggregateAchany
                };
        }


        [TestMethod]
        [TestCategory("ImportServices")]
        public void SourceDoesNotLoad_ReturnsFalse()
        {
            //  Arrange
            _mockDataSourceRepository.Setup(s => s.IsValidDataSource(It.IsAny<int>())).Returns(true);
                //  Valid Datasource Id
            _mockDatasource.Setup(d => d.LoadSource(It.IsAny<string>())).Returns(false); //  Source loades OK

            _importService = new SnhImportService(_mockRepository.Object, _mockDataSourceRepository.Object,
                                                  _mockDatasource.Object, _mockAlgorithm.Object, _mockMapper.Object);

            //  Act
            var result = _importService.ImportData(1, "source");

            //  Assert
            Assert.IsFalse(result, "Expected the import routine to return FALSE.");
        }

        [TestMethod]
        [TestCategory("ImportServices")]
        public void SourceLoads_NoAggregates_ReturnsTrue()
        {
            //  Arrange
            _mockDataSourceRepository.Setup(s => s.IsValidDataSource(It.IsAny<int>())).Returns(true);       //  Valid Datasource Id
            _mockDatasource.Setup(d => d.LoadSource(It.IsAny<string>())).Returns(true);                     //  Source loads OK
            _mockDatasource.Setup(d => d.GetAll()).Returns(_importAggregates);                              //  Import Aggreates
            _mockRepository.Setup(r => r.GetAllSortedByName()).Returns(new Collection<Aggregate>());        //  Aggregates
            _mockAlgorithm.Setup(a => a.Match(It.IsAny<string>(), It.IsAny<IList<Aggregate>>())).Returns(1);//  algorithm returns a match
            _mockMapper.Setup(m => m.Map(It.IsAny<ImportAggregate>())).Returns(_aggregateAchany);           //  mapper class returns valid aggregate
            _mockRepository.Setup((r => r.Update(It.IsAny<Aggregate>())));                                  //  update database
            _mockRepository.Setup(r => r.Add(It.IsAny<Aggregate>())).Returns(1);                            //  Add Aggregate to database returns id,successful

            _importService = new SnhImportService(_mockRepository.Object, _mockDataSourceRepository.Object,
                                                  _mockDatasource.Object, _mockAlgorithm.Object, _mockMapper.Object);

            //  Act
            var result = _importService.ImportData(1, "source");

            //  Assert
            Assert.IsTrue(result, "Expected the import routine to return True.");
        }

        [TestMethod]
        [TestCategory("ImportServices")]
        public void SourceLoads_Aggregates_ReturnsTrue()
        {
            //  Arrange
            _mockDataSourceRepository.Setup(s => s.IsValidDataSource(It.IsAny<int>())).Returns(true);       //  Valid Datasource Id
            _mockDatasource.Setup(d => d.LoadSource(It.IsAny<string>())).Returns(true);                     //  Source loads OK
            _mockDatasource.Setup(d => d.GetAll()).Returns(_importAggregates);                              //  Import Aggreates
            _mockRepository.Setup(r => r.GetAllSortedByName()).Returns(_aggregates);                        //  Aggregates
            _mockAlgorithm.Setup(a => a.Match(It.IsAny<string>(), It.IsAny<IList<Aggregate>>())).Returns(1);//  algorithm returns a match
            _mockMapper.Setup(m => m.Map(It.IsAny<ImportAggregate>())).Returns(_aggregateAchany);           //  mapper class returns valid aggregate
            _mockRepository.Setup((r => r.Update(It.IsAny<Aggregate>())));                                  //  update database
            _mockRepository.Setup(r => r.Add(It.IsAny<Aggregate>())).Returns(1);                            //  Add Aggregate to database returns id,successful

            _importService = new SnhImportService(_mockRepository.Object, _mockDataSourceRepository.Object,
                                                  _mockDatasource.Object, _mockAlgorithm.Object, _mockMapper.Object);

            //  Act
            var result = _importService.ImportData(1, "source");

            //  Assert
            Assert.IsTrue(result, "Expected the import routine to return True.");
        }

    }
}
