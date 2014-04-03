using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Model;
using Infrastructure.Interfaces.Algorithms;
using Infrastructure.Interfaces.Data;
using Infrastructure.Interfaces.Mappers;
using Infrastructure.ServiceModel;

namespace Infrastructure.Services
{
    /// <summary>
    /// Class <c>RenUkImportService</c> implements the <see cref="IRenUkImportService"/>
    /// contract.  It imports the data from the RenewablesUK Screen Scrape and
    /// updates the <see cref="Infrastructure.Data.AggregateContext"/>.
    /// </summary>
    public class RenUkImportService : AbstractImportService, IRenUkImportService
    {
        /// <summary>
        /// Ctor: references the RenUkDataSource, otherwise all other dependencies are common
        /// </summary>
        /// <param name="repository">Common repository for the Aggregate</param>
        /// <param name="dataSourceRepository">Common repository for the DataSource</param>
        /// <param name="dataSource">RenUk Specific DataSource instance</param>
        /// <param name="matchingAlgorithm">Common Matching algorithm</param>
        /// <param name="mapper">Mapper class to map importaggregate to aggreate class</param>
        public RenUkImportService(IAggregateRepository repository, IDataSourceRepository dataSourceRepository,
            IRenUkDataSource dataSource, IMatchingAlgorithm matchingAlgorithm, IMapper<ImportAggregate, Aggregate> mapper)
            : base(repository, dataSourceRepository, dataSource, matchingAlgorithm, mapper)
        {
        }
    }
}
