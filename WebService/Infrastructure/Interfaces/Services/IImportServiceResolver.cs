using Core.Interfaces.Services;

namespace Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Interface <c>IImportServiceResolver</c> defines the contract
    /// for the ImportServiceResolver, which resolves the supplied
    /// datasource Id and returns the instance of the 
    /// <see cref="IImportService"/>
    /// </summary>
    public interface IImportServiceResolver
    {
        /// <summary>
        /// Gets the instance of the ImportSerivce for the specified Datasource
        /// </summary>
        /// <param name="id">Id of the datasource</param>
        /// <returns>Instance of the ImportService</returns>
        IImportService Resolve(int id);
    }
}
