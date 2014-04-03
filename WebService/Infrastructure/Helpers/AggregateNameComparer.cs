using System;
using System.Collections.Generic;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Class <c>AggregateNameComparer</c> is defined to implement
    /// the IComparer(T) interface and provide a means of sorting
    /// the Aggregate by Name, for use within the Linq queries.
    /// It allows the Aggregates to be sorted using the 
    /// string.Ordinal option
    /// </summary>
    public class AggregateNameComparer : IComparer<String>
    {
        /// <summary>
        /// General implementation of IComparer interface
        /// </summary>
        /// <param name="x">String 1 </param>
        /// <param name="y">String 2</param>
        /// <returns>integer indication the ordinality of the two strings</returns>
        /// <remarks>
        /// This is implemented for completeness, but Linq, which is the real 
        /// motivation for creating the class, requires the Explicit implementation.
        /// </remarks>
        public int Compare(string x, string y)
        {
            var result = String.Compare(x, y, StringComparison.Ordinal);
            return result;
        }

        /// <summary>
        /// Explicit implementation of IComparer interface
        /// </summary>
        /// <param name="x">String 1 </param>
        /// <param name="y">String 2</param>
        /// <returns>integer indication the ordinality of the two strings</returns>
        /// <remarks>
        /// Linq requires the Explicit implementation otherwise it doesn't find the implementation.
        /// </remarks>
        int IComparer<string>.Compare(string x, string y)
        {
            var result = String.Compare(x, y, StringComparison.Ordinal);
            return result;
        }
    }
}
