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
    /// Supports the api/datatype request for a Get verb, all other 
    /// verbs are unsupported and return the methd UnsupportedRequest
    /// </summary>
    public class DatatypeController : ApiController
    {
        private readonly IDataService _service;         //  Data service for accessing DataTypes
        private readonly IDataTypeMapper _mapper;      //  Mapper class, DataType to UIDataType
        private readonly IDataTypeLinkGenerator _generator;     //  Module to create the Hypermedia Links

        /// <summary>
        /// Ctor: Accepts injected dataservice
        /// </summary>
        /// <param name="dataService">The DataService instance being injected</param>
        /// <param name="dataTypeMapper">The mapper instance</param>
        public DatatypeController(IDataService dataService, IDataTypeMapper dataTypeMapper, IDataTypeLinkGenerator generator)
        {
            if (dataService == null)
                throw new ArgumentNullException("dataService", "No valid dataservice supplied to the controller.");
            if (dataTypeMapper == null)
                throw new ArgumentNullException("dataTypeMapper", "No valid mapper class supplied to the controller.");
            if (generator == null)
                throw new ArgumentNullException("generator", "No valid link generator supplied to the controller.");

            _service = dataService;
            _mapper = dataTypeMapper;
            _generator = generator;
        }

        // GET api/datatype
        //public IEnumerable<DataType> Get()
        public HttpResponseMessage Get()
        {
            var types = _service.GetDataTypes().ToArray();
            if (!types.Any())
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No DataTypes Found");
            var uiTypes = _mapper.MapTypesToLinks(types, _generator);
            return Request.CreateResponse(HttpStatusCode.OK, uiTypes);
        }

        ////  Ensure unsupported verbs are caught and not found returned
        //[AcceptVerbs(new string[] { "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "PATCH" })]
        //public HttpResponseMessage UnsupportedRequest()
        //{
        //    return Request.CreateResponse(HttpStatusCode.NotFound, "Unsupported request.");
        //}
    }
}
