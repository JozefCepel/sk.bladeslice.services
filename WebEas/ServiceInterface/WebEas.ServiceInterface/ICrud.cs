using System;
using System.Linq;
using WebEas.ServiceModel;

namespace WebEas.ServiceInterface
{
    /// <summary>
    /// CRUD operations
    /// </summary>
    public interface ICrud<Entity, Create, Update, Delete> : ICreate<Entity, Create>, IUpdate<Entity, Update>, IDelete<Delete>
        where Entity : IBaseEntity
        where Create : class
        where Update : class
        where Delete : class
    {
    }
}