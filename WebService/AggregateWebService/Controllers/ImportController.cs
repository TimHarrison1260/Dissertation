using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AggregateWebService.Interfaces.Helpers;
using Core.Interfaces.Services;

namespace AggregateWebService.Controllers
{
    /// <summary>
    /// Supports the api/import/5 or api/datasource/5/import with HttpVerb Put only
    /// </summary>
    public class ImportController : ApiController
    {
        private readonly IDataService _service;         //  Data service for accessing Aggregates
        private readonly IHttpRequestHelper _helper;    //  Helper class

        /// <summary>
        /// Ctor: Accepts injected dataservice
        /// </summary>
        /// <param name="dataService">The DataService instance being injected</param>
        /// <param name="helper">The helper class instance</param>
        public ImportController(IDataService dataService, IHttpRequestHelper helper)
        {
            if (dataService == null)
                throw new ArgumentNullException("dataService", "No valid dataservice supplied to the controller.");
            if (helper == null)
                throw new ArgumentNullException("helper", "No valid helper supplied to the controller.");

            _service = dataService;
            _helper = helper;
        }

        /// <summary>
        /// Handles the PUT HttpRequest from the Import (PUT api/import/5)
        /// </summary>
        /// <param name="id">Id of the Datasource being imported</param>
        /// <returns>True if the import is successful, otherwise False.</returns>
        public async Task<HttpResponseMessage> Put(int id)
        {
            try
            {
                string fileContents = null;

                if (_service.ImportRequiresSourceData(id))
                {
                    //  Get the source from the httpRequest
                    //  Validate the HTTPRequest contains "multipart/form-type"
                    if (!Request.Content.IsMimeMultipartContent())
                        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                    //  Get the contents of the file uploaded in the Request.
                    fileContents = await _helper.GetMimeFileContents(Request);
                }

                //  Call the Import facility on the DataService to import the data source contents
                var result = await Task.Factory.StartNew(() => _service.ImportDataSource(id, fileContents));

                if (result)
                    //  Return successful import result.
                    return Request.CreateResponse(HttpStatusCode.OK, "Resource successfully imported");

                //  otherwise Return an unsuccessful import result
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Resource not imported, Invalid source countent");
            }
            catch (Exception e)
            {
                //  An exception was thrown, 
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        ////  Ensure unsupported verbs are caught and not found returned
        //[AcceptVerbs(new string[] { "GET", "POST", "DELETE", "HEAD", "OPTIONS", "PATCH" })]
        //public HttpResponseMessage UnsupportedRequest()
        //{
        //    return Request.CreateResponse(HttpStatusCode.NotFound, "Unsupported request.");
        //}


    }
}
