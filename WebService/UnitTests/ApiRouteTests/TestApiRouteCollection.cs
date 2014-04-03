using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace UnitTests.ApiRouteTests
{
    public static class TestApiRouteCollection
    {
        public static void Register(HttpConfiguration config)
        {
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
                routeTemplate: "api/{controller}/{criteria}",
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


        }
    }
}
