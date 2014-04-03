namespace Infrastructure.Interfaces.Mappers
{
    /// <summary>
    /// Interface <c>IMapper</c> provides an interface
    /// for any classes that map a class to another class
    /// </summary>
    public interface IMapper<T, U> where T : class where U : class
    {
        /// <summary>
        /// Maps a Class of type T to a class of type U
        /// </summary>
        /// <param name="t">Class of type T to be mapped</param>
        /// <returns>Class of type U</returns>
        U Map(T t);
    }
}
