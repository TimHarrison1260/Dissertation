using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Interfaces;
using Core.Model;
using Infrastructure.Algorithms;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Data;
using UnitTests.Helpers;

namespace IntegrationTests
{
    [TestClass]
    public class SnhDataServiceTests
    {
        //  
        private SnhImportService _service = null;
        private const int SnhSourceId = 1;

        private IUnitOfWork _db = null;

        private ISnhDataSource _datasource;                        //  SNH Import data
        private IAggregateRepository<Aggregate> _repository;       //  Repository to Aggregate model
        private IDataSourceRepository _dataSourceRepository;       //  Repository to DataSource model
        private IMatchingAlgorithm _algorithm;                     //  The string matching algorithm
        private ICoefficientAlgorithm _coefficientAlgorithm;
        private IStringSimilarityAlgorithm _similarityAlgorithm;
        private IEditDistanceAlgorithm _editDistanceAlgorithm;
        private IAlgorithmPreProcess _preProcess;

        private string _kml = string.Empty;         //  Use for testing

        private string _kmlJune = string.Empty;     //  To load from file only once
        private string _kmlOctober = string.Empty;  //  To load from file only once
        private string _invalidKml = string.Empty;  //  An invalid kml file, has kml tag but no kmlns attribute

        private readonly DataHelper _helper = new DataHelper();

        public SnhDataServiceTests()
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

            _datasource = new SnhDataSource();

            _coefficientAlgorithm = new DicesCoefficient();
            _similarityAlgorithm = new LcSubstr();
            _editDistanceAlgorithm = new LevenshteinEditDistance();
            _preProcess = new PreProcessor();

            _algorithm = new MatchingAlgorithm(_coefficientAlgorithm, 0.9f, _similarityAlgorithm, 0.9f,
                                               _editDistanceAlgorithm, 2, _preProcess);

            _service = new SnhImportService(_repository, _dataSourceRepository, _datasource, _algorithm);
        }

        [TestMethod]
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
