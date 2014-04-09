using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AggregateWebService.Interfaces.Helpers;

namespace AggregateWebService.Helpers
{
    /// <summary>
    /// Class <c>HttpRequestHelper</c> is responsible for handling
    /// the files contained within the HttpRequest.
    /// </summary>
    public class HttpRequestHelper : IHttpRequestHelper
    {
        private readonly MultipartFormDataStreamProvider _provider;     //  DataStreamProvider

        private string _tempFileName = string.Empty;

        /// <summary>
        /// Ctor: set up the helper class
        /// </summary>
        public HttpRequestHelper()
        {
            var dataDir = HttpContext.Current.Server.MapPath("~/App_Data");
            _provider = new MultipartFormDataStreamProvider(dataDir);
        }

        /// <summary>
        /// Gets the contents of any file that is included within the
        /// HTTPRequest.  It is used to extract the SNH data source.
        /// </summary>
        /// <param name="request">The HttpRequest containing the files</param>
        /// <returns>The contents of the file as a String</returns>
        public async Task<string> GetMimeFileContents(HttpRequestMessage request)
        {
            string fileContents = string.Empty;

            //  Get the file from the request
            await request.Content.ReadAsMultipartAsync(_provider);

            //  Get the first filename, only allow one file to be uploaded
            if (_provider.FileData.Count > 0)
            {
                //  Read the contents of the uploaded file, for passing into the Import facility
                _tempFileName = _provider.FileData[0].LocalFileName;
                using (var reader = new StreamReader(new FileStream(_tempFileName, FileMode.Open, FileAccess.Read)))
                {
                    fileContents = await reader.ReadToEndAsync();
                }
                //  Delete the temporary file.
                if (File.Exists(_tempFileName))
                    File.Delete(_tempFileName);
            }

            return fileContents;
        }

    }
}