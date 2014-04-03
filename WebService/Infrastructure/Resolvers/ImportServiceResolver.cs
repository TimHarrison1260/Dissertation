using System;
using Core.Interfaces.Services;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Resolvers
{
    /// <summary>
    /// Class <c>ImportServiceResolver</c> accepts the Id from 
    /// a Domain DataSource and resolve this to an instance of
    /// the appropriate <see cref="IImportService"/> to facilitate
    /// using the correct component to import the external datasource.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For this implementation, a simple switch statement will be
    /// used and the instance in question will be injected through
    /// the constructor and give sufficient performance, meaning 
    /// the IoC can handle the actual instantiation.
    /// </para>
    /// <para>
    /// If the situation arose where there are 
    /// many datasources being aggregated then this would be replaced
    /// with a resolver that made use of Reflection techniques to
    /// instantiate the ImportService, the name of which would be 
    /// added to the DataSource class.
    /// </para>
    /// </remarks>
    public class ImportServiceResolver : IImportServiceResolver
    {

        private readonly ISnhImportService _Snh;
        private readonly IRenUkImportService _RenUk;

        public ImportServiceResolver(ISnhImportService snhImportService, IRenUkImportService renUkImportService)
        {
            if (snhImportService == null)
                throw new ArgumentNullException("snhImportService", "No valid SnhImportService supplied to resolver.");
            if (renUkImportService == null)
                throw new ArgumentNullException("renUkImportService", "No valid RenUkImportService supplied to resolver.");
            _Snh = snhImportService;
            _RenUk = renUkImportService;
        }

        /// <summary>
        /// Gets the instance of the ImportSerivce for the specified Datasource
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <returns>Instance of the ImportService or Null if invalid Id</returns>
        public IImportService Resolve(int id)
        {
            switch (id)
            {
                case 1:
                    return _Snh;
                case 2:
                    return _RenUk;
                default:
                    return null;
            }
        }
    }
}
