using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AggregateWebService.Controllers
{
    /// <summary>
    /// Supports any invalid request of the api, eg/api/rubbish/5 for all HttpVerbs
    /// </summary>
    /// <remarks>
    /// Changed code to support the error handling courtesy of Imran Baloch at:
    /// http://dotnet.dzone.com/articles/handling-http-404-error-aspnet
    /// </remarks>
    public class ErrorController : ApiController
    {
        /// <summary>
        /// Error Handler that handles all missing methods from any of the controllers.
        /// </summary>
        /// <returns>HttpStatusCode.NotFound</returns>
        [AcceptVerbs(new string[] { "GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "PATCH" })]
        public HttpResponseMessage HandleError()
        {
            var message = new HttpResponseMessage(HttpStatusCode.NotFound);
            message.ReasonPhrase = "Requested Resource not found:  " + 
                "or Http method not supported for this resource";
            return message;            
//            return Request.CreateResponse(HttpStatusCode.NotFound, "Requested Resource is not found.");
        }
    }
}
