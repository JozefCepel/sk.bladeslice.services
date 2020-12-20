using System;
using System.Linq;

namespace WebEas.ServiceInterface
{
    /// <summary>
    /// Create Data
    /// </summary>
    public interface ICreate<T, I>
    {
        /// <summary>
        /// Creates the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        T Create(I data);
    }
}