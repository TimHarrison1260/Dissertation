using System.Net.Http;
using System.Threading.Tasks;

namespace AggregateWebService.Interfaces.Helpers
{
    /// <summary>
    /// Interface <c>IHttpRequestHelper</c> provides the interface
    /// to the helper class that supports the HTTPRequest processing
    /// for the Import Controller.
    /// </summary>
    public interface IHttpRequestHelper
    {
        /// <summary>
        /// Gets the contents of any file that is included within the
        /// HTTPRequest.  It is used to extract the SNH data source.
        /// </summary>
        /// <param name="request">The HttpRequest containing the files</param>
        /// <returns>The contents of the file as a String</returns>
        Task<string> GetMimeFileContents(HttpRequestMessage request);
    }
}
