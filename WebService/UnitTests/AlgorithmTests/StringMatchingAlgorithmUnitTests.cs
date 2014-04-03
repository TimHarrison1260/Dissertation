using System;
using Infrastructure.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AlgorithmTests
{
    [TestClass]
    public class StringMatchingAlgorithmUnitTests
    {
        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void DicesCoefficientforExactMatch()
        {
            //  Arrange
            var str1 = "Cathkin Braes";
            var str2 = "Cathkin Braes";

            var algorithm = new DicesCoefficient();

            //  Act
            var result = Math.Round(algorithm.Match(str1, str2), 3);

            //  Assert
            Assert.AreEqual(1,result, "Expected an exact match, should return 1");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void DicesCoefficientSimilarityMatch()
        {
            //  Arrange
            var str1 = "Hadyard Hill";
            var str2 = "Hadyard Hill, Barr";

            var algorithm = new DicesCoefficient();

            //  Act
            var result = Math.Round(algorithm.Match(str1, str2),3);

            //  Assert
            Assert.AreEqual(0.818, result, "Expected an similarity match of 0.8 (81.8%)");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void DicesCoefficientNoMatch()
        {
            //  Arrange
            var str1 = "Achany Estate";
            var str2 = "Aikengall";

            var algorithm = new DicesCoefficient();

            //  Act
            var result = Math.Round(algorithm.Match(str1, str2), 3);

            //  Assert
            Assert.AreEqual(0.000, result, "Expected an No match 0.000 (0%)");
        }
        
        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void LcsPercentMinExactMatch()
        {
            //  Arrange
            var str1 = "Cathkin Braes";
            var str2 = "Cathkin Braes";

            var algorithm = new LcSubstr();

            //  Act
            var result = Math.Round(algorithm.Match(str1, str2), 3);

            //  Assert
            Assert.AreEqual(1, result, "Expected an exact match, should return 1");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void LcsPercentMinSimilarityMatch()
        {
            //  Arrange
            var str1 = "Hadyard Hill";
            var str2 = "Hadyard Hill, Barr";

            var algorithm = new LcSubstr();

            //  Act
            var result = Math.Round(algorithm.Match(str1, str2), 3);

            //  Assert
            Assert.AreEqual(1.0, result, "Expected an similarity match of 1.0 (100%) as Hadyard Hill is the longest common substring and is the same as the shortest input string");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void LcsPercentMinNoMatch()
        {
            //  Arrange
            var str1 = "Achany Estate";
            var str2 = "Aikengall";

            var algorithm = new LcSubstr();

            //  Act
            var result = Math.Round(algorithm.Match(str1, str2), 3);

            //  Assert
            Assert.AreEqual(0.111, result, "Expected an No match 0.111 (11.1%), because LCS is length 1 which is 11.1% of the input string 'aikengall'");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void LedExactMatch()
        {
            //  Arrange
            var str1 = "Cathkin Braes";
            var str2 = "Cathkin Braes";

            var algorithm = new LevenshteinEditDistance();

            //  Act
            var result = algorithm.Match(str1, str2);

            //  Assert
            Assert.AreEqual(0, result, "Expected an exact match, should return 0");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void LedSimilarityMatch()
        {
            //  Arrange
            var str1 = "Hadyard Hill";
            var str2 = "Hadyard Hill, Barr";

            var algorithm = new LevenshteinEditDistance();

            //  Act
            var result = algorithm.Match(str1, str2);

            //  Assert
            Assert.AreEqual(6, result, "Expected a similarity match, should be an edit distance of 6");
        }

        [TestMethod]
        [TestCategory("StringSimilarityAlgorithms")]
        public void LedNoMatch()
        {
            //  Arrange
            var str1 = "Achany Estate";
            var str2 = "Aikengall";

            var algorithm = new LevenshteinEditDistance();

            //  Act
            var result = algorithm.Match(str1, str2);

            //  Assert
            Assert.AreEqual(10, result, "Expected a similarity match, should be an edit distance of 10");
        }
    }
}
