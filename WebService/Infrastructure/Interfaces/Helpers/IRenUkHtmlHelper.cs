using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using Infrastructure.Data;

namespace Infrastructure.Interfaces.Helpers
{
    /// <summary>
    /// Interface <c>IRenUkHtmlHelper</c> provides an interface to the
    /// helper methods required by the <see cref="RenUkDataSource"/>class.
    /// </summary>
    public interface IRenUkHtmlHelper
    {
        /// <summary>
        /// Loads the HtmlDocument from the specified Url
        /// </summary>
        /// <param name="baseUrl">BaseUrl for RenUk web site</param>
        /// <param name="page">Page Number</param>
        /// <param name="doc">The returned HTML document: empty document if any problems</param>
        /// <param name="region">The RenUk 'Region'. 1094 represents the default of Scotland</param>
        /// <returns>HTTP Return code.</returns>
        /// <remarks>
        /// The success or not of the HTTP call is returned from the routine.  
        /// If any HTTP status code other than "OK" is returned the HtmlDocument 
        /// output parameter contains an empty HTMLDocument
        /// </remarks>
        HttpStatusCode LoadHtmlPage(out HtmlDocument doc, string baseUrl, int page, string region = "1094");

        /// <summary>
        /// Loads the HtmlDocument from the specified Url
        /// </summary>
        /// <param name="url">Url for the aggregate on the external web site</param>
        /// <param name="doc">The returned HTML document: empty document if any problems</param>
        /// <returns>HTTP Return code.</returns>
        /// <remarks>
        /// The success or not of the HTTP call is returned from the routine.  
        /// If any HTTP status code other than "OK" is returned the HtmlDocument 
        /// output parameter contains an empty HTMLDocument
        /// </remarks>
        HttpStatusCode LoadHtmlPage(out HtmlDocument doc, string url);

        /// <summary>
        /// Retrieves the nodes from the "results" lists tag.
        /// </summary>
        /// <param name="doc">The Html Document</param>
        /// <returns>A collection of 'li' tags representing the aggregates</returns>
        IEnumerable<HtmlNode> GetAggregateNodes(HtmlDocument doc);

        /// <summary>
        /// Extract the Name only from the Aggregate
        /// </summary>
        /// <param name="node">The Aggregate element</param>
        /// <returns>The aggregate Name or an empty string if not found</returns>
        string GetAggregateName(HtmlNode node);

        /// <summary>
        /// Extracts the aggregate colum values
        /// </summary>
        /// <param name="node">The Aggregate element</param>
        /// <returns>A string, JSON object, representing the column values</returns>
        string GetAggregateColumnValues(HtmlNode node);

        /// <summary>
        /// Gets the node containing the totals, total aggregates and total pages
        /// </summary>
        /// <param name="doc">The first page (HtmlDocument)</param>
        /// <returns>Total HtmlNode of document</returns>
        string[] GetTotalAggregatesAndPages(HtmlDocument doc);

        /// <summary>
        /// Constructs the RenUk Url for a specific Aggregate entry
        /// </summary>
        /// <param name="baseUrl">The RenUk base Url</param>
        /// <param name="aggregateId">Id of the required aggregate</param>
        /// <param name="region">The RenUk 'Region'. 1094 represents the default of Scotland</param>
        /// <returns>The renUk Url for the specific Aggregate</returns>
        string GenerateAggregateUrl(string baseUrl, string aggregateId, string region = "1094");

        /// <summary>
        /// Gets the headers of the columns in the results page
        /// </summary>
        /// <param name="doc">The document containing the results</param>
        /// <returns>Array of strings representing the column headers.</returns>
        string[] GetColumnHeadings(HtmlDocument doc);

        /// <summary>
        /// Extracts the Url from the JSON object containing the url
        /// </summary>
        /// <param name="jsonUrl">Json object</param>
        /// <returns>Url</returns>
        string GetUrlFromJsonObject(string jsonUrl);
    }
}
