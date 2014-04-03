
namespace Core.Interfaces.Services
{
    /// <summary>
    /// Interface <c>ISnhImportService</c> describes the contract specific
    /// to the import of the SNH dataset.  It inherits the 
    /// <see cref="IImportService"/> and therefore
    /// allows the implementation of the SNH data service.
    /// </summary>
    /// <remarks>
    /// It inherits <see cref="IImportService"/>, to ensure the
    /// Import method is always implemented for the interface.
    /// </remarks>
    public interface ISnhImportService : IImportService
    {
    }
}
