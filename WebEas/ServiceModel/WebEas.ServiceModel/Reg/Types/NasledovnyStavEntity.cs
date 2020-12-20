using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Reg.Types
{
    [Schema("reg")]
    [Alias("C_StavEntity_StavEntity")]
    [DataContract]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class NasledovnyStavEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the c_staventity_id_parent.
        /// </summary>
        /// <value>The c_staventity_id_parent.</value>
        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public int C_StavEntity_Id_Parent { get; set; }

        /// <summary>
        /// Gets or sets the c_staventity_id_child.
        /// </summary>
        /// <value>The c_staventity_id_child.</value>
        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public int C_StavEntity_Id_Child { get; set; }

        /// <summary>
        /// Gets or sets the C_StavovyPriestor_Id.
        /// </summary>
        /// <value>The C_StavovyPriestor_Id.</value>
        [PrimaryKey]
        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        [Ignore]
        public StavEntityView NasledovnyStav { get; set; }
    }
}