using System;
using System.Net.Http;
//using System.Web;
using System.Web.Http;
//using System.Web.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using AggregateWebService.Controllers;
using AggregateWebService.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ApiRouteTests
{
    [TestClass]
    public class IncomingRouteTests
    {
        private HttpConfiguration _config;
        //private Mock<System.Net.Http.HttpRequestMessage> _mockHttpRequest;

        [TestInitialize]
        public void Initialise()
        {
            //  Define an api route table
            _config = new HttpConfiguration();
            TestApiRouteCollection.Register(_config);
        }


        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Datasource_mapsTo_DataSourceController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/datasource/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(DatasourceController), routeTester.GetControllerType(), "Expected the DatasourceController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Datasource_Post_mapsTo_ErrorController_ErrorHandler()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com/api/datasource/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(DatasourceController), routeTester.GetControllerType(), "Expected the CatchallController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the HandleError Method to be returned");
            Assert.AreEqual(typeof(ErrorController), routeTester.GetErrorControllerType(), "Expected the ErrorController to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Datasource_1_mapsTo_DataSourceController_MethodGet_id1()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/datasource/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(DatasourceController), routeTester.GetControllerType(), "Expected the DatasourceController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Datasource_1_Post_mapsTo_ErrorController_ErrorHandler()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com/api/datasource/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(DatasourceController), routeTester.GetControllerType(), "Expected the DatasourceController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the HandleError Method to be returned");
            Assert.AreEqual(typeof(ErrorController), routeTester.GetErrorControllerType(), "Expected the ErrorController to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Datasource_1_Import_mapsTo_ImportController_MethodPut_id1()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, "http://www.test.com/api/datasource/1/import");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ImportController), routeTester.GetControllerType(), "Expected the ImportController to be returned");
            Assert.AreEqual("Put", routeTester.GetActionName(), "Expected the Put Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Datasource_1_Import_Get_mapsTo_ErrorController_HandleError()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/datasource/1/import");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ImportController), routeTester.GetControllerType(), "Expected the ImportController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the HandleError Method to be returned");
            Assert.AreEqual(typeof(ErrorController), routeTester.GetErrorControllerType(), "Expected the ErrorController to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Import_1_mapsTo_ImportController_MethodPut_id1()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, "http://www.test.com/api/import/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ImportController), routeTester.GetControllerType(), "Expected the ImportController to be returned");
            Assert.AreEqual("Put", routeTester.GetActionName(), "Expected the Put Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Import_1_Get_mapsTo_ErrorController_HandleError()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/import/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ImportController), routeTester.GetControllerType(), "Expected the ImportController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the HandleError Method to be returned");
            Assert.AreEqual(typeof(ErrorController), routeTester.GetErrorControllerType(), "Expected the ErrorController to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Import_NoId_Put_mapsTo_ErrorController_HandleError()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, "http://www.test.com/api/import/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ErrorController), routeTester.GetControllerType(), "Expected the ErrorController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the HandleError Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void DataType_mapsTo_DataTypeController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/datatype/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(DatatypeController), routeTester.GetControllerType(), "Expected the DatatypeController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Windfarm_1_Status_mapsTo_StatusController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/windfarm/1/status");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(StatusController), routeTester.GetControllerType(), "Expected the StatusController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Status_1_mapsTo_StatusController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/status/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(StatusController), routeTester.GetControllerType(), "Expected the StatusController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Windfarm_1_mapsTo_WindfarmController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/windfarm/1/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(WindfarmController), routeTester.GetControllerType(), "Expected the WindfarmController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Windfarm_mapsTo_WindfarmController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/windfarm/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(WindfarmController), routeTester.GetControllerType(), "Expected the WindfarmController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual(string.Empty, routeTester.GetParameter(), "Expected 'id' as blank");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Windfarm_Criteria_mapsTo_WindfarmController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/windfarm/criteria+1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(WindfarmController), routeTester.GetControllerType(), "Expected the WindfarmController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("criteria 1", routeTester.GetParameter("criteria"), "Expected 'id' as blank");
        }


        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Search_Criteria_mapsTo_WindfarmController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/search/criteria+1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(WindfarmController), routeTester.GetControllerType(), "Expected the WindfarmController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("criteria 1", routeTester.GetParameter("criteria"), "Expected 'id' as blank");
        }


        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Windfarm_1_Footprint_mapsTo_FootprintController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/windfarm/1/footprint");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(FootprintController), routeTester.GetControllerType(), "Expected the FootprintController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Footprint_1_mapsTo_FootprintController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/footprint/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(FootprintController), routeTester.GetControllerType(), "Expected the FootprintController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Footprint_mapsTo_FootprintController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/footprint/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(FootprintController), routeTester.GetControllerType(), "Expected the FootprintController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Windfarm_1_Statistics_mapsTo_StatisticsController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/windfarm/1/statistics");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(StatisticsController), routeTester.GetControllerType(), "Expected the statisticsController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void statistics_1_mapsTo_statisticsController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/statistics/1");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(StatisticsController), routeTester.GetControllerType(), "Expected the statisticsController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
            Assert.AreEqual("1", routeTester.GetParameter(), "Expected the Id to be '1'");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void Statistics_mapsTo_statisticsController_MethodGet()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/statistics/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(StatisticsController), routeTester.GetControllerType(), "Expected the statisticsController to be returned");
            Assert.AreEqual("Get", routeTester.GetActionName(), "Expected the Get Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void NonExistent_Get_mapsTo_ErrorController_HandleError()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.test.com/api/nonexistant/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ErrorController), routeTester.GetControllerType(), "Expected the ErrorController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the Get Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void NonExistent_Post_mapsTo_ErrorController_HandleError()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com/api/nonexistant/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ErrorController), routeTester.GetControllerType(), "Expected the ErrorController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the Handleerror Method to be returned");
        }

        [TestMethod]
        [TestCategory("IncomingRoutes")]
        public void api_nocontroller_Post_mapsTo_ErrorController_HandleError()
        {
            //  Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com/api/");

            //  Act
            var routeTester = new RouteTester(_config, request);

            //  Assert
            Assert.AreEqual(typeof(ErrorController), routeTester.GetControllerType(), "Expected the ErrorController to be returned");
            Assert.AreEqual("HandleError", routeTester.GetActionName(), "Expected the HandleError Method to be returned");
        }


    }

    /// <summary>
    /// class for constructing the Routing classes 
    /// </summary>
    /// <remarks>
    /// Thanks to http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/
    /// for this class to assist the Route testing for Web Api routes.
    /// </remarks>
    internal class RouteTester
    {
        private HttpConfiguration config;
        private HttpRequestMessage request;
        private IHttpRouteData routeData;
        private IHttpControllerSelector controllerSelector;
        private IHttpActionSelector actionSelector;
        private HttpControllerContext controllerContext;

        /// <summary>
        /// Ctor: set up the request etc.
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="req"></param>
        public RouteTester(HttpConfiguration conf, HttpRequestMessage req)
        {
            config = conf;
            request = req;
            routeData = config.Routes.GetRouteData(request);
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            //controllerSelector = new DefaultHttpControllerSelector(config);
            controllerSelector = new HttpNotFoundAwareDefaultHttpControllerSelector(config);    // Handle 404 errors
            actionSelector = new HttpNotFoundAwareControllerActionSelector();                   // Handle 404 errors

            controllerContext = new HttpControllerContext(config, routeData, request);
        }

        public Type GetControllerType()
        {
            var descriptor = controllerSelector.SelectController(request);
            controllerContext.ControllerDescriptor = descriptor;
            return descriptor.ControllerType;
        }

        public string GetActionName()
        {
            if (controllerContext.ControllerDescriptor == null)
                GetControllerType();

            //var actionSelector = new ApiControllerActionSelector();
            var descriptor = actionSelector.SelectAction(controllerContext);

            return descriptor.ActionName;
        }

        /*
         * The ActionName may change the controller to the ErrorController
         * if the method does not exist in the requested Controller 
         */
        public Type GetErrorControllerType()
        {
            if (controllerContext.ControllerDescriptor == null)
                GetControllerType();

            var descriptor = actionSelector.SelectAction(controllerContext);
            var controllerDesctiptor = descriptor.ControllerDescriptor;
            return controllerDesctiptor.ControllerType;
        }


        public string GetParameter(string parameterName = "id")
        {
            var value = controllerContext.RouteData.Values[parameterName].ToString();
            return value;
        }

        //public int GetParameterCount()
        //{
        //    var count = controllerContext.RouteData.Values.Count;
        //    return count;
        //}

    }



}
