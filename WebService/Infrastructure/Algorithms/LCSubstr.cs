using System;
using System.Collections.ObjectModel;
using System.Linq;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Algorithms;

namespace Infrastructure.Algorithms
{
    public class LcSubstr : IStringSimilarityAlgorithm   // IAlgorithm<double>
    {
        /// <summary>
        /// This matches the two strings and determines the length of the
        /// longest common substring as a percentage of the shortest of the
        /// two strings.
        /// </summary>
        /// <param name="s1">string 1</param>
        /// <param name="s2">string 2</param>
        /// <returns>A percentage</returns>
        public double Match(string s1, string s2)
        {
            var results = LongestCommonSubstring(s1.ToLower(), s2.ToLower());

            //  Results can potentially contain more than one string of the longest length
            //  Here, only the first one is used, as there is most likely to be only one 
            //  anyway, to calculate the percentage.
            var lcs = (results.Count() > 0) ? results[0] : string.Empty;

            //  Calculate the length of the lcs as a fration of the shortest string
            var lcsPMin = Convert.ToDouble(lcs.Length) /
                          Math.Min(Convert.ToDouble(s1.Length),
                                   Convert.ToDouble(s2.Length));
            return lcsPMin;
        }

        /// <summary>
        /// Longest Coommon Substring stgorithm
        /// </summary>
        /// <param name="S">string 1</param>
        /// <param name="T">string 2</param>
        /// <returns>array of strings being the longest ones found</returns>
        private static string[] LongestCommonSubstring(string S, string T)
        {
            var m = S.Length;
            var n = T.Length;

            var max = 0;
            var L = new int[m,n];
            var ret = new Collection<string>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (S[i] == T[j]) //  Compare the characters: equal
                    {
                        if (i == 0 || j == 0) //  Start of a row or a column
                        {
                            L[i, j] = 1;
                        }
                        else
                        {
                            L[i, j] = L[i - 1, j - 1] + 1;
                        }
                        //  Check if new maximum length
                        if (L[i, j] > max)
                        {
                            //  Increase the max found
                            max = L[i, j];

                            //  Initialise return array and add new string
                            ret = new Collection<string>();
                            var newMaxStr = S.Substring(i - max + 1, max);
                            ret.Add(newMaxStr);
                        }
                        else
                        {
                            if (L[i, j] == max)
                            {
                                //  Add maximum string to the output
                                var newMaxStr = S.Substring(i - max + 1, max);
                                ret.Add(newMaxStr);
                            }
                        }
                    }
                    else
                    {
                        L[i, j] = 0;
                    }
                }
            }
            return ret.ToArray();

        }

    }
}
