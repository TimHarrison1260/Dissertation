using System.Collections.Generic;
using System.Text;
using Infrastructure.Interfaces.Algorithms;

namespace Infrastructure.Algorithms
{
    /// <summary>
    /// Class <c>PreProcessor</c> is responsible for 
    /// implementing the pre-processing for each of
    /// the algorithms used.  It contains the definition
    /// of the Domain Vocabulary: a list of reserved words.
    /// </summary>
    /// <remarks>
    /// It contains the definition of the Domain Vocabulary.  Future
    /// development would be to abstract this component and inject
    /// the implementation at run-time.
    /// </remarks>
    public class PreProcessor : IAlgorithmPreProcess
    {
        //  TODO: decide on the most appropriate generic collection type.  Perhaps a custom type.
        /// <summary>
        /// Hold a collection of the reserved words that make up the Domain Vocabulary.
        /// </summary>
        private readonly List<string> _reservedWords;

        /// <summary>
        /// Ctor: Initialise the Domain specific vocabulary of reserved words.
        /// </summary>
        public PreProcessor()
        {
            //  Initialise the reserved words list.
            _reservedWords = new List<string> 
                {
                    "extension", 
                    "ext", 
                    "phase", 
                    "wind", 
                    "farm", 
                    "windfarm", 
                    "turbine", 
                    "turbines", 
                    "resubmission", 
                    "community" ,
                    "estate",
                    "renewable",
                    "renewables",
                    "energy",
                    "park",
                    "project"
                };
            _reservedWords.Sort();      //  Use the default string comparer.
        }

        /// <summary>
        /// Applies the selected preprocessing to the specified string
        /// </summary>
        /// <param name="input">String to be processed</param>
        /// <param name="preProcessType">Type of PreProcess</param>
        /// <returns>Processed Input string</returns>
        public string ApplyPreProcess(string input, PreProcessTypesEnum preProcessType)
        {
            switch (preProcessType)
            {
                case PreProcessTypesEnum.RemoveReservedWords:
                    return Sanitise(input);
                case PreProcessTypesEnum.RemoveSpecialCharacters:
                    return Sanitise(input, true);
            }
            return string.Empty;
        }

        /// <summary>
        /// Cleans the input wind farm name by removing the Domain Specific 
        /// Vocabulary and optionally removing the special characters.
        /// </summary>
        /// <param name="input">String to be cleaned</param>
        /// <param name="removeSpecialCharacters">Indicates if special characters are to be removed</param>
        /// <returns>The cleaned wind farm name</returns>
        private string Sanitise(string input, bool removeSpecialCharacters = false)
        {
            var cleanedString = new StringBuilder();
            var noOfWordsInOutput = 0;
            //var words = input.Split(' ', '(', ')');
            var words = input.Split(' ');
            foreach (var word in words)
            {
                if (!_reservedWords.Contains(word.ToLower()))       //  check in reserved words list
                {
                    cleanedString.Append((!removeSpecialCharacters && noOfWordsInOutput > 0) ? " " + word : word);
                    noOfWordsInOutput++;
                }
            }
            return cleanedString.ToString();
        }
    }
}
