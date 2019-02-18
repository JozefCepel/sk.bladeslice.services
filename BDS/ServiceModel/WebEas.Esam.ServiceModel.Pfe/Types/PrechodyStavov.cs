using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("reg")]
    [Alias("C_StavEntity_StavEntity")]
    [DataContract]
    public class PrechodyStavov : BaseEntity
    {
        /// <summary>
        /// Gets or sets the C_StavEntity_Id_Parent.
        /// </summary>
        /// <value>The C_StavEntity_Id_Parent.</value>
        [PrimaryKey]
        [DataMember]
        public int C_StavEntity_Id_Parent { get; set; }

        /// <summary>
        /// Gets or sets the C_StavEntity_Id_Child.
        /// </summary>
        /// <value>The C_StavEntity_Id_Child.</value>
        [PrimaryKey]
        [DataMember]
        public int C_StavEntity_Id_Child { get; set; }

        /// <summary>
        /// Gets or sets the C_StavovyPriestor_Id.
        /// </summary>
        /// <value>The C_StavovyPriestor_Id.</value>
        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }
    }
}
