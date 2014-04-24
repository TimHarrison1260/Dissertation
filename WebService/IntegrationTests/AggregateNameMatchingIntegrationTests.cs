using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Algorithms;
using Infrastructure.Interfaces.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Model;

namespace IntegrationTests
{
    [TestClass]
    public class AggregateNameMatchingIntegrationTests
    {

        private ICoefficientAlgorithm _coefficientAlgorithm;
        private IStringSimilarityAlgorithm _similarityAlgorithm;
        private IEditDistanceAlgorithm _editDistanceAlgorithm;
        private IAlgorithmPreProcess _preProcessor;

        private const double CoefficientLimit = 0.9f;
        private const double SimilarityLimit = 0.9f;
        private const int EditDistanceLimit = 2;

        private IMatchingAlgorithm _algorithm;

        private IList<Aggregate> _aggregates;


        [TestInitialize]
        public void Initialise()
        {
            //  Create new instance of the algorithms and pre-processor
            _coefficientAlgorithm = new DiceCoefficient();
            _similarityAlgorithm = new LcSubstr();
            _editDistanceAlgorithm = new LevenshteinEditDistance();
            _preProcessor = new PreProcessor();

            //  Instantiate the Aggregate Matching algorithm
            _algorithm = new MatchingAlgorithm(_coefficientAlgorithm, CoefficientLimit,
                                                _similarityAlgorithm, SimilarityLimit,
                                                _editDistanceAlgorithm, EditDistanceLimit,
                                                _preProcessor);

            //  Set up the test aggregates
            _aggregates = new List<Aggregate>()
                {
                    new Aggregate()
                        {
                            Id = 1,
                            Name = "Ardrossan",
                            LastUpdated = DateTime.Now,
                            Data = new Collection<AggregateData>()
                        },
                    new Aggregate()
                        {
                            Id=2,
                            Name = "Ardrossan Extension",
                            LastUpdated = DateTime.Now,
                            Data=new Collection<AggregateData>()
                        },
                    new Aggregate()
                        {
                            Id = 3,
                            Name = "10 x 50kW Windbank turbines at Tambowie Farm, Milngavie",
                            LastUpdated = DateTime.Now,
                            Data = new Collection<AggregateData>()
                        },
                    new Aggregate()
                        {
                            Id = 4,
                            Name = "Cathkin Braes",
                            LastUpdated = DateTime.Now,
                            Data = new Collection<AggregateData>()
                        },
                    new Aggregate()
                        {
                            Id = 5,
                            Name = "Achany Estate",
                            LastUpdated = DateTime.Now,
                            Data = new Collection<AggregateData>()
                        }
                };
        }


        [TestMethod]
        [TestCategory("MatchingProcessIntegration")]
        public void AggregateMatchExactSingleWordNameReturnIdOf1()
        {
            //  Arrange
            var inString = "Ardrossan";

            //  Act
            var result = _algorithm.Match(inString, _aggregates);

            //  Assert
            Assert.AreEqual(1, result, "Expected to find aggregate with id of 1");
        }

        [TestMethod]
        [TestCategory("MatchingProcessIntegration")]
        public void AggregateMatchExactDoubleWordNameReturnIdOf2()
        {
            //  Arrange
            var inString = "Ardrossan Extension";

            //  Act
            var result = _algorithm.Match(inString, _aggregates);

            //  Assert
            Assert.AreEqual(1, result, "Expected to find aggregate with id of 2, but finds aggregate id 1 'ardrossan' which is contained within 'ardrossan extention'.  This is the multiple matching problem");
        }

        [TestMethod]
        [TestCategory("MatchingProcessIntegration")]
        public void AggregateMatchExactMultipleWordNameReturnIdOf3()
        {
            //  Arrange
            var inString = "10 x 50kW Windbank turbines at Tambowie Farm, Milngavie";

            //  Act
            var result = _algorithm.Match(inString, _aggregates);

            //  Assert
            Assert.AreEqual(3, result, "Expected to find aggregate with id of 3: '10 x 50kW Windbank turbines at Tambowie Farm, Milngavie'");
        }

        [TestMethod]
        [TestCategory("MatchingProcessIntegration")]
        public void AggregateMatchSimilarityMultipleWordNameReturnIdOf3()
        {
            //  Arrange
            var inString = "Tambowie Farm";

            //  Act
            var result = _algorithm.Match(inString, _aggregates);

            //  Assert
            Assert.AreEqual(3, result, "Expected to find aggregate with id of 3: '10 x 50kW Windbank turbines at Tambowie Farm, Milngavie'");
        }

        [TestMethod]
        [TestCategory("MatchingProcessIntegration")]
        public void AggregateMatchSimilarityDoubleWordNameReturnIdOf4()
        {
            //  Arrange
            var inString = "Cathekin Brase";

            //  Act
            var result = _algorithm.Match(inString, _aggregates);

            //  Assert
            Assert.AreEqual(4, result, "Expected to find aggregate with id of 4: 'Cathkin Braes'.  2 transforms are allowed in the Edit Distance algorithm.");
        }

        [TestMethod]
        [TestCategory("MatchingProcessIntegration")]
        public void AggregateMatchNoMatchDoubleWordNameReturnIdOfMinus1()
        {
            //  Arrange
            var inString = "Aikengall";

            //  Act
            var result = _algorithm.Match(inString, _aggregates);

            //  Assert
            Assert.AreEqual(-1, result, "Expected to find no matching aggregate; -1 should be returned from algorithm");
        }

    }
}
