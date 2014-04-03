using System;
using Infrastructure.Interfaces.Data;

namespace Infrastructure.Resolvers
{
        /// <summary>
        /// Class <c>DataSourceResolver</c> accepts the Id from 
        /// a Domain DataSource and resolve this to an instance of
        /// the appropriate <see cref="IDataSource"/> to facilitate
        /// using the correct component to get the external datasource.
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
    public class DataSourceResolver : IDataSourceResolver
        {

            private readonly ISnhDataSource _snh;
            private readonly IRenUkDataSource _renUk;

        public DataSourceResolver(ISnhDataSource snhDataSource, IRenUkDataSource renUkDataSource)
        {
            if (snhDataSource == null)
                throw new ArgumentNullException("snhDataSource", "No valid SnhDataSource supplied to resolver.");
            if (renUkDataSource == null)
                throw new ArgumentNullException("renUkDataSource", "No valid RenUkDataSource supplied to resolver.");
            _snh = snhDataSource;
            _renUk = renUkDataSource;
        }

        /// <summary>
        /// Gets the instance of the DataSource for the specified Datasource
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <returns>Instance of the DataSource or Null if invalid Id</returns>
        public IDataSource Resolve(int id)
        {
            switch (id)
            {
                case 1:
                    return _snh;
                case 2:
                    return _renUk;
                default:
                    return null;
            }
        }
    }
}
