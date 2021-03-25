using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ServiceStack;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Base Dto
    /// </summary>
    [DataContract]
    public abstract class BaseDto<T> : IDto, IChangeIdentifier where T : class, IBaseEntity, new()
    {
        /// <summary>
        /// Gets or sets the poznamka.
        /// </summary>
        /// <value>The poznamka.</value>
        [DataMember]
        public string Poznamka { get; set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public Type EntityType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Nastavenie datumu zmeny z FE
        /// </summary>
        /// <value>The datum zmeny.</value>
        [DataMember(Name = "ci")]
        public long? ChangeIdentifierDto { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        public bool Validate(IWebEasRepositoryBase repository)
        {
            //tu posielame Operation.Insert ale kontrola na SqlValidationAttribute nezbehne
            Validator.Validate(this, Operation.Insert, repository, this.EntityType);
            return true;
        }

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        public T ConvertToEntity(T data)
        {
            data.Poznamka = this.Poznamka;
            data.ChangeIdentifierDto = this.ChangeIdentifierDto;

            if (this.GetType().GetProperties().Any(nav => nav.HasAttribute<ServiceStack.DataAnnotations.PrimaryKeyAttribute>()) && typeof(T).GetProperties().Any(nav => nav.HasAttribute<ServiceStack.DataAnnotations.PrimaryKeyAttribute>()))
            {
                PropertyInfo source = this.GetType().GetProperties().First(nav => nav.HasAttribute<ServiceStack.DataAnnotations.PrimaryKeyAttribute>());
                PropertyInfo dest = typeof(T).GetProperties().First(nav => nav.HasAttribute<ServiceStack.DataAnnotations.PrimaryKeyAttribute>());
                dest.SetValue(data, source.GetValue(this));
            }

            this.BindToEntity(data);

            return data;
        }

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <returns></returns>
        public T ConvertToEntity()
        {
            return this.ConvertToEntity(new T());
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="dbObject">The db object.</param>
        /// <returns></returns>
        public IBaseEntity GetEntity(IBaseEntity dbObject)
        {
            return this.ConvertToEntity((T)dbObject);
        }

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <returns></returns>
        public IBaseEntity GetEntity()
        {
            return this.ConvertToEntity();
        }

        /// <summary>
        /// Updates entity.
        /// </summary>
        /// <returns></returns>
        public void UpdateEntity(T data)
        {
            data.Poznamka = this.Poznamka;
            data.ChangeIdentifierDto = this.ChangeIdentifierDto;
            this.BindToEntity(data);
        }

        /// <summary>
        /// Binds to entity.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        protected abstract void BindToEntity(T data);
    }
}