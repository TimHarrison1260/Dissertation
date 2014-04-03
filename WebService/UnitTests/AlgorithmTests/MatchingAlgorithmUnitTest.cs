using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Model;
using Infrastructure.Algorithms;
using Infrastructure.Interfaces.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.AlgorithmTests
{
    [TestClass]
    public class MatchingAlgorithmUnitTest
    {
        private Mock<ICoefficientAlgorithm> _mockCoefficientAlgorithm;
        private Mock<IStringSimilarityAlgorithm> _mockSimilarityAlgorithm;
        private Mock<IEditDistanceAlgorithm> _mockEditDistanceAlgorithm;
        private Mock<IAlgorithmPreProcess> _mockPreProcessor;

        private const double CoefficientLimit = 0.9f;
        private const double SimilarityLimit = 0.9f;
        private const int EditDistanceLimit = 2;

        private IList<Aggregate> _aggregates;
        private const string SearchValue1 = "Test1";
        private const string SearchValue2 = "Test2";
            
        [TestInitialize]
        public void Initialise()
        {
            //  Mock the algorithms and pre-processor
            _mockCoefficientAlgorithm = new Mock<ICoefficientAlgorithm>();
            _mockSimilarityAlgorithm = new Mock<IStringSimilarityAlgorithm>();
            _mockEditDistanceAlgorithm = new Mock<IEditDistanceAlgorithm>();
            _mockPreProcessor = new Mock<IAlgorithmPreProcess>();

            //  Set up the test aggregates
            _aggregates = new List<Aggregate>()
                {
                    new Aggregate()
                        {
                            Id = 1,
                            Name = "Test1",
                            LastUpdated = DateTime.Now,
                            Data = new Collection<AggregateData>()
                        },
                    new Aggregate()
                        {
                            Id=2,
                            Name = "Test2",
                            LastUpdated = DateTime.Now,
                            Data=new Collection<AggregateData>()
                        }
                };
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchWithDiceCoefficientReturnsAggregateNumber1()
        {
            //  Arrange
            var searchValue = "Test1";

            //  Set up the mock conditions: Dice return 1 for Test1, first case so no need to set up other return values
            _mockCoefficientAlgorithm.Setup(r => r.Match(searchValue,It.IsAny<string>())).Returns(1.0f);

            _mockSimilarityAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f); //  No Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(10); //  No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit, 
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(1, result, "Expected the match to return the first instance: Aggregate Id of 1");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchWithDiceCoefficientReturnsAggregateNumber2()
        {
            //  Arrange
            var searchValue = "Test2";

            //  Set up the mock conditions: Dice returns 1 for "Test2 to show an exact match
            _mockCoefficientAlgorithm.Setup(r => r.Match(searchValue, "Test1")).Returns(0.0f);      //  No Match
            _mockCoefficientAlgorithm.Setup(r => r.Match(searchValue, "Test2")).Returns(1.0f);      //  Match

            _mockSimilarityAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f); //  No Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(10); //  No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit,
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(2, result, "Expected the match to return the first instance: Aggregate Id of 2");
        }


        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchWithSubstringReturnsAggregateNumber1()
        {
            //  Arrange
            var searchValue = "Test1";

            //  Set up the mock conditions: Dice return 1 for Test1, first case so no need to set up other return values
            _mockCoefficientAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f);    // No Match

            _mockSimilarityAlgorithm.Setup(r => r.Match(searchValue, "Test1")).Returns(1.0f); //  Match
            _mockSimilarityAlgorithm.Setup(r => r.Match(searchValue, "Test2")).Returns(0.0f); //  No Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(10); //  No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit,
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(1, result, "Expected the match to return the first instance: Aggregate Id of 1");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchWithSubstringReturnsAggregateNumber2()
        {
            //  Arrange
            var searchValue = "Test1";

            //  Set up the mock conditions: Dice return 1 for Test1, first case so no need to set up other return values
            _mockCoefficientAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f);    // No Match

            _mockSimilarityAlgorithm.Setup(r => r.Match(searchValue, "Test1")).Returns(0.0f); //  No Match
            _mockSimilarityAlgorithm.Setup(r => r.Match(searchValue, "Test2")).Returns(1.0f); //  Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(10); //  No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit,
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(2, result, "Expected the match to return the first instance: Aggregate Id of 2");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchWithEditDistanceReturnsAggregateNumber1()
        {
            //  Arrange
            var searchValue = "Test1";

            //  Set up the mock conditions: Dice return 1 for Test1, first case so no need to set up other return values
            _mockCoefficientAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f);    // No Match

            _mockSimilarityAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f); //  No Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(searchValue, "Test1")).Returns(0); //  Match
            _mockEditDistanceAlgorithm.Setup(r => r.Match(searchValue, "Test2")).Returns(10); // No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit,
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(1, result, "Expected the match to return the first instance: Aggregate Id of 1");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchWithEditDistanceReturnsAggregateNumber2()
        {
            //  Arrange
            var searchValue = "Test2";

            //  Set up the mock conditions: Dice return 1 for Test1, first case so no need to set up other return values
            _mockCoefficientAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f);    // No Match

            _mockSimilarityAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f); //  No Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(searchValue, "Test1")).Returns(10); //  Match
            _mockEditDistanceAlgorithm.Setup(r => r.Match(searchValue, "Test2")).Returns(0); // No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit,
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(2, result, "Expected the match to return the first instance: Aggregate Id of 2");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithm")]
        public void MatchAlgorithmNoMatchFound()
        {
            //  Arrange
            var searchValue = "Test3";

            //  Set up the mock conditions: Dice return 1 for Test1, first case so no need to set up other return values
            _mockCoefficientAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f);    // No Match

            _mockSimilarityAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(0.0f); //  No Match

            _mockEditDistanceAlgorithm.Setup(r => r.Match(It.IsAny<string>(), It.IsAny<string>())).Returns(10); //  No Match

            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test1", It.IsAny<PreProcessTypesEnum>())).Returns("Test1");
            _mockPreProcessor.Setup(p => p.ApplyPreProcess("Test2", It.IsAny<PreProcessTypesEnum>())).Returns("Test2");

            var algorithm = new MatchingAlgorithm(_mockCoefficientAlgorithm.Object, CoefficientLimit,
                                                  _mockSimilarityAlgorithm.Object, SimilarityLimit,
                                                  _mockEditDistanceAlgorithm.Object, EditDistanceLimit,
                                                  _mockPreProcessor.Object);

            //  Act
            var result = algorithm.Match(searchValue, _aggregates);

            //  Assert
            Assert.AreEqual(-1, result, "Expected the match to return -1, no match found");
        }

    }
}
