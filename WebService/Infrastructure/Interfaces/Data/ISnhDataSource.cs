namespace Infrastructure.Interfaces.Data
{
    /// <summary>
    /// Interface <c>ISnhDataSource</c> is the interface for
    /// the specific SNH data source. It inherits
    /// the <see cref="IDataSource"/> interface to ensure the
    /// appropriate contract is implemented.  
    /// </summary>
    /// <remarks>
    /// Adding this extra layer of interface, makes it easier
    /// to define the datasource to the IoC DI container.  
    /// </remarks>
    public interface ISnhDataSource : IDataSource
    {
    }
}
