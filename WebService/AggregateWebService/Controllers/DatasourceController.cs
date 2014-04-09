using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using Core.Interfaces.Services;     //  Data service to the back end.
//  UI model being serialised and exposed through service


namespace AggregateWebService.Controllers
{
    /// <summary>
    /// Supports the api/Datasource requests
    /// </summary>
    public class DatasourceController : ApiController
    {
        private readonly IDataService _service;         //  Data service for accessing Aggregates
        private readonly IDatasourceMapper _mapper;      //  Mapper class, Datasource to UIDatasource
        private readonly IDatasourceLinkGenerator _generator;     //  Module to create the Hypermedia Links

        /// <summary>
        /// Ctor: Accepts injected dataservice
        /// </summary>
        /// <param name="dataService">The DataService instance being injected</param>
        /// <param name="datasourceMapper">The mapper instance</param>
        public DatasourceController(IDataService dataService, IDatasourceMapper datasourceMapper, IDatasourceLinkGenerator generator)
        {
            if (dataService == null)
                throw new ArgumentNullException("dataService", "No valid dataservice supplied to the controller.");
            if (datasourceMapper == null)
                throw new ArgumentNullException("datasourceMapper", "No valid mapper class supplied to the controller.");
            if (generator == null)
                throw new ArgumentNullException("generator", "No valid link generator supplied to the controller.");

            _service = dataService;
            _mapper = datasourceMapper;
            _generator = generator;
        }

        // GET api/datasource
        public HttpResponseMessage Get()
        {
            var sources = _service.GeDataSources().ToArray();
            if (!sources.Any())
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Datasources Found");
            var links = _mapper.MapSourcesToLinks(sources, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, links);
        }

        // GET api/datasource/5
        public HttpResponseMessage Get(int id)
        {
            var source = _service.GetDataSource(id);
            if (source == null)
            {
                var message = new HttpError(string.Format("Datasource (id = {0}) not found", id));
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }

            var output = _mapper.MapSource(source, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, output);
        }

    }
}
