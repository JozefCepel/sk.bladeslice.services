using System;
using System.Linq;
using WebEas.ServiceModel;

namespace WebEas.ServiceInterface
{
    /// <summary>
    /// Update data
    /// </summary>
    public interface IUpdate<T, I> where T: IBaseEntity where I: class
    {         
        /// <summary>
        /// Updates the multi.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        T Update(I data);
    }
}