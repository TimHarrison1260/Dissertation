using System.Linq;
using System.Runtime.Remoting;
using System.Xml;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Model;
using Infrastructure.Algorithms;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Interfaces.Algorithms;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Mappers;
using Infrastructure.Interfaces.Services;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using Infrastructure.ServiceModel;
using Infrastructure.Services;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HtmlAgilityPack;
using UnitTests.Helpers;

namespace IntegrationTests
{
    [TestClass]
    public class DataServiceIntegrationTests
    {
        private HtmlDocument _htmlDoc;
        private HtmlDocument _htmlSingleAggregateDoc;

        private IUnitOfWork _db;
        private IAggregateRepository _repository;                  //  Repository to Aggregate model
        private IDataSourceRepository _dataSourceRepository;       //  Repository to DataSource model
        private IDataTypeRepository _dataTypeRepository;           //   Repositlry to DataTypes
        
        private IRenUkDataSource _renUkDatasource;                 //  RenUk Datasource
        private ISnhDataSource _snhDataSource;                     //  SNH Datasource

        private IDataSourceResolver _dataSourceResolver;                      //  Datasource Resolver
        private IImportServiceResolver _importServiceResolver;      //  Import service resolver

        private IMatchingAlgorithm _algorithm;                     //  The string matching algorithm
        private ICoefficientAlgorithm _coefficientAlgorithm;
        private IStringSimilarityAlgorithm _similarityAlgorithm;
        private IEditDistanceAlgorithm _editDistanceAlgorithm;
        private IAlgorithmPreProcess _preProcess;

        private IMapper<ImportAggregate, Aggregate> _mapper;

        private IRenUkImportService _renUkImportService;
        private ISnhImportService _snhImportService;

        //  Test  Helper class, used to load HTML from text files in UnitTests/Data folder.
        private DataHelper _testHelper = new DataHelper();

        private const string baseUrl = @"http://www.renewableuk.com/en/renewable-energy/wind-energy/uk-wind-energy-database/index.cfm/";

        private const int SnhDataSourceId = 1;
        private const int RenUkDatasourceId = 2;
        private const int NonExistentDataSource = 99;
        private const int AikengallAggregate = 2;
        private const int NonExistentAggregate = 9999;

        public DataServiceIntegrationTests()
        {
            //_htmlDoc = _testHelper.ReadHtmlPartialPage();
            _htmlDoc = _testHelper.ReadHtml1FullPage();
            _htmlSingleAggregateDoc = _testHelper.ReadHtmlWithOneResult();
        }


        [TestInitialize]
        public void Initialise()
        {
            //  Data Context and Repositories
            _db = new AggregateContext();
            _repository = new AggregateRepository(_db);
            _dataSourceRepository = new DataSourceRepository(_db);
            _dataTypeRepository = new DataTypeRepository(_db);     //  No parm, currently Enumeration
            //  DataSources and Resolver
            var renUkHelper = new RenUkHtmlHelper();
            _renUkDatasource = new RenUkDataSource(renUkHelper);
            var snhHelper = new SnhKmlHelper();
            _snhDataSource = new SnhDataSource(snhHelper);
            _dataSourceResolver = new DataSourceResolver(_snhDataSource, _renUkDatasource);
            //  Matching algorithm
            _coefficientAlgorithm = new DiceCoefficient();
            _similarityAlgorithm = new LcSubstr();
            _editDistanceAlgorithm = new LevenshteinEditDistance();
            _preProcess = new PreProcessor();
            _algorithm = new MatchingAlgorithm(_coefficientAlgorithm, 0.9f, _similarityAlgorithm, 0.9f,
                                               _editDistanceAlgorithm, 2, _preProcess);
            //  Mapper class
            _mapper = new ImportAggregateToAggregateMapper();
            //  Import Services and Resolver
            _renUkImportService = new RenUkImportService(_repository, _dataSourceRepository, _renUkDatasource, _algorithm, _mapper);
            _snhImportService = new SnhImportService(_repository, _dataSourceRepository, _snhDataSource, _algorithm,
                _mapper);
            _importServiceResolver = new ImportServiceResolver(_snhImportService, _renUkImportService);
        }


        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void ImportRenUk_Successful()
        {
            //  Arrange
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository,_importServiceResolver, _dataSourceResolver);

            var inputDoc = _htmlDoc;

            using (ShimsContext.Create())
            {
                //  Fake the call to the internet
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) => { return inputDoc; };
                //  Fake the statue code returned from HTTP response
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web => { return System.Net.HttpStatusCode.OK; });

                //  Act
                var result = dataService.ImportDataSource(2, baseUrl);

                //  Assert
                Assert.IsTrue(result, "Expected a successful import to return True.");
            }

        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void ImportRenUk_BadStatusHTTP_Unsuccessful()
        {
            //  Arrange
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            var inputDoc = _htmlDoc;

            using (ShimsContext.Create())
            {
                //  Fake the call to the internet
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) => { return inputDoc; };
                //  Fake the statue code returned from HTTP response
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web => { return System.Net.HttpStatusCode.NotFound; });

                //  Act
                var result = dataService.ImportDataSource(2, baseUrl);

                //  Assert
                Assert.IsFalse(result, "Expected an unsuccessful import to return False.");
            }

        }


        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GeAlltDataSources_Successful()
        {
            //  Arrange: 
            //  Accessing data direct from data store, so keep to base data.
            //  Always check for the minimum test data, as other data could be 
            //  there because of the imports, so do not check for any cases
            //  where the import data could be included.  Don't check for things
            //  like import or updated dates as these cannot be guaranteed and 
            //  result in a false negative test.
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Act
            var result = dataService.GeDataSources().ToArray();
            var minDatasources = (result.Count() >= 2) ? true : false;

            //  Assert
            Assert.IsNotNull(result, "Expected some datasources to be returned.");
            Assert.IsTrue(minDatasources, "Expected at least 2 datasources to be returned 'SNH & RenUK'.");
            Assert.AreEqual("Scottish Natural Heritage KML Source", result[0].Title, "Expected 'SNH' as the first datasource");
            Assert.AreEqual("RenewableUk WebSite", result[1].Title, "Expected 'RenUk' as the second datasource");
        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetSNHDataSource_Successful()
        {
            //  Arrange: 
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Act
            var result = dataService.GetDataSource(SnhDataSourceId);

            //  Assert
            Assert.IsNotNull(result, "Expected 'SNH' datasource to be returned.");
            Assert.AreEqual("Scottish Natural Heritage KML Source", result.Title, "Expected 'SNH' as the first datasource");
        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetInvalidDataSource_Unsuccessful()
        {
            //  Arrange: 
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Act
            var result = dataService.GetDataSource(NonExistentDataSource);

            //  Assert
            Assert.IsNull(result, "Expected no datasource to be returned.");
        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetDataTypes_Successful()
        {
            //  Arrange: 
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Act
            var result = dataService.GetDataTypes().ToArray();

            //  Assert
            var minCount = result.Count() >= 2;
            Assert.IsTrue(minCount, "Expected at least 2 data types to be returned, 'Status' and 'Turbine'");

            var containsStatus = result.Contains("Status");
            var containsTurbine = result.Contains("Turbine");

            Assert.IsTrue(containsStatus, "Expected 'Status' to be in the returned datatypes.");
            Assert.IsTrue(containsTurbine, "Expected 'turbine' to be in the returned datatypes.");
        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetAggregates_Successful()
        {
            //  Arrange: 
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Act
            var result = dataService.GetAggregates(string.Empty).ToArray();     // Get all aggregates

            //  Assert
            var minCount = result.Count() >= 10;
            Assert.IsTrue(minCount, "Expected at least 10 aggregates to be returned");

        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetAggregate_Aikengall_Successful()
        {
            //  Arrange: 
            var inputDoc = _htmlSingleAggregateDoc;

            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Mock the external call to the internet.
            using (ShimsContext.Create())
            {
                //  Fake the call to the internet
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) => { return inputDoc; };
                //  Fake the statue code returned from HTTP response
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web => { return System.Net.HttpStatusCode.OK; });

            //  Act
            var result = dataService.GetAggregate(AikengallAggregate);

            //  Assert
            Assert.IsNotNull(result, "Expected aggregate 'Aikengall' to be returned");
            Assert.AreEqual("Aikengall", result.Name, "Expected Name to be 'Aikengall'.");
            }

        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetAggregate_NonExistantAggregate_Unsuccessful()
        {
            //  Arrange: 
            var inputDoc = _htmlSingleAggregateDoc;

            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Mock the external call to the internet.
            using (ShimsContext.Create())
            {
                //  Fake the call to the internet
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) => { return inputDoc; };
                //  Fake the statue code returned from HTTP response
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web => { return System.Net.HttpStatusCode.OK; });

                //  Act
                var result = dataService.GetAggregate(NonExistentAggregate);     

                //  Assert
                Assert.IsNull(result, "Expected no aggregate to be returned");
            }

        }


        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetAggregatesWithDataType_Status_Successful()
        {
            //  Arrange: 
            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Act
            var result = dataService.GetAggregatesWithDataType(DataTypeEnum.Status).ToArray();     // Get all aggregates

            //  Assert
            var minCount = result.Count() >= 1; //  Aikengall can guarantee a Status segment
            Assert.IsTrue(minCount, "Expected at least 1 aggregates to be returned");

        }

        [TestMethod]
        [TestCategory("DataService Integration Tests")]
        public void GetAggregateWithDataType_Aikengall_Status_Successful()
        {
            //  Arrange: 
            var inputDoc = _htmlSingleAggregateDoc;

            var dataService = new DataService(_repository, _dataSourceRepository, _dataTypeRepository, _importServiceResolver, _dataSourceResolver);

            //  Mock the external call to the internet.
            using (ShimsContext.Create())
            {
                //  Fake the call to the internet
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.LoadString =
                    (htmlWeb, s) => { return inputDoc; };
                //  Fake the statue code returned from HTTP response
                HtmlAgilityPack.Fakes.ShimHtmlWeb.AllInstances.StatusCodeGet =
                    (web => { return System.Net.HttpStatusCode.OK; });

                //  Act
                var result = dataService.GetAggregateWithDataType(AikengallAggregate,DataTypeEnum.Status);

                //  Assert
                var resultData = result.Data.ToArray();
                var minsegments = (resultData.Count() >= 1);

                Assert.IsNotNull(result, "Expected aggregate 'Aikengall' to be returned");
                Assert.AreEqual("Aikengall", result.Name, "Expected Name to be 'Aikengall'.");
                Assert.IsTrue(minsegments, "Expected at least one data segment 'Status' to be returned.");

                Assert.AreEqual(DataTypeEnum.Status, resultData[0].DataType, "Expected the first data segment to be a 'Turbine' segment.");
                Assert.AreEqual(DataTypeEnum.Turbine, resultData[1].DataType, "Expected the second data segment to be a 'Status' segment.");
            }

        }

    }
}
