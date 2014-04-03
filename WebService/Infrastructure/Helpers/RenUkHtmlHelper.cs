using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;               //  Http Status Code
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Infrastructure.Interfaces.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Class <c>RenUkHtmlHelper</c> provides methods to 
    /// help manipulate the HTML pages from the RenUK 
    /// web site
    /// </summary>
    public class RenUkHtmlHelper : IRenUkHtmlHelper
    {
        private readonly HtmlWeb _web = new HtmlWeb();  // Html Agility Pack

        //  Structure, to store the column values for serialising to JSON object
        private struct Statistics
        {
            public string Name;
            public string Region;
            public string Location;
            public string Turbines;
            public string ProjectCapacity;
            public string TurbineCapacity;
            public string Developer;
            public string StatusDate;
            public string Status;
            public string ProjectType;

            public string ToJson()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        //private struct JsonUrl
        //{
        //    public string url;
        //}


        #region Publid Methods

        /// <summary>
        /// Loads the HtmlDocument from the specified Url
        /// </summary>
        /// <param name="baseUrl">BaseUrl for RenUk web site</param>
        /// <param name="page">Page Number</param>
        /// <param name="doc">The returned HTML document: empty document if any problems</param>
        /// <param name="region">The RenUk 'Region'. 1094 represents the default of Scotland</param>
        /// <returns>"OK" unless problem occurred, then returns a string representation of the HTTP return code.</returns>
        /// <remarks>
        /// The success or not of the HTTP call is returned from the routine.  
        /// If any HTTP status code other than "OK" is returned the HtmlDocument 
        /// output parameter contains an empty HTMLDocument
        /// </remarks>
        public HttpStatusCode LoadHtmlPage(out HtmlDocument doc, string baseUrl, int page, string region = "1094")
        {
            var url = GeneratePageUrl(baseUrl, page, region);
            return LoadHtmlPage(out doc, url);
        }

        /// <summary>
        /// Load the dosument from the specified Url
        /// </summary>
        /// <param name="doc">The output document, loaded</param>
        /// <param name="url">The specific Url</param>
        /// <returns>Returns the HTTPStatusCode from the call.</returns>
        /// <remarks>
        /// The success or not of the HTTP call is returned from the routine.  
        /// If any HTTP status code other than "OK" is returned the HtmlDocument 
        /// output parameter contains an empty HTMLDocument
        /// </remarks>
        public HttpStatusCode LoadHtmlPage(out HtmlDocument doc, string url)
        {
            try
            {
                var html = _web.Load(url);

                //  Set the return values depending on the success of the load.
                doc = (_web.StatusCode != HttpStatusCode.OK) ? new HtmlDocument() : html;
                return _web.StatusCode;
            }
            catch (Exception ex)
            {
                //  Consume any exceptions thrown by the Agility pack 
                //  Return an empty document and set the return code to InternalServerError.
                doc = new HtmlDocument();
                return HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// Retrieves the nodes from the "results" lists tag.
        /// </summary>
        /// <param name="doc">The Html Document</param>
        /// <returns>A collection of 'li' tags representing the aggregates</returns>
        public IEnumerable<HtmlNode> GetAggregateNodes(HtmlDocument doc)
        {
            //  Get the search results node
            HtmlNode searchResultnode = null;
            if (GetSearchResultsNode(doc, out searchResultnode))
            {
                var orderedLists = searchResultnode.Descendants("ol");

                foreach (var child in orderedLists)
                {
                    var attrs = child.Attributes;
                    var attrCount = attrs.Count();

                    if (attrCount > 0 && attrs[0].Value == "result-listing")
                    {
                        var entries = child.Elements("li");
                        return entries;         //  Leave immediately, to save time.
                    }
                }
            }
            //  If we get here, there's no aggregates in the document
            return new Collection<HtmlNode>();
        }

        /// <summary>
        /// Extract the Name only from the Aggregate
        /// </summary>
        /// <param name="node">The Aggregate element</param>
        /// <returns>The aggregate Name or an empty string if not found</returns>
        public string GetAggregateName(HtmlNode node)
        {
            var names = GetColumnValues(node, "1");

            if (names.Count() >= 1 && names[0] != null)
                //  Name found
                return names[0];

            return string.Empty;
        }

        /// <summary>
        /// Extracts the aggregate colum values
        /// </summary>
        /// <param name="node">The Aggregate element</param>
        /// <returns>A string, JSON object, representing the column values</returns>
        public string GetAggregateColumnValues(HtmlNode node)
        {
            var values = GetColumnValues(node);

            //  Create an object (private struct) to allow JSON.Net to produce the JSON object;
            var jsonOut = new Statistics
            {
                Name = string.IsNullOrEmpty(values[0]) ? string.Empty : values[0],
                Region = string.IsNullOrEmpty(values[1]) ? string.Empty : values[1],
                Location = string.IsNullOrEmpty(values[2]) ? string.Empty : values[2],
                Turbines = string.IsNullOrEmpty(values[3]) ? string.Empty : values[3],
                ProjectCapacity = string.IsNullOrEmpty(values[4]) ? string.Empty : values[4],
                TurbineCapacity = string.IsNullOrEmpty(values[5]) ? string.Empty : values[5],
                Developer = string.IsNullOrEmpty(values[6]) ? string.Empty : values[6],
                StatusDate = string.IsNullOrEmpty(values[7]) ? string.Empty : values[7],
                Status = string.IsNullOrEmpty(values[8]) ? string.Empty : values[8],
                ProjectType = string.IsNullOrEmpty(values[9]) ? string.Empty : values[9]
            };

            var jsonReturn = JsonConvert.SerializeObject(jsonOut);

            return jsonReturn;
        }

        /// <summary>
        /// Gets the node containing the totals, total aggregates and total pages
        /// </summary>
        /// <param name="doc">The first page (HtmlDocument)</param>
        /// <returns>Total HtmlNode of document</returns>
        public string[] GetTotalAggregatesAndPages(HtmlDocument doc)
        {
            var totalAggregates = "0";
            var totalPages = "0";

            //var node = doc.GetElementbyId("layer-ukwed-search");
            //  Get the search results node
            HtmlNode node = null;
            if (GetSearchResultsNode(doc, out node))
            {
                //  Get all <div> elements
                var divs = node.Descendants("div");
                //  Loop through them looking for the correct ones
                foreach (var div in divs)
                {
                    //var name = div.Name;
                    var attrs = div.Attributes;
                    if (attrs.Count > 0)
                    {
                        //var attrValue = attrs[0].Value;
                        if (attrs[0].Value == "style-pagination-results")
                        {
                            //  Get the Total number of wind farms from the div
                            totalAggregates = GetTotalAggregates(div);
                        }
                        if (attrs[0].Value == "style-pagination-pages style-right-append-total")
                            totalPages = GetTotalPages(div);
                    }
                }
            }
            //  There no search results elements
            var result = new[] { totalAggregates, totalPages };
            return result;
        }

        /// <summary>
        /// Constructs the RenUk Url for a specific Aggregate entry
        /// </summary>
        /// <param name="baseUrl">The RenUk base Url</param>
        /// <param name="aggregateId">Id of the required aggregate</param>
        /// <param name="region">The RenUk 'Region'. 1094 represents the default of Scotland</param>
        /// <returns>The renUk Url for the specific Aggregate, as a JSON object</returns>
        public string GenerateAggregateUrl(string baseUrl, string aggregateId, string region = "1094")
        {
            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(aggregateId))
                return string.Empty;

            var bldr = new StringBuilder();
            bldr.Append("{\"Url\": \"");
            bldr.AppendFormat(@"{0}/name/{1}/status/All/region/{2}/", baseUrl, aggregateId, region);
            bldr.Append("\"}");
            return bldr.ToString();
        }

        /// <summary>
        /// Gets the headers of the columns in the results page
        /// </summary>
        /// <param name="doc">The document containing the results</param>
        /// <returns>Array of strings representing the column headers.</returns>
        public string[] GetColumnHeadings(HtmlDocument doc)
        {
            //  Get the search results node
            HtmlNode searchResultnode = null;
            if (GetSearchResultsNode(doc, out searchResultnode))
            {
                var divs = searchResultnode.Descendants("div");

                foreach (var child in divs)
                {
                    var attrs = child.Attributes;
                    var attrCount = attrs.Count();

                    if (attrCount > 0 && attrs[0].Value == "layer-heading layer-clearfix")
                    {
                        //  Pass the node into the GetColumnValues
                        var headers = GetColumnValues(child);
                        return headers;
                    }
                }
            }
            //  If we get here, there's no results in the document
            return new[] {string.Empty};
        }

        /// <summary>
        /// Extracts the Url from the JSON object containing the url
        /// </summary>
        /// <param name="jsonUrl">Json object</param>
        /// <returns>Url or Null if no url contained</returns>
        public string GetUrlFromJsonObject(string jsonUrl)
        {
            if (string.IsNullOrEmpty(jsonUrl)) return null;
            //  extract the url
            var properties = JObject.Parse(jsonUrl);
            var url = properties["Url"];
            return url != null ? url.ToString() : null;
        }





        #endregion


        #region private methods

        /// <summary>
        /// Gets all the column values from the li node (Aggregate)
        /// </summary>
        /// <param name="node">The Aggregate Node</param>
        /// <param name="columnNumber">Desired Columne Number, 'all' or'1' for the name column</param>
        /// <returns>Array of column values</returns>
        /// <remarks>
        /// Abstracted the getting the column values to here to follow OO SRP and
        /// the column tag names are only maintained in a single place.
        /// </remarks>
        private string[] GetColumnValues(HtmlNode node, string columnNumber = "all")
        {
            //  Using an array we can guarantee the order of the column values
            var columnValues = new string[10];

            //  Extract the elements from the column <p> tags
            var columns = node.Elements("p");

            //  Use foreach so there is only one iteration over the collection
            //  then we don't need to use .ToArray() to allow indexing;
            foreach (var column in columns)
            {
                if (column.Attributes.Count > 0)
                {
                    var colName = column.Attributes[0].Value;
                    var colAttributes = colName.Split(' ');
                    var colId = colAttributes[0];
                    switch (colId)
                    {
                        case "panel-column1":
                            /*
                             *  HTML Agility Pack has a utility class HtmlEntity containing a static class
                             *  DeEntitize, which decode html elements like &nbsp; replacing them with
                             *  the appropriate character.
                             * */
                            var trimChars = new char[] { '\r', '\n', ' ' };
                            //var colValue = column.InnerText;
                            //var colValueTrimmed = colValue.Trim(trimChars);
                            //columnValues[0] = HtmlAgilityPack.HtmlEntity.DeEntitize(colValueTrimmed);
                            //  If only interested in the name, the first column, then exit immediately
                            columnValues[0] = HtmlAgilityPack.HtmlEntity.DeEntitize(column.InnerText.Trim(trimChars));
                            if (columnNumber != "all")
                                return columnValues;
                            break;
                        case "panel-column2":
                            columnValues[1] = column.InnerText;
                            break;
                        case "panel-column3":
                            columnValues[2] = HtmlEntity.DeEntitize(column.InnerText);
                            break;
                        case "panel-column4":
                            columnValues[3] = column.InnerText;
                            break;
                        case "panel-column5":
                            columnValues[4] = column.InnerText;
                            break;
                        case "panel-column6":
                            columnValues[5] = column.InnerText;
                            break;
                        case "panel-column7":
                            columnValues[6] = column.InnerText;
                            break;
                        case "panel-column8":
                            columnValues[7] = column.InnerText;
                            break;
                        case "panel-column9":
                            columnValues[8] = column.InnerText;
                            break;
                        case "panel-column10":
                            columnValues[9] = column.InnerText;
                            break;
                    }
                }
            }
            return columnValues;
        }


        /// <summary>
        /// Gets the Search Results node from the Html Document passed in
        /// </summary>
        /// <param name="doc">The document to be searched</param>
        /// <param name="searchResultsNode">The search results node</param>
        /// <returns>True if found else false</returns>
        /// <remarks>
        /// Abstracted the checks since it is required in more than one method, and 
        /// follows OO-SRP, as only place where the name of the search results node
        /// is defined.
        /// </remarks>
        private bool GetSearchResultsNode(HtmlDocument doc, out HtmlNode searchResultsNode)
        {
            //  Check if the document passed in is null or empty
            if (doc == null || doc.DocumentNode.ChildNodes.Count == 0)
            {
                searchResultsNode = null;
                return false;
            }

            //  Now find the "searchResults" node.
            searchResultsNode = doc.GetElementbyId("layer-ukwed-search");
            if (searchResultsNode == null)      // Not found, wrong type of document
                return false;

            //  At last, found the search results node
            return true;
        }


        /// <summary>
        /// Constructs the RenUK Url for a particular page of aggregates
        /// </summary>
        /// <param name="baseUrl">The renUk base Url</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="region">The RenUk 'Region'. 1094 represents the default for Scotland</param>
        /// <returns>The RenUK Url for the specific page</returns>
        private string GeneratePageUrl(string baseUrl, int pageNumber, string region)
        {
            var bldr = new StringBuilder();
            bldr.AppendFormat(@"{0}/page/{1}/status/All/region/{2}/", baseUrl, pageNumber, region);
            return bldr.ToString();
        }

        /// <summary>
        /// Extracts the total number of aggregates found from the site, all pages.
        /// </summary>
        /// <param name="node">document page</param>
        /// <returns>Number of Aggregates found</returns>
        private string GetTotalAggregates(HtmlNode node)
        {
            //  Find the <span> element with class "pagination-number"
            var spans = node.Descendants("span");
            foreach (var span in spans)
            {
                var attrs = span.Attributes;
                if (attrs.Count > 0 && attrs[0].Value == "pagination-number")
                {
                    return span.InnerHtml;
                }
            }
            //  If we get here nothing has been returned from the web site page
            return "0";
        }

        /// <summary>
        /// Extracts the number of pages containing aggregates from the document
        /// </summary>
        /// <param name="node">the document</param>
        /// <returns>The number of pages</returns>
        private string GetTotalPages(HtmlNode node)
        {
            var lastIndex = 0;
            var pages = string.Empty;

            //  Find the last <span> element with class "page-numbering"
            var spanNodes = node.Descendants("span");
            if (spanNodes.Count() == 0)      //  No pages found, leave immediately.
                return "1";

            //  Want to loop backwards since looking for the last span tag.
            var spans = spanNodes.ToArray();
            for (var i = spans.Count() - 1; i >= 0; i--)
            {
                var attrs = spans[i].Attributes;
                if (attrs.Count > 0 && attrs[0].Value == "page-numbering")
                {
                    lastIndex = i;
                    break;
                }
            }
            var lastSpan = spans[lastIndex];

            //  Get the anchor element
            var anchors = lastSpan.Descendants("a").ToArray();
            if (anchors.Count() > 0)
            {
                return anchors[0].InnerHtml;
            }
            //  If we get here nothing has been returned from the web site page
            return "0";
        }

        #endregion


    }
}
