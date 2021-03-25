
namespace WebEas.ServiceInterface
{
    /// <summary>
    /// Delete data
    /// </summary>
    public interface IDelete<T> where T: class
    {
        /// <summary>
        /// Delete data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Delete(T data);
    }
}