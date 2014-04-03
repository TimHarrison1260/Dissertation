using AggregateWebService.Generators;
using AggregateWebService.Interfaces.Generators;
using AggregateWebService.Interfaces.Helpers;
using AggregateWebService.Interfaces.Mappers;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AggregateWebService.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(AggregateWebService.App_Start.NinjectWebCommon), "Stop")]

namespace AggregateWebService.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = 
                new Ninject.WebApi.DependencyResolver.NinjectDependencyResolver(kernel);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //  Call the IoC container to configure the application.
            IoC.Configuration.RegisterServices(kernel);

            //  Configure any UI specific entries in the IoC container
            //  Bind the mapper between the domain Datasource and the UI Datasource classes.
            kernel.Bind<IDatasourceMapper>().To<Mappers.DataSourcesMapper>();
            //  Bind the mapper for the DataTypes and the UI DataTypes
            kernel.Bind<IDataTypeMapper>().To<Mappers.DataTypeMapper>();
            //  Bind the mapper for the Aggregates (Windfarms)
            kernel.Bind<IAggregateMapper>().To<Mappers.AggregateMapper>();

            //  Bind the Link Generator classes (Windfarm: the Aggregate)
            kernel.Bind<IAggregateLinkGenerator>().To<AggregateLinkGenerator>();
            //  Bind the Link Generator classes (FootPrint)
            kernel.Bind<IFootprintLinkGenerator>().To<FootprintLinkGenerator>();
            //  Bind the Link Generator classes (Statistics)
            kernel.Bind<IStatisticsLinkGenerator>().To<StatisticsLinkGenerator>();
            //  Bind the Link Generator classes (Status)
            kernel.Bind<IStatusLinkGenerator>().To<StatusLinkGenerator>();
            //  Bind the Link Generator classes (DataSource)
            kernel.Bind<IDatasourceLinkGenerator>().To<DataSourceLinkGenerator>();
            //  Bind the Link Generator classes (DataType)
            kernel.Bind<IDataTypeLinkGenerator>().To<DataTypeLinkGenerator>();
            //  Bind the Link Generator classes (Turbine)
            kernel.Bind<ITurbineLinkGenerator>().To<TurbineLinkGenerator>();

            //  Bind the HttpRequest Helper class (Import)
            kernel.Bind<IHttpRequestHelper>().To<Helpers.HttpRequestHelper>();
        }        
    }
}
