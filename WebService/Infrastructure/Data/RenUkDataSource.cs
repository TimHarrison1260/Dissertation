using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Helpers;
using Infrastructure.ServiceModel;
using Infrastructure.Services;

namespace Infrastructure.Data
{
    /// <summary>
    /// Class <c> RenUkDataSource</c> provides access to the functionality
    /// required by the <see cref="RenUkImportService"/>.  It implements the
    /// <see cref="IRenUkDataSource"/> interface, referenced by the service.
    /// </summary>
    public class RenUkDataSource : IRenUkDataSource
    {
        private IList<ImportAggregate> _data;       // Not readonly so it can be set outside the ctor.
        private readonly IRenUkHtmlHelper _helper;

        /// <summary>
        /// Ctor:
        /// </summary>
        /// <param name="helper">Instance of the Helper Class</param>
        public RenUkDataSource(IRenUkHtmlHelper helper)
        {
            if (helper == null)
                throw new ArgumentNullException("helper", "No valid helper class supplied to RenUkDataSource.");
            _helper = helper;
            _data = new List<ImportAggregate>();
        }

        /// <summary>
        /// Loads the data from the specified Source (Url)
        /// </summary>
        /// <param name="source">Source Url</param>
        /// <returns>True if OK, otherwise False</returns>
        public bool LoadSource(string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            var data = new List<ImportAggregate>();
            var result = LoadData(out data, source);

            if (result) SetData(data);
            return result;
        }

        /// <summary>
        /// Gets all the aggregate names from the data source
        /// </summary>
        /// <returns>A collection of aggregate names</returns>
        public IEnumerable<string> GetAllNames()
        {
            var results = _data.Select(a => a.Identifier)
                .OrderBy(n => n)
                .Distinct();
            return results;
        }

        /// <summary>
        /// Gets the instances of all source records
        /// </summary>
        /// <returns>Returns a collection of ImportAggregate objects</returns>
        public IEnumerable<ImportAggregate> GetAll()
        {
            var results = _data.Select(a => a)
                .OrderBy(a => a.Identifier)
                .Distinct();
            return results;
        }

        /// <summary>
        /// Get the Aggregate for the specified Id
        /// </summary>
        /// <param name="url">The Url of the aggregate requested</param>
        /// <returns>The requested Aggregate or NULL if nothing found</returns>
        /// <remarks>
        /// <para>
        /// This method is for use when the data held in the internal
        /// data store is a URL to the external source, which then 
        /// has to be called to get the actual data.
        /// </para>
        /// </remarks>
        public ImportAggregate Get(string url)
        {
            //  Deserialise the JSON object containing the Url
            var actualUrl = _helper.GetUrlFromJsonObject(url);
            if (actualUrl == null) return null;

            //  This Has to accept the Url and call to the external source.
            var importAggregate = new ImportAggregate();

            if (LoadData(out importAggregate, actualUrl))
                return importAggregate;
            return null;
        }

        public IEnumerable<string> GetAltIndices(string name)
        {
            throw new NotImplementedException();
        }

        public void SetData(IList<ImportAggregate> data)
        {
            _data = data;
        }


        #region private methods

        /// <summary>
        /// Scrapes the RenUK website for the aggregate data
        /// </summary>
        /// <param name="sourceData">The output collection of ImportAggregates extracted from the data source</param>
        /// <param name="source">Base Url of the RenUk Website to be scraped</param>
        /// <returns>True if loaded successfully otherwise False</returns>
        /// <remarks>
        /// See article http://stackoverflow.com/questions/1207144/c-sharp-why-doesnt-ref-and-out-support-polymorphism
        /// for why an interface (IList) is not used as the 'out' parameter.  Look at answer by Eric Lippert and follow
        /// the links he suggests.
        /// </remarks>
        private bool LoadData(out List<ImportAggregate> sourceData, string source)
        {
            //  Initialise the output data.
            sourceData = new List<ImportAggregate>();               //

            var baseUrl = source;
            var page = 1;

            //  Read the first web page, return if not successful
            var doc = new HtmlDocument();
            if (_helper.LoadHtmlPage(out doc, baseUrl, page) != HttpStatusCode.OK)
                return false;

            var totals = _helper.GetTotalAggregatesAndPages(doc);
            var totalPages = Convert.ToInt32(totals[1]);

            //  process each of the pages available from the web site.
            while (page <= totalPages)
            {
                //  Scrape the windfarms from the html document
                sourceData.AddRange(ScrapeDocument(doc, baseUrl));
                //  Get the next page
                page++;
                if (page <= totalPages)
                    if (_helper.LoadHtmlPage(out doc, baseUrl, page) != HttpStatusCode.OK)
                    {
                        //  Initialise output incase it's partially filled, failed on page 2 or greater.
                        sourceData = new List<ImportAggregate>();
                        return false;
                    }
                    //return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the resource for a specific url, coming from the Get operation
        /// </summary>
        /// <param name="sourceData">Output data scraped from the web site</param>
        /// <param name="url">The url of the resource</param>
        /// <returns>True if successfully scraped, otherwise false</returns>
        private bool LoadData(out ImportAggregate sourceData, string url)
        {
            sourceData = new ImportAggregate();

            //  Read the web page, return if not successful
            var doc = new HtmlDocument();
            if (_helper.LoadHtmlPage(out doc, url) != HttpStatusCode.OK)
                return false;

            var data = ScrapeDocument(doc);
            if (data == null)
                return false;

            sourceData = data;
            return true;

        }

        /// <summary>
        /// Scrapes the document (page) for the aggregate data
        /// </summary>
        /// <param name="doc">The HTML page</param>
        /// <param name="baseUrl">The baseUrl of RenUK web site</param>
        /// <returns>A colection of Imported data</returns>
        private IEnumerable<ImportAggregate> ScrapeDocument(HtmlDocument doc, string baseUrl)
        {
            var importAggregates = new List<ImportAggregate>();
            //  Loop through the list of aggregate elements.
            foreach (var entry in _helper.GetAggregateNodes(doc))
            {
                var aggregateName = _helper.GetAggregateName(entry);
                var dataUrl = _helper.GenerateAggregateUrl(baseUrl, aggregateName);
                var a = CreateAggregate(aggregateName, dataUrl);
                importAggregates.Add(a);
            }
            return importAggregates;
        }

        private ImportAggregate ScrapeDocument(HtmlDocument doc)
        {
            //  Loop through the list of aggregate elements.
            foreach (var entry in _helper.GetAggregateNodes(doc))
            {
                var aggregateName = _helper.GetAggregateName(entry);
                var data = _helper.GetAggregateColumnValues(entry);
                var a = CreateAggregate(aggregateName, data);
                //  Only interested in the first should >1 aggregates be returned
                return a;
            }
            //  If we get here, nothing was returned
            return null;
        }


        /// <summary>
        /// Creates and populates the importAggregate from the Name
        /// </summary>
        /// <param name="name">The name of the aggregate (AggregateIdentifier)</param>
        /// <param name="data">The data as a JSON object</param>
        /// <returns>Returns the new ImportAggregate</returns>
        private ImportAggregate CreateAggregate(string name, string data)
        {
            var importData = new ImportData()
            {
                DataType = Core.Model.DataTypeEnum.Statistics,
                Data = data
            };

            var importAggregate = new ImportAggregate()
            {
                Identifier = name,
                ImportDate = DateTime.Now,
                SourceId = 2,
                Data = new Collection<ImportData>()
            };
            importAggregate.Data.Add(importData);
            return importAggregate;
        }



        #endregion
    }
}