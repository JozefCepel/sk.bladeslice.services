using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("reg")]
    [Alias("C_StavEntity")]
    [DataContract]
    public class StavEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the c_staventity_id.
        /// </summary>
        /// <value>The c_staventity_id.</value>
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_StavEntity_Id { get; set; }

        /// <summary>
        /// Gets or sets the c_StavovyPriestor_id.
        /// </summary>
        /// <value>The c_StavovyPriestor_id.</value>
        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        /// <summary>
        /// Gets or sets the kod.
        /// </summary>
        /// <value>The kod.</value>
        [DataMember]
        public string Kod { get; set; }

        /// <summary>
        /// Gets or sets the Strom.
        /// </summary>
        /// <value>The Strom.</value>
        [DataMember]
        public string Strom { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember]
        public string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the PociatocnyStav.
        /// </summary>
        /// <value>The PociatocnyStav.</value>
        [DataMember]
        public bool JePociatocnyStav { get; set; }

        /// <summary>
        /// Gets or sets the KoncovyStav.
        /// </summary>
        /// <value>The KoncovyStav.</value>
        [DataMember]
        public bool JeKoncovyStav { get; set; }

        /// <summary>
        /// Gets or sets the JeKladneVybavenie.
        /// </summary>
        /// <value>The JeKladneVybavenie.</value>
        [DataMember]
        public bool JeKladneVybavenie { get; set; }

        /// <summary>
        /// Gets or sets the C_Formular_Id.
        /// </summary>
        /// <value>The C_Formular_Id.</value>
        [DataMember]
        public int C_Formular_Id { get; set; }

        /// <summary>
        /// Gets or sets the textacia.
        /// </summary>
        /// <value>The Textacia.</value>
        [DataMember]
        public string Textacia { get; set; }

        /// <summary>
        /// Gets or sets the BiznisAkcia.
        /// </summary>
        /// <value>The BiznisAkcia.</value>
        [DataMember]
        public string BiznisAkcia { get; set; }

        /// <summary>
        /// Gets or sets the PovinnyDokument.
        /// </summary>
        /// <value>The PovinnyDokument.</value>
        [DataMember]
        public bool PovinnyDokument { get; set; }

        [Ignore]
        public List<StavEntity> NasledovneStavy { get; set; }

    }
}
