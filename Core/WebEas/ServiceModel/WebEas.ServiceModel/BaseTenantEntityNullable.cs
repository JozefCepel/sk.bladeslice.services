using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Pouziva pri filtrovani a zadavani tenanta
    /// </summary>
    [DataContract]
    [Serializable]
    public class BaseTenantEntityNullable : BaseEntity, IBaseTenantEntityNullable
    {
        /// <summary>
        /// Gets or sets the d_ tenant_ id.
        /// </summary>
        /// <value>The d_ tenant_ id.</value>
        [IgnoreDataMember]
        public Guid? D_Tenant_Id { get; set; }
                
        /// <summary>
        /// Gets the global record.
        /// </summary>
        /// <value>The global record.</value>
        [Ignore]
        [DataMember]
        [PfeColumn(Text = "Globálny záznam", Editable = false, Rank = 99, ReadOnly = true)]        
        public bool GlobalRecord
        {
            get
            {
                return this.D_Tenant_Id == null;
            }
        }
    }
}