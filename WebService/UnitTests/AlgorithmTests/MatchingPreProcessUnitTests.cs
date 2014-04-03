using Infrastructure.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AlgorithmTests
{
    [TestClass]
    public class MatchingPreProcessUnitTests
    {
        [TestMethod]
        [TestCategory("MatchingAlgorithmPreProcessor")]
        public void RemoveReservedWordsNoneToRemove()
        {
            //  Arrange
            var inString = "Neilston";
            var outString = "Neilston";

            var preProcessor = new PreProcessor();

            //  Act
            var result = preProcessor.ApplyPreProcess(inString, PreProcessTypesEnum.RemoveReservedWords);

            //  Assert
            Assert.AreEqual(outString, result, "Expectd no reserved words to be removed leaving Neilston.");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithmPreProcessor")]
        public void RemoveReservedWordsPartName()
        {
            //  Arrange
            var inString = "Neilston Community Wind Farm";
            var outString = "Neilston";

            var preProcessor = new PreProcessor();

            //  Act
            var result = preProcessor.ApplyPreProcess(inString, PreProcessTypesEnum.RemoveReservedWords);

            //  Assert
            Assert.AreEqual(outString, result, "Expectd Community, Wind and Farm to be removed leaving Neilston.");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithmPreProcessor")]
        public void RemoveReservedWordsCompleteName()
        {
            //  Arrange
            var inString = "Community Wind Farm";
            var outString = "";

            var preProcessor = new PreProcessor();

            //  Act
            var result = preProcessor.ApplyPreProcess(inString, PreProcessTypesEnum.RemoveReservedWords);

            //  Assert
            Assert.AreEqual(outString, result, "Expectd Community, Wind and Farm to be removed leaving empty string.");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithmPreProcessor")]
        public void RemoveSpecialCharactersSomeToRemove()
        {
            //  Arrange
            var inString = "There are some spaces in this";
            var outString = "Therearesomespacesinthis";

            var preProcessor = new PreProcessor();

            //  Act
            var result = preProcessor.ApplyPreProcess(inString, PreProcessTypesEnum.RemoveSpecialCharacters);

            //  Assert
            Assert.AreEqual(outString, result, "Expectd all spaces to be removed leaving 'Therearesomespacesinthis'.");
        }

        [TestMethod]
        [TestCategory("MatchingAlgorithmPreProcessor")]
        public void RemoveSpecialCharactersNoneToRemove()
        {
            //  Arrange
            var inString = "ThereAreNoSpacesInThis";
            var outString = "ThereAreNoSpacesInThis";

            var preProcessor = new PreProcessor();

            //  Act
            var result = preProcessor.ApplyPreProcess(inString, PreProcessTypesEnum.RemoveReservedWords);

            //  Assert
            Assert.AreEqual(outString, result, "Expectd nothing to be removed leaving 'ThereAreNoSpacesInThis'.");
        }
    }
}
