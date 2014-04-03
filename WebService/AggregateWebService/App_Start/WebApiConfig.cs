using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using AggregateWebService.Extensions;

namespace AggregateWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*
             * Register the Custom Controller and Action selectors, so the errors
             * from missing controllers, action methods are handled.
             * 
             * Sample code courtesy of Imran Baloch at: 
             * http://dotnet.dzone.com/articles/handling-http-404-error-aspnet
             */
            config.Services.Replace(typeof(IHttpControllerSelector), new HttpNotFoundAwareDefaultHttpControllerSelector(config));
            config.Services.Replace(typeof(IHttpActionSelector), new HttpNotFoundAwareControllerActionSelector());

            /*
             * Register the routes for the service
             */
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //  Alternate for Import/n: Datasource/n/Import
            config.Routes.MapHttpRoute(
                name: "DatasourceImportAlt",
                routeTemplate: "api/datasource/{id}/{controller}",
                defaults: "",
                constraints: new { controller = "^Import$", id = @"^[0-9]{1,5}" }
                );

            //  Import/n: n is required and must be numeric, so default is not best suited
            config.Routes.MapHttpRoute(
                name: "DatasourceImport",
                routeTemplate: "api/import/{id}",
                defaults: new { controller = "Import" },
                constraints: new { id = @"^[0-9]{1,5}" }
                );

            //  Import/n: n is required and must be numeric, so default is not best suited
            config.Routes.MapHttpRoute(
                name: "InvalidImport",
                routeTemplate: "api/import/",
                defaults: new { controller = "Error" }
                );

            //  Alternate for Status/n or others: Windfarm/n/Status
            config.Routes.MapHttpRoute(
                name: "WindfarmDataType",
                routeTemplate: "api/windfarm/{id}/{controller}",
                defaults: "",
                constraints: new { controller = "^Status$|^Footprint$|^Statistics$|^Turbine$", id = @"^[0-9]{1,5}" }
                );

            //  Windfarm Detail, required Id.
            //  Would be trapped by default api, but by specifying a required
            //  numeric Id here, if it gets through then the Id is not numeric
            //  and can be treated as a string: search criteria
            config.Routes.MapHttpRoute(
                name: "WindfarmDetail",
                routeTemplate: "api/windfarm/{id}",
                defaults: new { controller = "Windfarm" },
                constraints: new { id = @"^[0-9]{1,5}" }
                );

            config.Routes.MapHttpRoute(
                name: "Search",
                routeTemplate: "api/search/{criteria}",
                defaults: new { controller = "Windfarm" }
                );

            config.Routes.MapHttpRoute(
                name: "WindfarmSearch",
                routeTemplate: "api/windfarm/{criteria}",
                //                routeTemplate: "api/{controller}/{criteria}",
                defaults: new { controller = "Windfarm" },
                constraints: new { controller = "^Windfarm$|^Search$" }
                );

            //  Default API Route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = "^Datasource$|^Datatype$|^Windfarm$|^Status$|^Footprint$|^Statistics$|^Turbine$", id = @"^[0-9]{0,5}" }
            );

            //  Catchall remaining api/???/? as errors
            config.Routes.MapHttpRoute(
                name: "apiError",
                routeTemplate: "api/{something}/{id}",
                defaults: new { controller = "error", id = RouteParameter.Optional, something = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
