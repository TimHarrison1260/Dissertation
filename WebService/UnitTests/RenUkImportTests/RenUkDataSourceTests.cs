using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Core.Model;
using HtmlAgilityPack;
using Infrastructure.Data;
using Infrastructure.Interfaces.Helpers;
using Infrastructure.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.RenUkImportTests
{
    [TestClass]
    public class RenUkDataSourceTests
    {
        //  Store the test HTML document.
        private readonly HtmlDocument _testDoc = new HtmlDocument();
        private readonly HtmlDocument _404Doc = new HtmlDocument();
        private readonly IList<HtmlNode> _aggregateNodes = new List<HtmlNode>();
        private HtmlNode[] _aggregateNodesArray;
        private string[] _aggregateNamesArray;

        private IList<ImportAggregate> _aggregateData;

        //  Location of the RenUk testdata (Source of page 1 from the web site)
        private const string FileName = @"..\..\Data\RenUkPage1Test.html";
        private const string NotFound404FileName = @"..\..\Data\RenUk404NotFound.html";
        private const string LifileName = @"..\..\Data\RenUkPage1AllLiTags.html";

        //  Url
        private const string baseUrl = @"http://www.renewableuk.com/en/renewable-energy/wind-energy/uk-wind-energy-database/index.cfm/";
        private const string rubbishBaseUrl = @"http://www.arubbishurl.com/";

        //  Test wind farm names
        private const string AchairnFarm = "Achairn Farm, Stirkoke";
        private const string AchanyEstate = "Achany Estate";
        private const string Achlachan = "Achlachan";


        /// <summary>
        /// Ctor: load the test documents from the datafiles.  
        /// </summary>
        /// <remarks>
        /// Load files in constructor to avoid having to do this
        /// more than once for the batch of tests.  All tests
        /// only read the data so no conflicts will occur
        /// between them, making the tests atomic.  A TestInitialise
        /// method would execute for every single test, not wanted
        /// for performance reasons.
        /// </remarks>
        public RenUkDataSourceTests()
        {
            //  Load the test html document from a test file.
            _testDoc = new HtmlDocument();
            _testDoc.Load(FileName);
            _404Doc = new HtmlDocument();
            _404Doc.Load(NotFound404FileName);

            //  Load the test Aggregate nodes
            var tmpAggregateNodes = new HtmlDocument();
            tmpAggregateNodes.Load(LifileName);
            var element = tmpAggregateNodes.GetElementbyId("searchresults");
            var nodes = element.Elements("li");

            _aggregateNodesArray = new HtmlNode[3];
            var i = 0;
            foreach (var node in nodes)
            {
                _aggregateNodes.Add(node);
                _aggregateNodesArray[i] = node;
                i++;
            }
            //  Load the corresponding Names array
            _aggregateNamesArray = new string[3];
            _aggregateNamesArray[0] = AchairnFarm;
            _aggregateNamesArray[1] = AchanyEstate;
            _aggregateNamesArray[2] = Achlachan;

            //  Load the corresponding ImportAggregate collection, out of alphabetic sequence
            //  to test the ordering of data.
            var importDate = new DateTime(2014, 02, 19, 9, 10, 30, 0);
            _aggregateData = new List<ImportAggregate>()
            {
                new ImportAggregate()
                {
                    Identifier = AchanyEstate,
                    ImportDate = importDate,
                    SourceId = 2,
                    Data = new Collection<ImportData>()
                    {
                        new ImportData()
                        {
                            DataType = DataTypeEnum.Statistics,
                            Data = rubbishBaseUrl + "/name/" + AchanyEstate + "/status/all/region/1094"
                        }
                    }
                },
                new ImportAggregate()
                {
                    Identifier = AchairnFarm,
                    ImportDate = importDate,
                    SourceId = 2,
                    Data = new Collection<ImportData>()
                    {
                        new ImportData()
                        {
                            DataType = DataTypeEnum.Statistics,
                            Data = rubbishBaseUrl + "/name/" + AchairnFarm + "/status/all/region/1094"
                        }
                    }
                },
                new ImportAggregate()
                {
                    Identifier = Achlachan,
                    ImportDate = importDate,
                    SourceId = 2,
                    Data = new Collection<ImportData>()
                    {
                        new ImportData()
                        {
                            DataType = DataTypeEnum.Statistics,
                            Data = rubbishBaseUrl + "/name/" + Achlachan + "/status/all/region/1094"
                        }
                    }
                }
            };

        }



        [TestInitialize]
        public void Initilise(){}


        /// <summary>
        /// Test for a successful load of the data source, from a valid document
        /// </summary>
        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void LoadSource_Successfully()
        {
            var document = _testDoc;
            
            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();
            mockHelper.Setup(l => l.LoadHtmlPage(out document, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(HttpStatusCode.OK);
            mockHelper.Setup(a => a.GetTotalAggregatesAndPages(It.IsAny<HtmlDocument>())).Returns(new[] {"3", "1"});
            mockHelper.Setup(n => n.GetAggregateNodes(It.IsAny<HtmlDocument>())).Returns(_aggregateNodes);
            mockHelper.Setup(m => m.GetAggregateName(_aggregateNodesArray[0])).Returns(_aggregateNamesArray[0]);
            mockHelper.Setup(m => m.GetAggregateName(_aggregateNodesArray[1])).Returns(_aggregateNamesArray[1]);
            mockHelper.Setup(m => m.GetAggregateName(_aggregateNodesArray[2])).Returns(_aggregateNamesArray[2]);
            mockHelper.Setup(u => u.GenerateAggregateUrl(It.IsAny<string>(), _aggregateNamesArray[0], It.IsAny<string>()))
                .Returns(rubbishBaseUrl + "name/" + _aggregateNamesArray[0] + "/status/all/region/1094/");
            mockHelper.Setup(u => u.GenerateAggregateUrl(It.IsAny<string>(), _aggregateNamesArray[1], It.IsAny<string>()))
                .Returns(rubbishBaseUrl + "name/" + _aggregateNamesArray[1] + "/status/all/region/1094/");
            mockHelper.Setup(u => u.GenerateAggregateUrl(It.IsAny<string>(), _aggregateNamesArray[2], It.IsAny<string>()))
                .Returns(rubbishBaseUrl + "name/" + _aggregateNamesArray[2] + "/status/all/region/1094/");

            //  Instantiate the datasource class
            var dataSource = new RenUkDataSource(mockHelper.Object);

            //  Act
            var result = dataSource.LoadSource(rubbishBaseUrl);

            //  Assert
            Assert.IsTrue(result, "Expected a successful load of the 3 aggregates, should have returned 'OK'");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void LoadSource_NotFound_Unsuccessful()
        {
            var document = _404Doc;

            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();
            mockHelper.Setup(l => l.LoadHtmlPage(out document, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(HttpStatusCode.NotFound);

            //  Instantiate the datasource class
            var dataSource = new RenUkDataSource(mockHelper.Object);

            //  Act
            var result = dataSource.LoadSource(rubbishBaseUrl);

            //  Assert
            Assert.IsFalse(result, "Expected a successful load of the 3 aggregates, should have returned 'OK'");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void LoadSource_InvalidSource_Unsuccessful()
        {
            var document = new HtmlDocument();

            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();
            mockHelper.Setup(l => l.LoadHtmlPage(out document, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(HttpStatusCode.NoContent);

            //  Instantiate the datasource class
            var dataSource = new RenUkDataSource(mockHelper.Object);

            //  Act
            var result = dataSource.LoadSource(rubbishBaseUrl);

            //  Assert
            Assert.IsFalse(result, "Expected a successful load of the 3 aggregates, should have returned 'OK'");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void GetAllNames_Successful()
        {
            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();

            var dataSource = new RenUkDataSource(mockHelper.Object);
            dataSource.SetData(_aggregateData);     // call public method, not on I/F, to set the data for testing

            //  Act
            var result = dataSource.GetAllNames().ToArray();

            //  Assert
            Assert.AreEqual(3, result.Count(), "Expected the names of 3 import aggregates to be returned.");
            Assert.AreEqual(AchairnFarm, result[0], string.Format("Expected first aggregate name to be {0}", AchairnFarm));
            Assert.AreEqual(AchanyEstate, result[1], string.Format("Expected first aggregate name to be {0}", AchanyEstate));
            Assert.AreEqual(Achlachan, result[2], string.Format("Expected first aggregate name to be {0}", Achlachan));
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void GetAllNames_None_Unsuccessful()
        {
            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();

            var dataSource = new RenUkDataSource(mockHelper.Object);
            dataSource.SetData(new List<ImportAggregate>());     // call public method, not on I/F, to set the data for testing

            //  Act
            var result = dataSource.GetAllNames().ToArray();

            //  Assert
            Assert.AreEqual(0, result.Count(), "Expected Zero import aggregates to be returned.");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void GetAll_Successful()
        {
            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();

            var dataSource = new RenUkDataSource(mockHelper.Object);
            dataSource.SetData(_aggregateData);     // call public method, not on I/F, to set the data for testing

            //  Act
            var result = dataSource.GetAll().ToArray();

            //  Assert
            Assert.AreEqual(3, result.Count(), "Expected the names of 3 import aggregates to be returned.");
            Assert.AreEqual(AchairnFarm, result[0].Identifier, string.Format("Expected first aggregate name to be {0}", AchairnFarm));
            Assert.AreEqual(AchanyEstate, result[1].Identifier, string.Format("Expected first aggregate name to be {0}", AchanyEstate));
            Assert.AreEqual(Achlachan, result[2].Identifier, string.Format("Expected first aggregate name to be {0}", Achlachan));
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void GetAll_NoAggregates_Unsuccessful()
        {
            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();

            var dataSource = new RenUkDataSource(mockHelper.Object);
            dataSource.SetData(new List<ImportAggregate>());     // call public method, not on I/F, to set the data for testing

            //  Act
            var result = dataSource.GetAll().ToArray();

            //  Assert
            Assert.AreEqual(0, result.Count(), "Expected Zero import aggregates to be returned.");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void Get_Successful()
        {
            var document = _testDoc;

            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();
            mockHelper.Setup(l => l.LoadHtmlPage(out document, It.IsAny<string>()))
                .Returns(HttpStatusCode.OK);
            mockHelper.Setup(n => n.GetAggregateNodes(It.IsAny<HtmlDocument>())).Returns(_aggregateNodes);
            mockHelper.Setup(m => m.GetAggregateName(_aggregateNodesArray[0])).Returns(_aggregateNamesArray[0]);
            mockHelper.Setup(v => v.GetAggregateColumnValues(It.IsAny<HtmlNode>()))
                .Returns("A JSON data object");
            mockHelper.Setup(u => u.GetUrlFromJsonObject(It.IsAny<string>())).Returns("An External URL");

            //  Instantiate the datasource class
            var dataSource = new RenUkDataSource(mockHelper.Object);

            //  Act
            var result = dataSource.Get(rubbishBaseUrl);

            var dataout = result.Data.First().Data;

            //  Assert
            Assert.AreEqual(_aggregateData[1].Identifier, result.Identifier, "Expected Achairn to be returned ");
            Assert.AreEqual("A JSON data object", dataout, "Expected the 'JSON object to be returned in the data");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void Get_UnSuccessful_HttpStatusCode_NotFound()
        {
            var document = _testDoc;

            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();
            mockHelper.Setup(l => l.LoadHtmlPage(out document, It.IsAny<string>()))
                .Returns(HttpStatusCode.NotFound);

            //  Instantiate the datasource class
            var dataSource = new RenUkDataSource(mockHelper.Object);

            //  Act
            var result = dataSource.Get(rubbishBaseUrl);

            //  Assert
            Assert.IsNull(result, "Expected 'null' to be returned ");
            //Assert.IsNull(result.Identifier, "Expected 'empty' object to be returned ");
        }

        [TestMethod]
        [TestCategory("RenUkDataSource")]
        public void Get_Unsuccessful_HttpOK_NoAggregates()
        {
            var document = _testDoc;

            //  Arrange
            var mockHelper = new Mock<IRenUkHtmlHelper>();
            mockHelper.Setup(l => l.LoadHtmlPage(out document, It.IsAny<string>()))
                .Returns(HttpStatusCode.OK);
            mockHelper.Setup(n => n.GetAggregateNodes(It.IsAny<HtmlDocument>())).Returns(new Collection<HtmlNode>());

            //  Instantiate the datasource class
            var dataSource = new RenUkDataSource(mockHelper.Object);

            //  Act
            var result = dataSource.Get(rubbishBaseUrl);

            //  Assert
            Assert.IsNull(result, "Expected 'null' to be returned ");
            //Assert.IsNull(result.Identifier, "Expected 'empty' object to be returned ");
        }

    }
}
