using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using Core.Interfaces.Services;

namespace AggregateWebService.Controllers
{
    /// <summary>
    /// Supports api/windfarm./5 for HttpVerb Get only
    /// </summary>
    public class WindfarmController : ApiController
    {
        private readonly IDataService _service;         //  Data service for accessing Aggregates
        private readonly IAggregateMapper _mapper;      //  Mapper class, Windfarm to UIAggregate
        private readonly IAggregateLinkGenerator _generator;     //  Module to create the Hypermedia Links

        /// <summary>
        /// Ctor: Accepts injected dataservice
        /// </summary>
        /// <param name="dataService">The DataService instance being injected</param>
        /// <param name="aggregateMapper">The mapper instance</param>
        /// <param name="generator">Link generator instance</param>
        public WindfarmController(IDataService dataService, IAggregateMapper aggregateMapper, IAggregateLinkGenerator generator)
        {
            if (dataService == null)
                throw new ArgumentNullException("dataService", "No valid dataservice supplied to the controller.");
            if (aggregateMapper == null)
                throw new ArgumentNullException("aggregateMapper", "No valid mapper class supplied to the controller.");
            if (generator == null)
                throw new ArgumentNullException("generator", "No valid link generator supplied to the controller.");

            _service = dataService;
            _mapper = aggregateMapper;
            _generator = generator;
        }

        /// <summary>
        /// GET api/windfarm
        /// : Get all windfarms 
        /// </summary>
        /// <returns>A collection of windfarms</returns>
        //public IEnumerable<WindfarmInfo> Get()
        public HttpResponseMessage Get()
        {
            var aggregates = _service.GetAggregates(string.Empty).ToArray();
            if (!aggregates.Any())
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Windfarms found");

            var uiAggregates = _mapper.MapAggregatesToLinks(aggregates, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, uiAggregates);
        }

        public HttpResponseMessage Get(string criteria)
        {
            var q = criteria != null ? criteria : string.Empty;

            var aggregates = _service.GetAggregates(q).ToArray();
            if (!aggregates.Any())
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Windfarms found matching '{0}'", criteria));

            var uiAggregates = _mapper.MapAggregatesToLinks(aggregates, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, uiAggregates);
        }

        /// <summary>
        /// GET api/windfarm/5
        /// : Get the windfarm with Id 5, and include all datatype segment
        /// </summary>
        /// <param name="id">The Id of the windfarm</param>
        /// <returns>The Windfarm Windfarm entry plus all possible links</returns>
        //public Windfarm Get(int id)
        public HttpResponseMessage Get(int id)
        {
            var aggregate = _service.GetAggregate(id);
            if (aggregate == null)
            {
                var message = new HttpError(string.Format("Windfarm (id = {0}) not found", id));
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }

            var uiAggregate = _mapper.MapAggregate(aggregate, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, uiAggregate);
        }

        ////  Ensure unsupported verbs are caught and not found returned
        //[AcceptVerbs(new string[] { "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "PATCH" })]
        //public HttpResponseMessage UnsupportedRequest()
        //{
        //    return Request.CreateResponse(HttpStatusCode.NotFound, "Unsupported request.");
        //}
    }
}
