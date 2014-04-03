
namespace Core.Interfaces.Services
{
    /// <summary>
    /// Interface <c>IRenUkImportService</c> describes the contract specific
    /// to the import of the RenewableUk Screen Scrape.  It inherits the 
    /// <see cref="IImportService"/> and therefore
    /// allows the implementation of the RenewableUk data service.
    /// </summary>
    /// <remarks>
    /// It inherits <see cref="IImportService"/>, to ensure the
    /// Import method is always implemented for the interface.
    /// </remarks>
    public interface IRenUkImportService : IImportService
    {
    }
}
