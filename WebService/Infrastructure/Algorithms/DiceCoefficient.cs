using System.Collections.Generic;
using System.Text.RegularExpressions;
using Infrastructure.Interfaces.Algorithms;

namespace Infrastructure.Algorithms
{
    /// <summary>
    /// This routine is an iimplementation of the Dice's Coefficient Algorithm
    /// It works by comparing adjacent pairs of characters after the whitespace
    /// has been removed.  The coefficient represents a percentage match computed
    /// as 2 x the common pairs from the two lists of pairs / total number of pairs from both lists
    /// </summary>
    /// <remarks>
    /// This is lifted from the following article by Simon Whites blog 
    /// "How to Strike a Match".  http://www.catalysoft.com/articles/StrikeAMatch.html.
    /// 
    /// The actual implementation below is the Java routine converted to C# by various
    /// bloggers on the StackOverflow posting "A better similarity ranking algorithm for variable length strings",
    /// which can be found here: http://stackoverflow.com/questions/653157/a-better-similarity-ranking-algorithm-for-variable-length-strings
    /// specifically the C# solution posted by Michael La Voie.
    /// </remarks>
    public class DiceCoefficient : ICoefficientAlgorithm   // IAlgorithm<double>
    {
        /// <summary>
        /// Returns the Disce's Coefficient of the strings that match
        /// </summary>
        /// <param name="str1">String 1</param>
        /// <param name="str2">String 2</param>
        /// <returns>The percentage match from 0.0 to 1.0 where 1.0 is 100%</returns>
        public double Match(string str1, string str2)
        {
            var result = CompareStrings(str1, str2);
            return result;
        }

        /// <summary>
        /// Compares the two strings based on letter pair matches
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns>The percentage match from 0.0 to 1.0 where 1.0 is 100%</returns>
        private double CompareStrings(string str1, string str2)
        {
            var pairs1 = WordLetterPairs(str1.ToUpper());
            var pairs2 = WordLetterPairs(str2.ToUpper());

            var intersection = 0;
            var union = pairs1.Count + pairs2.Count;

            foreach (var t in pairs1)
            {
                for (var j = 0; j < pairs2.Count; j++)
                {
                    if (t == pairs2[j])
                    {
                        intersection++;
                        pairs2.RemoveAt(j);//Must remove the match to prevent "GGGG" from appearing to match "GG" with 100% success

                        break;
                    }
                }
            }

            return (2.0 * intersection) / union;
        }

        /// <summary>
        /// Gets all letter pairs for each
        /// individual word in the string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> WordLetterPairs(string str)
        {
            var allPairs = new List<string>();

            // Tokenize the string and put the tokens/words into an array
            var words = Regex.Split(str, @"\s");

            // For each word
            foreach (var t in words)
            {
                if (string.IsNullOrEmpty(t)) continue;
                // Find the pairs of characters
                var pairsInWord = LetterPairs(t);

                allPairs.AddRange(pairsInWord);
            }

            return allPairs;
        }

        /// <summary>
        /// Generates an array containing every 
        /// two consecutive letters in the input string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static IEnumerable<string> LetterPairs(string str)
        {
            var numPairs = str.Length - 1;

            var pairs = new string[numPairs];

            for (var i = 0; i < numPairs; i++)
            {
                pairs[i] = str.Substring(i, 2);
            }

            return pairs;
        }



    }
}
