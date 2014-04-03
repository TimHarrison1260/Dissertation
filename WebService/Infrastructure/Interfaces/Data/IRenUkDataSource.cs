using Core.Model;

namespace Infrastructure.Interfaces.Data
{
    /// <summary>
    /// Interface <c>IRenUkDataSource</c> is the interface for
    /// the specific Renewable Uk data source. It inherits
    /// the <see cref="IDataSource"/> interface to ensure the
    /// appropriate contract is implemented.  
    /// </summary>
    /// <remarks>
    /// Adding this extra layer of interface, makes it easier
    /// to define the datasource to the IoC DI container.  
    /// </remarks>
    public interface IRenUkDataSource : IDataSource
    {
    }
}
