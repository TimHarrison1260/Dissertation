
namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface <c>IUpdateRepository</c> is a generic base
    /// interface designed to provide a Facade for the
    /// Update operations requireed by an implementing
    /// repopsitory class
    /// </summary>
    /// <typeparam name="T">The concrete Type the Repository is using.</typeparam>
    public interface IUpdateRepository<T> where T : class 
    {
        /// <summary>
        /// Adds a new aggregate object to the data store
        /// </summary>
        /// <param name="aggregate">The new Aggregate object</param>
        /// <returns>The assigned Id of the new Aggregate</returns>
        int Add(T aggregate);

        /// <summary>
        /// Updates an existing aggregate
        /// </summary>
        /// <param name="aggregate">The Aggregate containing the updates to be applied</param>
        void Update(T aggregate);
    }
}
