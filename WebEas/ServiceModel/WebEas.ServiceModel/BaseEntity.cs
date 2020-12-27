using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Base Entity
    /// </summary>
    [DataContract]
    public abstract class BaseEntity : IBaseEntity, IAccessFlag
    {
        private object id = null;

        /// <summary>
        /// Gets or sets the vytvoril.
        /// </summary>
        /// <value>The vytvoril.</value>
        [IgnoreDataMember]
        [PfeIgnore]
        public Guid? Vytvoril { get; set; }

        /// <summary>
        /// Gets or sets the datum vytvorenia.
        /// </summary>
        /// <value>The datum vytvorenia.</value>
        [DataMember]
        [PfeSort(Sort = PfeOrder.Desc, Rank = 99)]
        [PfeColumn(Text = "Creation time", Hidden = true, Type = PfeDataType.DateTime, Editable = false, ReadOnly = true)]
        public DateTime DatumVytvorenia { get; set; }

        /// <summary>
        /// Gets or sets the zmenil.
        /// </summary>
        /// <value>The zmenil.</value>
        [IgnoreDataMember]
        [PfeIgnore]
        public Guid? Zmenil { get; set; }

        /// <summary>
        /// Gets or sets the datum zmeny.
        /// </summary>
        /// <value>The datum zmeny.</value>
        [DataMember]
        [PfeColumn(Text = "Time of change", Hidden = true, Type = PfeDataType.DateTime, Editable = false, ReadOnly = true)]
        public DateTime DatumZmeny { get; set; }

        /// <summary>
        /// Gets or sets the poznamka.
        /// </summary>
        /// <value>The poznamka.</value>
        [DataMember]
        [PfeColumn(Text = "Note", Hidden = true, Xtype = PfeXType.Textarea)]
        [StringLength(512)]
        public string Poznamka { get; set; }

        /// <summary>
        /// Gets or sets the access flag.
        /// </summary>
        /// <value>The access flag.</value>
        [Ignore]
        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false, Type = PfeDataType.Number, Editable = false, ReadOnly = true)]
        public long AccessFlag { get; set; }

        /// <summary>
        /// Gets the change identifier.
        /// </summary>
        /// <value>The change identifier.</value>
        [Ignore]
        [DataMember(Name = "ci")]
        [PfeColumn(Hidden = true, Hideable = false, Type = PfeDataType.Number, Editable = false)]
        public long ChangeIdentifier
        {
            get
            {
                return DatumZmeny.ToIdentifier();
            }
            set { }//blbne deserializacia
        }

        /// <summary>
        /// Datum zmeny z FE. Sluzi na kontrolu konkurencneho pristupu, ci nahodou medzitym niekto nezmenil udaje
        /// </summary>
        /// <value>The change date.</value>
        [Ignore]
        [IgnoreDataMember]
        public long? ChangeIdentifierDto { get; set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <returns></returns>
        public object GetId()
        {
            if (this.id == null || (this.id is int && ((int)this.id) == 0) || (this.id is long && ((long)this.id) == 0L) || (this.id is short && ((short)this.id) == (short)0))
            {
                Type type = this.GetType();
                if (type.GetProperties().Any(nav => nav.HasAttribute<PrimaryKeyAttribute>()))
                {
                    PropertyInfo prop = type.GetProperties().First(nav => nav.HasAttribute<PrimaryKeyAttribute>());
                    this.id = prop.GetValue(this);
                }
            }
            return this.id;
        }

        /// <summary>
        /// Optimalizacia sql update - updatujeme len zmenene property
        /// </summary>
        [Ignore]
        [IgnoreDataMember]
        public object DefaultRecord { get; set; }
    }
}