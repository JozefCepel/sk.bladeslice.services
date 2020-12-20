using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_StavEntity")]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class StavEntity : BaseEntity, IValidate
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
        [PfeColumn(Text = "Názov")]
        [PfeValueColumn]
        public string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the C_Formular_Id.
        /// </summary>
        /// <value>The C_Formular_Id.</value>
        [DataMember]
        [PfeColumn(Hidden = true)]
        public int? C_Formular_Id { get; set; }

        /// <summary>
        /// Gets or sets the PociatocnyStav.
        /// </summary>
        /// <value>The PociatocnyStav.</value>
        [DataMember]
        [PfeColumn(Text = "Počiatočný stav")]
        public bool JePociatocnyStav { get; set; }

        /// <summary>
        /// Gets or sets the KoncovyStav.
        /// </summary>
        /// <value>The KoncovyStav.</value>
        [DataMember]
        [PfeColumn(Text = "Koncový stav")]
        public bool JeKoncovyStav { get; set; }

        /// <summary>
        /// Gets or sets the JeKladneVybavenie.
        /// </summary>
        /// <value>The JeKladneVybavenie.</value>
        [DataMember]
        [PfeColumn(Text = "Kladné vybavenie")]
        public bool JeKladneVybavenie { get; set; }

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

        /// <summary>
        /// Validovanie dat pred insertom a updatom
        /// </summary>
        public void Validate()
        {
            //CK_C_StavEntity_KladneVybavenieLenKoncovy	(NOT ([JeKoncovyStav]<>(1) AND [JeKladneVybavenie]=(1)))
            if (this.JeKladneVybavenie && !this.JeKoncovyStav)
            {
                throw new WebEasValidationException("CK_C_StavEntity_KladneVybavenieLenKoncovy - (NOT ([JeKoncovyStav]<>(1) AND [JeKladneVybavenie]=(1)))", "Kladné vybavenie môže mať len koncový stav!");
            }
        }
    }

}