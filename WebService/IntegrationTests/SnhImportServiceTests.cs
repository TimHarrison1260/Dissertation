using System.Text;
using Core.Interfaces.Repositories;
using Infrastructure.Helpers;
using Infrastructure.Interfaces.Algorithms;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Mappers;
using Infrastructure.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Model;
using Infrastructure.Algorithms;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.ServiceModel;
using Infrastructure.Data;
using UnitTests.Helpers;

namespace IntegrationTests
{
    [TestClass]
    public class SnhImportServiceTests
    {
        //  
        private SnhImportService _service = null;
        private const int SnhSourceId = 1;

        private IUnitOfWork _db = null;

        private ISnhDataSource _datasource;                        //  SNH Import data
        private IAggregateRepository _repository;                  //  Repository to Aggregate model
        private IDataSourceRepository _dataSourceRepository;       //  Repository to DataSource model
        private IMatchingAlgorithm _algorithm;                     //  The string matching algorithm
        private ICoefficientAlgorithm _coefficientAlgorithm;
        private IStringSimilarityAlgorithm _similarityAlgorithm;
        private IEditDistanceAlgorithm _editDistanceAlgorithm;
        private IAlgorithmPreProcess _preProcess;
        private IMapper<ImportAggregate, Aggregate> _mapper; 

        private string _kml = string.Empty;         //  Use for testing

        private string _kmlJune = string.Empty;     //  To load from file only once
        private string _kmlOctober = string.Empty;  //  To load from file only once
        private string _invalidKml = string.Empty;  //  An invalid kml file, has kml tag but no kmlns attribute

        private readonly DataHelper _helper = new DataHelper();

        public SnhImportServiceTests()
        {
            _kmlJune = _helper.ReadJuneFile();
            _kmlOctober = _helper.ReadOctoberFile();
            _invalidKml = _helper.ReadInvalidTestFile();
        }


        [TestInitialize]
        public void Initialise()
        {
            _db = new AggregateContext();
            _repository = new AggregateRepository(_db);
            _dataSourceRepository = new DataSourceRepository(_db);

            var helper = new SnhKmlHelper();
            _datasource = new SnhDataSource(helper);

            _coefficientAlgorithm = new DiceCoefficient();
            _similarityAlgorithm = new LcSubstr();
            _editDistanceAlgorithm = new LevenshteinEditDistance();
            _preProcess = new PreProcessor();

            _algorithm = new MatchingAlgorithm(_coefficientAlgorithm, 0.9f, _similarityAlgorithm, 0.9f,
                                               _editDistanceAlgorithm, 2, _preProcess);

            _mapper = new ImportAggregateToAggregateMapper();

            _service = new SnhImportService(_repository, _dataSourceRepository, _datasource, _algorithm, _mapper);
        }

        [TestMethod]
        [TestCategory("SnhImportServiceIntegration")]
        public void ImportSnhOctoberFileOK()
        {
            //  Arrange
            //  Make sure we're getting a new instance initialised properly
            //  so that the ones loaded from files do not get changed.
            var bldr = new StringBuilder(_kmlOctober);
            _kml = bldr.ToString();

            //  Act
            var result = _service.ImportData(SnhSourceId, _kml);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("SnhImportServiceIntegration")]
        public void ImportSnhJuneFileOK()
        {
            //  Arrange
            //  Make sure we're getting a new instance initialised properly
            //  so that the ones loaded from files do not get changed.
            var bldr = new StringBuilder(_kmlJune);
            _kml = bldr.ToString();

            //  Act
            var result = _service.ImportData(SnhSourceId, _kml);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("SnhImportServiceIntegration")]
        public void ImportInvalidFileFail()
        {
            //  Arrange
            //  Make sure we're getting a new instance initialised properly
            //  so that the ones loaded from files do not get changed.
            var bldr = new StringBuilder(_invalidKml);
            _kml = bldr.ToString();

            //  Act
            var result = _service.ImportData(SnhSourceId, _kml);

            Assert.IsFalse(result);
        }



        [TestCleanup]
        public void Clean()
        {
            
        }

    }
}
