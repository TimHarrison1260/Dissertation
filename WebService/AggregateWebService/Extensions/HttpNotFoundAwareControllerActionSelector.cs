using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using AggregateWebService.Controllers;

namespace AggregateWebService.Extensions
{
    /// <summary>
    /// This will trap any instances where the method specified in the request
    /// is not found within the controller.
    /// 
    /// Sample code courtesy of Imran Baloch at: 
    /// http://dotnet.dzone.com/articles/handling-http-404-error-aspnet
    /// 
    /// </summary>
    public class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
    {
        public HttpNotFoundAwareControllerActionSelector()
        {
        }

        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            HttpActionDescriptor decriptor = null;
            try
            {
                decriptor = base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                var code = ex.Response.StatusCode;
                if (code != HttpStatusCode.NotFound && code != HttpStatusCode.MethodNotAllowed)
                    throw;
                var routeData = controllerContext.RouteData;
                routeData.Values["action"] = "HandleError";
                IHttpController httpController = new ErrorController();
                controllerContext.Controller = httpController;
                controllerContext.ControllerDescriptor = new HttpControllerDescriptor(controllerContext.Configuration, "Error", httpController.GetType());
                decriptor = base.SelectAction(controllerContext);
            }
            return decriptor;
        }
    }
}