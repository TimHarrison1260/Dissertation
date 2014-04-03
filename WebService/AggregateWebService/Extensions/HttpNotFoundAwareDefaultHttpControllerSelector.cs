using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace AggregateWebService.Extensions
{
    /// <summary>
    /// This will trap any instances where the Controller specified in the WebApi Route is 
    /// not found.
    /// 
    /// Sample code courtesy of Imran Baloch at: 
    /// http://dotnet.dzone.com/articles/handling-http-404-error-aspnet
    /// 
    /// </summary>
    public class HttpNotFoundAwareDefaultHttpControllerSelector : DefaultHttpControllerSelector
    {
        public HttpNotFoundAwareDefaultHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound)
                    throw;
                var routeValues = request.GetRouteData().Values;
                routeValues["controller"] = "Error";
                routeValues["action"] = "HandleError";
                decriptor = base.SelectController(request);
            }
            return decriptor;
        }
    }
}