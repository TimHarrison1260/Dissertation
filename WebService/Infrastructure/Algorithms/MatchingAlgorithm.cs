using System;
using System.Collections.Generic;
using Core.Model;
using Infrastructure.Interfaces.Algorithms;

namespace Infrastructure.Algorithms
{
    /// <summary>
    /// Class <c>MatchingAlborithm</c> is responsible for implementing
    /// the complete matching process required by the aggregation.
    /// It comprises the 3 algorithms identified during testing:
    /// 1-Dice's Coefficient, 2-LongestCommonSubstringPercentage, 
    /// 3-Levenshtein Edit Distance.
    /// </summary>
    public class MatchingAlgorithm: IMatchingAlgorithm
    {
        private readonly double _coefficientLimit;  //  Match limit for Dice's Coefficient
        private readonly double _percentageLimit;   //  Match limit for Longest Common Substring Percentage Minimum
        private readonly int _editDistanceLimit;    //  Match limit for Levenshtein Edit distance

        private readonly ICoefficientAlgorithm _coefficientAlgorithm;
        private readonly IStringSimilarityAlgorithm _similarityAlgorithm;
        private readonly IEditDistanceAlgorithm _editDistanceAlgorithm;
        private readonly IAlgorithmPreProcess _preProcessor;

        //  The list of aggregate names the imported aggregates are being matched with.
        //private IList<Core.Model.Aggregate> _aggregates = null;


        public MatchingAlgorithm(ICoefficientAlgorithm coefficientAlgorithm, double coefficientLimit,
                                 IStringSimilarityAlgorithm similarityMatchAlgorithm, Double percentageLimit,
                                 IEditDistanceAlgorithm editDistanceAlgorithm, int editDistanceLimit,
                                 IAlgorithmPreProcess preProcessor)
        {
            if (coefficientAlgorithm == null)
                throw new ArgumentNullException("coefficientAlgorithm", "No valid Coefficient type algorithm supplied");
            if (similarityMatchAlgorithm == null)
                throw new ArgumentNullException("similarityMatchAlgorithm", "No valid Percentage type algorithm supplied");
            if (editDistanceAlgorithm == null)
                throw new ArgumentNullException("editDistanceAlgorithm", "No valid Edit Distance type algorithm supplied");
            if (preProcessor == null)
                throw new ArgumentNullException("preProcessor", "No valid pre-processor supplied");

            _coefficientAlgorithm = coefficientAlgorithm;
            _similarityAlgorithm = similarityMatchAlgorithm;
            _editDistanceAlgorithm = editDistanceAlgorithm;
            _preProcessor = preProcessor;

            _coefficientLimit = coefficientLimit;
            _percentageLimit = percentageLimit;
            _editDistanceLimit = editDistanceLimit;
        }


        /// <summary>
        /// Matches the two supplied strings and determines if they
        /// match based upon the specific algorithms used by the
        /// process
        /// </summary>
        /// <param name="source">The Name to be matched with one already aggregated</param>
        /// <param name="aggregates">The list of aggregated data</param>
        /// <returns>Returns id of the matching aggregate otherwise -1: no match found</returns>
        public int Match(string source, IList<Aggregate> aggregates)
        {
            var result = -1;
            Aggregate matchedAggregate = null;

            //  Check if the new string matches with any items already aggregated
            foreach (var aggregate in aggregates)
            {
                if (MatchStrings(source, aggregate.Name))
                {
                    result = aggregate.Id;
                    matchedAggregate = aggregate;
                    break;
                }
            }

            //  Remove the matched record to avoid matching with it again, and improve performance as the 
            //  imports progress
            //  TODO: refactor this when the additional processing is implemented to deal with multiple matches
            if (matchedAggregate != null)
            {
                var matchedIndex = aggregates.IndexOf(matchedAggregate);
                aggregates.RemoveAt(matchedIndex);
            }

            //  Return the Id of the matched aggregate
            return result;
        }

        /// <summary>
        /// Matches the two supplied strings and determines if they
        /// match based upon the specific algorithms used by the
        /// process
        /// </summary>
        /// <param name="source">The source Name</param>
        /// <param name="target">The Target Name</param>
        /// <returns>True if they match, otherwise False</returns>
        private bool MatchStrings(string source, string target)
        {
            //  Pre-processing: Remove the reserved words.
            var cleanedSource = _preProcessor.ApplyPreProcess(source, PreProcessTypesEnum.RemoveReservedWords);
            var cleanedTarget = _preProcessor.ApplyPreProcess(target, PreProcessTypesEnum.RemoveReservedWords);
            //  Match using the 1st algorithm: Dice's Coefficient
            var dc = _coefficientAlgorithm.Match(cleanedSource, cleanedTarget);
            if (dc > _coefficientLimit)
                return true;

            //  Pre-processing: Remove the special characters.
            var cleanedSource2 = _preProcessor.ApplyPreProcess(cleanedSource, PreProcessTypesEnum.RemoveSpecialCharacters);
            var cleanedTarget2 = _preProcessor.ApplyPreProcess(cleanedTarget, PreProcessTypesEnum.RemoveSpecialCharacters);
            var lcsPMin = _similarityAlgorithm.Match(cleanedSource2, cleanedTarget2);
            if (lcsPMin > _percentageLimit)
                return true;

            //  Pre-processing: None, use the values that were passed in.
            var led = _editDistanceAlgorithm.Match(source, target);
            if (led <= _editDistanceLimit)
                return true;

            //  No matches,
            return false;
        }
    }
}
