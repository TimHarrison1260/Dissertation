
namespace Core.Interfaces.Repositories
{
    /// <summary>
    /// Interface <c>IDataTypeRepository</c> defines the contract
    /// for the DataTypeRepository.  It is intended to provide
    /// methods for getting the DataTypes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Datatype is currently an enumeration, and it is complete
    /// overkill to service the information this way, but it should
    /// really be a class in the model, but it's too late to refactor
    /// it at the moment.  Providing access via this repository will
    /// simplify the refactoring in the future.
    /// </para>
    /// It requires only read operations at this implementation so
    /// only implements the <see cref="IBaseRepository{T}"/> interface.
    /// </remarks>
    public interface IDataTypeRepository : IBaseRepository<string>
    {
    }
}
