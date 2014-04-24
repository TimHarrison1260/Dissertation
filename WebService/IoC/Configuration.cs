using Core.Model;
using Infrastructure.ServiceModel;
using Ninject;

namespace IoC
{
    /// <summary>
    /// Static class <c>Configuration</c> is responsible for maintaning the DI mappings
    /// for the application.  It exists to abstract the bindings for the 
    /// Ninject IoC, from the UI and therefore the requirement for the UI to 
    /// reference the Infrastructure project.
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Static <c>RegisterServices</c> allows the necessary IoC bindings for the 
        /// application to be set.
        /// </summary>
        /// <param name="kernel">A reference to the Static instance of the Ninject IoC Kernel</param>
        public static void RegisterServices(IKernel kernel)
        {

            //  Allow Ninject to manage the instance of the DbContext so that we can get access to the same instance.
            kernel.Bind<Infrastructure.Interfaces.Data.IUnitOfWork>().To<Infrastructure.Data.AggregateContext>().InScope(c => System.Web.HttpContext.Current);

            //  Bind the repositories.
            kernel.Bind<Core.Interfaces.Repositories.IDataSourceRepository>().To<Infrastructure.Repositories.DataSourceRepository>();
            kernel.Bind<Core.Interfaces.Repositories.IAggregateRepository>().To<Infrastructure.Repositories.AggregateRepository>();
            kernel.Bind<Core.Interfaces.Repositories.IDataTypeRepository>().To<Infrastructure.Repositories.DataTypeRepository>();

            //  Bind the Matching algorithms
            kernel.Bind<Infrastructure.Interfaces.Algorithms.ICoefficientAlgorithm>().To<Infrastructure.Algorithms.DiceCoefficient>();
            kernel.Bind<Infrastructure.Interfaces.Algorithms.IEditDistanceAlgorithm>().To<Infrastructure.Algorithms.LevenshteinEditDistance>();
            kernel.Bind<Infrastructure.Interfaces.Algorithms.IStringSimilarityAlgorithm>().To<Infrastructure.Algorithms.LcSubstr>();
            kernel.Bind<Infrastructure.Interfaces.Algorithms.IAlgorithmPreProcess>().To<Infrastructure.Algorithms.PreProcessor>();
            kernel.Bind<Infrastructure.Interfaces.Algorithms.IMatchingAlgorithm>()
                .To<Infrastructure.Algorithms.MatchingAlgorithm>()
                .WithConstructorArgument("coefficientLimit", 0.9)       //  Threshold limit for Dice's Coefficient
                .WithConstructorArgument("percentageLimit", 0.9)        //  Threshold for Common Substring Percentage
                .WithConstructorArgument("editDistanceLimit", 2);       //  Threshold for Levenshtein Edit Distance

            //  Bind the DataSources.
            kernel.Bind<Infrastructure.Interfaces.Helpers.IRenUkHtmlHelper>().To<Infrastructure.Helpers.RenUkHtmlHelper>();
            kernel.Bind<Infrastructure.Interfaces.Data.IRenUkDataSource>().To<Infrastructure.Data.RenUkDataSource>();
            kernel.Bind<Infrastructure.Interfaces.Helpers.ISnhKmlHelper>().To<Infrastructure.Helpers.SnhKmlHelper>();
            kernel.Bind<Infrastructure.Interfaces.Data.ISnhDataSource>().To<Infrastructure.Data.SnhDataSource>();

            //  Bind the Datasource Resolvers
            kernel.Bind<Infrastructure.Interfaces.Data.IDataSourceResolver>().To<Infrastructure.Resolvers.DataSourceResolver>();
            
            //  Bind the Mapper class
            kernel.Bind<Infrastructure.Interfaces.Mappers.IMapper<ImportAggregate, Aggregate>>().To<Infrastructure.Mappers.ImportAggregateToAggregateMapper>();

            //  Bind the import services
            kernel.Bind<Core.Interfaces.Services.IRenUkImportService>().To<Infrastructure.Services.RenUkImportService>();
            kernel.Bind<Core.Interfaces.Services.ISnhImportService>().To<Infrastructure.Services.SnhImportService>();

            //  Bind the Import service resolver
            kernel.Bind<Infrastructure.Interfaces.Services.IImportServiceResolver>().To<Infrastructure.Resolvers.ImportServiceResolver>();

            //  Bind the Data Service itself
            kernel.Bind<Core.Interfaces.Services.IDataService>().To<Infrastructure.Services.DataService>();
            
        }
    }
}
