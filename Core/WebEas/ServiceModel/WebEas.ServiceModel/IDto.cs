using System;
using System.Linq;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Dto base interface
    /// </summary>
    public interface IDto
    {
        /// <summary>
        /// Gets or sets the poznamka.
        /// </summary>
        /// <value>The poznamka.</value>
        string Poznamka { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        bool Validate();

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        IBaseEntity GetEntity();

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        IBaseEntity GetEntity(IBaseEntity dbObject);

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        Type EntityType { get; }
    }
}