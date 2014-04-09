using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using AggregateWebService.Models;

namespace AggregateWebService.Mappers
{
    /// <summary>
    /// Class <c>DataSourceMapper</c> is responsible for 
    /// mapping the data source information from the dataservice
    /// to the UI DataSources class
    /// </summary>
    public class ImportMapper : IImportMapper
    {
        /// <summary>
        /// Maps a single aggregate data source to the UI datasource
        /// returned by the web service, including any additional links
        /// </summary>
        /// <param name="id">the Windfarm Data Source</param>
        /// <param name="result">the Windfarm Data Source</param>
        /// <param name="generator">The instance of the Link Generator</param>
        /// <returns>UI data source and links</returns>
        public ImportInfo MapSource(int id, bool result, IImportLinkGenerator generator)
        {
            if (id == 0)
                return new ImportInfo();

            var importinfo = new ImportInfo()
            {
                Result = (result) ? "Resource successfully imported" : "Resource not imported, Invalid source countent",
                Links = generator.GenerateItemLinks(id)
            };
            return importinfo;
        }
    }
}