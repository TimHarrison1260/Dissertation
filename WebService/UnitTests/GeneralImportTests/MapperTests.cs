using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Model;
using Infrastructure.Mappers;
using Infrastructure.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.GeneralImportTests
{
    [TestClass]
    public class MapperTests
    {
        private readonly DateTime _timeStamp = DateTime.Now;

        [TestMethod]
        [TestCategory("MapperClasses")]
        public void MapImportToAggregateClass_AggregateWithNoData_OK()
        {
            //  Arrange:    Create an Import class & instantiate the class
            var importAggregate = new ImportAggregate()
            {
                Identifier = "NoDataAggregate",
                ImportDate = _timeStamp,
                SourceId = 1,
                Data = new List<ImportData>()
            };

            var mapper = new ImportAggregateToAggregateMapper();

            //  Act:        Call the map method
            var result = mapper.Map(importAggregate);

            //  Assert
            Assert.AreEqual(0, result.Id, "Expected aggregate to have no Id, ie new aggregate");
            Assert.AreEqual("NoDataAggregate", result.Name, "Expected the aggregate to be called 'NoDataAggregate");
            Assert.AreEqual(_timeStamp, result.LastUpdated, string.Format("Expected last updated to be set to {0}", _timeStamp));
            Assert.AreEqual(0, result.Data.Count, "Expected there to be an empty data collection");
        }

        [TestMethod]
        [TestCategory("MapperClasses")]
        public void MapImportToAggregateClass_AggregateWithSingleData_OK()
        {
            //  Arrange:    Create an Import class & instantiate the class
            var importAggregate = new ImportAggregate()
            {
                Identifier = "FootPrintAggregate",
                ImportDate = _timeStamp,
                SourceId = 1,
                Data = new List<ImportData>()
                {
                    new ImportData()
                    {
                        Data = "{test data}",
                        DataType = DataTypeEnum.FootPrint
                    }
                }
            };

            var mapper = new ImportAggregateToAggregateMapper();

            //  Act:        Call the map method
            var result = mapper.Map(importAggregate);
            var resultData = result.Data.ToArray();

            //  Assert
            Assert.AreEqual(0, result.Id, "Expected aggregate to have no Id, ie new aggregate");
            Assert.AreEqual("FootPrintAggregate", result.Name, "Expected the aggregate to be called 'FootprintAggregate");
            Assert.AreEqual(_timeStamp, result.LastUpdated, string.Format("Expected last updated to be set to {0}", _timeStamp));
            Assert.AreEqual(1, result.Data.Count, "Expected there to be 1 entry in the data collection");

            Assert.AreEqual(0, resultData[0].AggregateId, "Expected the data to have no aggregate Id either.");
            Assert.AreEqual(1, resultData[0].DataSourceId, "Expected a DataSourceid of 1");
            Assert.AreEqual(0, resultData[0].AggregateId, "Expected the aggregateId not to be set, ie 0 for a new aggregate");
            Assert.AreEqual("FootPrintAggregate", resultData[0].Name, "Expected the data.AggregateId to be 'FootprintAggregate'");
            Assert.AreEqual("{test data}", resultData[0].Data, "Expected '{test data}' to be set.");
            Assert.AreEqual(_timeStamp, resultData[0].LastUpdated, string.Format("Expected last updated to be set to '{0}'", _timeStamp));
            Assert.AreEqual(DataTypeEnum.FootPrint, resultData[0].DataType, string.Format("Expected the datatype to be '{0}'", DataTypeEnum.FootPrint));
        }

        [TestMethod]
        [TestCategory("MapperClasses")]
        public void MapImportToAggregateClass_AggregateWithMultipleData_OK()
        {
            //  Arrange:    Create an Import class & instantiate the class
            var importAggregate = new ImportAggregate()
            {
                Identifier = "FootPrintAggregate",
                ImportDate = _timeStamp,
                SourceId = 1,
                Data = new List<ImportData>()
                {
                    new ImportData()
                    {
                        Data = "{test data}",
                        DataType = DataTypeEnum.FootPrint
                    },
                    new ImportData()
                    {
                        Data = "{test data 2}",
                        DataType = DataTypeEnum.Style
                    },
                }
            };

            var mapper = new ImportAggregateToAggregateMapper();

            //  Act:        Call the map method
            var result = mapper.Map(importAggregate);
            var resultData = result.Data.ToArray();

            //  Assert
            Assert.AreEqual(0, result.Id, "Expected aggregate to have no Id, ie new aggregate");
            Assert.AreEqual("FootPrintAggregate", result.Name, "Expected the aggregate to be called 'FootprintAggregate");
            Assert.AreEqual(_timeStamp, result.LastUpdated, string.Format("Expected last updated to be set to {0}", _timeStamp));
            Assert.AreEqual(2, result.Data.Count, "Expected there to be 2 entry in the data collection");

            Assert.AreEqual(0, resultData[0].AggregateId, "Expected the data to have no aggregate Id either.");
            Assert.AreEqual(1, resultData[0].DataSourceId, "Expected a DataSourceid of 1");
            Assert.AreEqual(0, resultData[0].AggregateId, "Expected the aggregateId not to be set, ie 0 for a new aggregate");
            Assert.AreEqual("FootPrintAggregate", resultData[0].Name, "Expected the data.AggregateId to be 'FootprintAggregate'");
            Assert.AreEqual("{test data}", resultData[0].Data, "Expected '{test data}' to be set.");
            Assert.AreEqual(_timeStamp, resultData[0].LastUpdated, string.Format("Expected last updated to be set to '{0}'", _timeStamp));
            Assert.AreEqual(DataTypeEnum.FootPrint, resultData[0].DataType, string.Format("Expected the datatype to be '{0}'", DataTypeEnum.FootPrint));

            Assert.AreEqual(0, resultData[1].AggregateId, "Expected the data to have no aggregate Id either.");
            Assert.AreEqual(1, resultData[1].DataSourceId, "Expected a DataSourceid of 1");
            Assert.AreEqual(0, resultData[1].AggregateId, "Expected the aggregateId not to be set, ie 0 for a new aggregate");
            Assert.AreEqual("FootPrintAggregate", resultData[1].Name, "Expected the data.AggregateId to be 'FootprintAggregate'");
            Assert.AreEqual("{test data 2}", resultData[1].Data, "Expected '{test data 2}' to be set.");
            Assert.AreEqual(_timeStamp, resultData[1].LastUpdated, string.Format("Expected last updated to be set to '{0}'", _timeStamp));
            Assert.AreEqual(DataTypeEnum.Style, resultData[1].DataType, string.Format("Expected the datatype to be '{0}'", DataTypeEnum.Style));
        }

        [TestMethod]
        [TestCategory("MapperClasses")]
        public void MapImportToAggregateClass_InitialisedAggregate_ReturnsEmptyAggregate()
        {
            //  Arrange:    Create an Import class & instantiate the class
            var importAggregate = new ImportAggregate()
            {
                Identifier = string.Empty,
                ImportDate = new DateTime(),
                SourceId = 0,
                Data = new Collection<ImportData>()
            };

            var mapper = new ImportAggregateToAggregateMapper();

            //  Act:        Call the map method
            var result = mapper.Map(importAggregate);

            //  Assert
            Assert.AreEqual(0, result.Id, "Expected aggregate to have no Id, ie new aggregate");
            Assert.AreEqual(string.Empty, result.Name, "Expected the aggregate to have no Name");
            Assert.AreEqual(new DateTime(), result.LastUpdated, string.Format("Expected last updated to be set to {0}", _timeStamp));
            Assert.AreEqual(0, result.Data.Count, "Expected there to be an empty data collection");
        }

        [TestMethod]
        [TestCategory("MapperClasses")]
        public void MapImportToAggregateClass_UnInitialisedAggregate_ReturnsEmptyAggregate()
        {
            //  Arrange:    Create an Import class & instantiate the class
            var importAggregate = new ImportAggregate();

            var mapper = new ImportAggregateToAggregateMapper();

            //  Act:        Call the map method
            var result = mapper.Map(importAggregate);

            //  Assert
            Assert.AreEqual(0, result.Id, "Expected aggregate to have no Id, ie new aggregate");
            Assert.AreEqual(string.Empty, result.Name, "Expected the aggregate to have no Name");
            Assert.AreEqual(new DateTime(), result.LastUpdated, string.Format("Expected last updated to be set to {0}", _timeStamp));
            Assert.AreEqual(0, result.Data.Count, "Expected there to be an empty data collection");
        }

        [TestMethod]
        [TestCategory("MapperClasses")]
        public void MapImportToAggregateClass_Null_ReturnsEmptyAggregate()
        {
            //  Arrange:    Create an Import class & instantiate the class
            ImportAggregate importAggregate = null;

            var mapper = new ImportAggregateToAggregateMapper();

            //  Act:        Call the map method
            var result = mapper.Map(importAggregate);

            //  Assert
            Assert.AreEqual(0, result.Id, "Expected aggregate to have no Id, ie new aggregate");
            Assert.AreEqual(string.Empty, result.Name, "Expected the aggregate to have no Name");
            Assert.AreEqual(new DateTime(), result.LastUpdated, string.Format("Expected last updated to be set to {0}", _timeStamp));
            Assert.AreEqual(0, result.Data.Count, "Expected there to be an empty data collection");
        }
    }
}
