using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Mappers;
using Core.Interfaces.Services;
using Core.Model;

namespace AggregateWebService.Controllers
{
    /// <summary>
    /// supports the api/footprint/5 and api/windfarm/5/footprint with
    /// an httpverb of Get only
    /// </summary>
    public class FootprintController : ApiController
    {
        private readonly IDataService _service;         //  Data service for accessing Aggregates
        private readonly IAggregateMapper _mapper;      //  Mapper class, Windfarm to UIAggregate
        private readonly IFootprintLinkGenerator _generator;     //  Module to create the Hypermedia Links

        /// <summary>
        /// Ctor: Accepts injected dataservice
        /// </summary>
        /// <param name="dataService">The DataService instance being injected</param>
        /// <param name="aggregateMapper">The mapper instance</param>
        public FootprintController(IDataService dataService, IAggregateMapper aggregateMapper, IFootprintLinkGenerator generator)
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
        /// GET api/footprint
        /// : Get all windfarms that have a Footprint datatype segment
        /// </summary>
        /// <returns>A collection of windfarms</returns>
        //public IEnumerable<WindfarmInfo> Get()
        public HttpResponseMessage Get()
        {
            var aggregates = _service.GetAggregatesWithDataType(DataTypeEnum.FootPrint).ToArray();
            if (!aggregates.Any())
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Windfarms found");

            var uiAggregates = _mapper.MapAggregatesToLinks(aggregates, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, uiAggregates);
        }

        /// <summary>
        /// GET api/footprint/5
        /// : Get the windfarm with Id 5, and included only the Footprint datatype segment
        /// </summary>
        /// <param name="id">The Id of the windfarm</param>
        /// <returns>The Windfarm Windfarm entry plus all possible links</returns>
        //public Windfarm Get(int id)
        public HttpResponseMessage Get(int id)
        {
            var aggregate = _service.GetAggregateWithDataType(id, DataTypeEnum.FootPrint);
            if (aggregate == null)
            {
                var message = new HttpError(string.Format("Windfarm (id = {0}) not found", id));
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }

            var uiAggregate = _mapper.MapAggregate(aggregate, DataTypeEnum.FootPrint, _generator);
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
