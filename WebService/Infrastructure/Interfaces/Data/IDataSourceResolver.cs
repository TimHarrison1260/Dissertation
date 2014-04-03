
namespace Infrastructure.Interfaces.Data
{
    /// <summary>
    /// Interface <c>IDataSourceResolver</c> defines the interface
    /// for the DataSourceResolver class which gets the instance
    /// of the <see cref="IDataSource"/> implementation that will
    /// allow access to the external data.  i.e. the data being
    /// aggregated by this Web Service
    /// </summary>
    public interface IDataSourceResolver
    {
        /// <summary>
        /// Resolves the DataSource Id to return an instance of the IDataSource
        /// to facilitate accessing the data external to the Web Service.
        /// </summary>
        /// <param name="id">The Id of the datasource to be used</param>
        /// <returns>The Instance of the IDataSource compoenent</returns>
        IDataSource Resolve(int id);
    }
}
