using System;
using System.Linq;
using WebEas.ServiceModel;

namespace WebEas.ServiceInterface
{
    /// <summary>
    /// CRUD operations
    /// </summary>
    public interface ICrud<T, I> : ICreate<T, I>, IUpdate<T, I>
        where T : IBaseEntity
        where I : class
    {
    }
}