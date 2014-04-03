using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Helpers;

namespace UnitTests.RenUkImportTests
{
    [TestClass]
    public class RenUkHtmlHelperTests
    {
        //  Store the test HTML document.
        private readonly HtmlDocument _testDoc = new HtmlDocument();
        private readonly HtmlDocument _404Doc = new HtmlDocument();
        private readonly HtmlDocument _1SearchResult = new HtmlDocument();
        private readonly HtmlDocument _noSearchResults = new HtmlDocument();
        private readonly IList<HtmlNode> _aggregateNodes = new List<HtmlNode>();

        private DataHelper _testHelper = new DataHelper();      // loading test data files.

        private const string RubbishBaseUrl = @"http://www.arubbishurl.com/";

        private const string AchairnJson = "{\"Name\":\"Achairn Farm, Stirkoke\",\"Region\":\"Scotland\",\"Location\":\"Wick\",\"Turbines\":\"3\",\"ProjectCapacity\":\"6.15\",\"TurbineCapacity\":\"2.05\",\"Developer\":\"Innes (James & Ronald)\",\"StatusDate\":\"01-May-2009\",\"Status\":\"Operational\",\"ProjectType\":\"onshore\"}";
        private const string BarrJson = "{\"Name\":\"\",\"Region\":\"Scotland\",\"Location\":\"Westfield House\",\"Turbines\":\"1\",\"ProjectCapacity\":\"2\",\"TurbineCapacity\":\"2\",\"Developer\":\"Renewable Devices Group\",\"StatusDate\":\"21-Jul-2011\",\"Status\":\"Approved\",\"ProjectType\":\"onshore\"}";


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
        public RenUkHtmlHelperTests()
        {
            //  Load the test html document from a test file.
            _testDoc = _testHelper.ReadHtmlGood();
            _404Doc = _testHelper.ReadHtml404();
            _1SearchResult = _testHelper.ReadHtmlWithOneResult();
            _noSearchResults = _testHelper.ReadHtmlWithNoResults();

            //  Load the test Aggregate nodes
            var tmpAggregateNodes = new HtmlDocument();
            tmpAggregateNodes = _testHelper.ReadHtmlLiTags();
            var element = tmpAggregateNodes.GetElementbyId("searchresults");
            var nodes = element.Elements("li");
            foreach (var node in nodes)
            {
                _aggregateNodes.Add(node);
            }

        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void LoadHtmlPageSuccessfully()
        {
            //  Arrange
            var doc = _testDoc;
            var url = RubbishBaseUrl + "page/1/status/All/region/1094/";

            //  Prepare the output Html Document
            HtmlDocument outputDocument = null;

            using (ShimsContext.Create())
            {
                //  Intercept HtmlWeb object (calls the url to get the web site Html) using MS Shims
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) =>
                    {
                        return doc;
                    };
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web =>
                    {
                        return System.Net.HttpStatusCode.OK;
                    });

                //  Set upt the helper class
                var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

                //  Act
                var result = helper.LoadHtmlPage(out outputDocument, url, 1);

                //  Assert
                Assert.AreEqual(HttpStatusCode.OK, result, "Expected a successful load, return 'OK'");
                Assert.IsNotNull(outputDocument, "Expected the HtmlDocument to be loaded, instead it's NULL.");
            }
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void LoadHtmlPageUnsuccessful404NotFound()
        {
            //  Arrange
            var doc = _404Doc;
            var url = RubbishBaseUrl + "page/1/status/All/region/1094/";

            //  Prepare the output Html Document
            HtmlDocument outputDocument = null;

            using (ShimsContext.Create())
            {
                //  Intercept HtmlWeb object (calls the url to get the web site Html) using MS Shims
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) =>
                    {
                        return doc;
                    };
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web =>
                    {
                        return System.Net.HttpStatusCode.NotFound;
                    });

                //  Set up the helper class
                var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

                //  Act
                var result = helper.LoadHtmlPage(out outputDocument, url, 1);

                //  Assert
                Assert.AreEqual(HttpStatusCode.NotFound, result, "Expected a 404 not found to be returned");
                Assert.IsNotNull(outputDocument, "Expected the HtmlDocument to be a New instance, instead it's NULL.");
            }
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateNodesSuccessful()
        {
            //  Arrange
            var doc = _testDoc;

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetAggregateNodes(doc);

            //  Assert
            Assert.AreEqual(50, result.Count(), "Expected 50 aggregate nodes to be returned.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateNodesUnsuccessful404Document()
        {
            //  Arrange
            var doc = _404Doc;

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetAggregateNodes(doc);

            //  Assert
            Assert.AreEqual(0, result.Count(), "Expected 0 aggregate nodes to be returned, the site returned a 404: Not Found.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateNodesUnsuccessfulEmptyDocument()
        {
            //  Arrange
            var doc = new HtmlDocument();

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetAggregateNodes(doc);

            //  Assert
            Assert.AreEqual(0, result.Count(), "Expected 0 aggregate nodes to be returned, the site returned a 404: Not Found.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateNodesUnsuccessfulNullDocument()
        {
            //  Arrange
            HtmlDocument doc = null;

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetAggregateNodes(doc);

            //  Assert
            Assert.AreEqual(0, result.Count(), "Expected 0 aggregate nodes to be returned, the site returned a 404: Not Found.");
        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateNameSuccessful()
        {
            //  Arrange
            var achairn = _aggregateNodes[0];       // li 1 to 3 are OK, li 4 has no name
            var achany = _aggregateNodes[1];
            var achlachan = _aggregateNodes[2];

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var achairnResult = helper.GetAggregateName(achairn);
            var achanyResult = helper.GetAggregateName(achany);
            var achlachanResult = helper.GetAggregateName(achlachan);

            //  Assert
            Assert.AreEqual("Achairn Farm, Stirkoke", achairnResult, "Expected 'Achairn Farm, Stirkoke' as the name.");
            Assert.AreEqual("Achany Estate", achanyResult, "Expected 'Achany Estate' as the name.");
            Assert.AreEqual("Achlachan", achlachanResult, "Expected 'Achlachan' as the name.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateNameUnsuccessfulNoName()
        {
            //  Arrange
            var barr = _aggregateNodes[3];      // li number 4 has no name

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var barrResult = helper.GetAggregateName(barr);

            //  Assert
            Assert.AreEqual(string.Empty, barrResult, "Expected empty string as the name for 'barr' doesn't exist.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateColumnValuesSuccessful()
        {
            //  Arrange
            var achairn = _aggregateNodes[0];       // li 1 to 3 are OK, li 4 has no name

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var achairnResult = helper.GetAggregateColumnValues(achairn);

            //  Assert
            Assert.AreEqual(AchairnJson, achairnResult, "Expected a valid JSON object to be returned.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateColumnValuesSuccessfulUsingFakes()
        {
            //  Arrange
            var achairn = _aggregateNodes[0];       // li 1 to 3 are OK, li 4 has no name

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            using (ShimsContext.Create())
            {
                //  Intercept HtmlWeb object (calls the url to get the web site Html) using MS Shims
                Newtonsoft.Json.Fakes.ShimJsonConvert.SerializeObjectObject =
                    (jsonConvert) =>
                    {
                        return AchairnJson;
                    };

                //  Act
                var achairnResult = helper.GetAggregateColumnValues(achairn);

                //  Assert
                Assert.AreEqual(AchairnJson, achairnResult, "Expected a valid JSON object to be returned.");
            }

        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateColumnValuesNoName()
        {
            //  Arrange
            var barr = _aggregateNodes[3];       // li 1 to 3 are OK, li 4 has no name

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var barrResult = helper.GetAggregateColumnValues(barr);

            //  Assert
            Assert.AreEqual(BarrJson, barrResult, "Expected a valid JSON object to be returned.");        
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetAggregateColumnValuesNoNameUsingFakes()
        {
            //  Arrange
            var barr = _aggregateNodes[3];       // li 1 to 3 are OK, li 4 has no name

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            using (ShimsContext.Create())
            {
                //  Intercept HtmlWeb object (calls the url to get the web site Html) using MS Shims
                Newtonsoft.Json.Fakes.ShimJsonConvert.SerializeObjectObject =
                    (jsonConvert) =>
                    {
                        return BarrJson;
                    };

                //  Act
                var barrResult = helper.GetAggregateColumnValues(barr);

                //  Assert
                Assert.AreEqual(BarrJson, barrResult, "Expected a valid JSON object to be returned.");
            }
            
        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetTotalAggregatesAndPagesBothSuccessful()
        {
            //  Arrange
            var doc = _testDoc;
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetTotalAggregatesAndPages(doc);

            //  Assert
            Assert.AreEqual("522",result[0], "Expected 522 Aggregates in results");
            Assert.AreEqual("11", result[1], "Expected 11 pages in the results.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetTotalAggregatesAndPagesTotalAggregatesOnly()
        {
            //  Arrange
            var doc = _1SearchResult;
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetTotalAggregatesAndPages(doc);

            //  Assert
            Assert.AreEqual("1", result[0], "Expected 1 Aggregates in results");
            Assert.AreEqual("1", result[1], "Expected 1 page in the results.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetTotalAggregatesAndPagesNoValues()
        {
            //  Arrange
            var doc = _noSearchResults;
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetTotalAggregatesAndPages(doc);

            //  Assert
            Assert.AreEqual("0", result[0], "Expected zero Aggregates in results");
            Assert.AreEqual("0", result[1], "Expected zero pages in the results.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GenerateAggregateUrlTests()
        {
            //  Arrange
            var baseUrl = @"http://www.test.com/";
            var id = "Achany";
            var region = "99";

            string urlBaseNullIdNullRegionDefault = string.Empty;
            string urlBaseValueIdNullRegionDefault = string.Empty;
            string urlBaseValueIdValueRegionDefault = "{\"Url\": \"" + @"http://www.test.com//name/Achany/status/All/region/1094/" + "\"}";
            string urlBaseValueIdValueRegion99 = "{\"Url\": \"" + @"http://www.test.com//name/Achany/status/All/region/99/" + "\"}";
            string urlBaseEmptyIdEmptyRegionDefault = string.Empty;

            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result1 = helper.GenerateAggregateUrl(null, null);
            var result2 = helper.GenerateAggregateUrl(baseUrl, null);
            var result3 = helper.GenerateAggregateUrl(baseUrl, id);
            var result4 = helper.GenerateAggregateUrl(baseUrl, id, region);
            var result5 = helper.GenerateAggregateUrl(string.Empty, string.Empty);

            //  Assert
            Assert.AreEqual(urlBaseNullIdNullRegionDefault, result1, "Expected empty string as both parameters invalid (null).");
            Assert.AreEqual(urlBaseValueIdNullRegionDefault, result2, "Expected empty string as AggregateId parameter invalid (null).");
            Assert.AreEqual(urlBaseValueIdValueRegionDefault, result3, "Expected '{\"Url\": \"http://www.test.com//name/Achany/status/All/region/1094/\"}' as both parameters OK and region defaulted to 1094.");
            Assert.AreEqual(urlBaseValueIdValueRegion99, result4, "Expected '{\"Url\": \"http://www.test.com//name/Achany/status/All/region/99/\"}' as both parameters OK and region 99.");
            Assert.AreEqual(urlBaseEmptyIdEmptyRegionDefault, result5, "Expected empty string as both parameters invalid (empty strings).");
        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetColumnHeadersSuccessful()
        {
            //  Arrange
            var doc = _testDoc;
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetColumnHeadings(doc);

            //  Assert
            Assert.AreEqual(10, result.Count(), "Expected 10 strings to be returned: 10 column headers");
            Assert.AreEqual("Wind Project", result[0], "Expected 'Wind Project' as the column 1 heading.");
            Assert.AreEqual("Region", result[1], "Expected 'Region' as the column 1 heading.");
            Assert.AreEqual("Location", result[2], "Expected 'Location' as the column 1 heading.");
            Assert.AreEqual("Turbines", result[3], "Expected 'Turbines' as the column 1 heading.");
            Assert.AreEqual("Project Capacity (MW)", result[4], "Expected 'Project Capacity (MW)' as the column 1 heading.");
            Assert.AreEqual("Turbine Capacity (MW)", result[5], "Expected 'Turbine Capacity (MW)' as the column 1 heading.");
            Assert.AreEqual("Developer", result[6], "Expected 'Developer' as the column 1 heading.");
            Assert.AreEqual("Current Status Date", result[7], "Expected 'Current Status Date</' as the column 1 heading.");
            Assert.AreEqual("Status of Project", result[8], "Expected 'Status of Project' as the column 1 heading.");
            Assert.AreEqual("Type of Project", result[9], "Expected 'Type of Project' as the column 1 heading.");
        }

        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetColumnHeadersNone()
        {
            //  Arrange
            var doc = _noSearchResults;
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetColumnHeadings(doc);

            //  Assert
            Assert.AreEqual(1, result.Count(), "Expected only 1 string to be returned: No headers");
            Assert.AreEqual(string.Empty, result[0], "Expected empty string in the results.");
        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetUrlFromJsonObject_Successful()
        {
            //  Arrange
            var jsonObject = "{\"Url\":\"The External Url\"}";
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();
            
            //  Act
            var result = helper.GetUrlFromJsonObject(jsonObject);

            //  Assert
            Assert.AreEqual("The External Url", result, "Expected 'The External Url' to be returned.");
        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetUrlFromJsonObject_InvlidObject_Unsuccessful()
        {
            //  Arrange
            var jsonObject = "{\"NotUrl\":\"The External Url\"}";
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetUrlFromJsonObject(jsonObject);

            //  Assert
            Assert.IsNull(result, "Expected an null reference to be returned.");
        }


        [TestMethod]
        [TestCategory("RenUkHtmlHelper")]
        public void GetUrlFromJsonObject_EmptyString_Unsuccessful()
        {
            //  Arrange
            var jsonObject = string.Empty;
            var helper = new Infrastructure.Helpers.RenUkHtmlHelper();

            //  Act
            var result = helper.GetUrlFromJsonObject(jsonObject);

            //  Assert
            Assert.IsNull(result, "Expected an null reference to be returned.");
        }



    }
}