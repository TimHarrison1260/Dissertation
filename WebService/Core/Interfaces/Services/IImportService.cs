
namespace Core.Interfaces.Services
{
    /// <summary>
    /// Interface <c>IImportService</c> defines the contract for 
    /// the data import services.
    /// </summary>
    public interface IImportService
    {
        /// <summary>
        /// Import the data from the source
        /// </summary>
        /// <param name="sourceId">The Id of the DataSource</param>
        /// <param name="source">The source to be imported.  Includes 
        /// actual source file contents or could be a Url pointing to 
        /// the source.
        /// </param>
        /// <returns>TRUE if the source is successfully imported, otherwise FALSE.</returns>
        bool ImportData(int sourceId, string source = null);

        /// <summary>
        /// Import the data from the source
        /// </summary>
        /// <param name="sourceId">The Id of the DataSource</param>
        /// <param name="source">The source to be imported.  Includes 
        /// actual source file contents or could be a Url pointing to 
        /// the source.
        /// </param>
        /// <returns>TRUE if the source is successfully imported, otherwise FALSE.</returns>
        bool ImportData(int sourceId, object source);
    }
}
