using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("zdv_int")]
    [Alias("EPOD_UradnyDokument")]
    public class PodanieEpodView
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        [Alias("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the e sluzba.
        /// </summary>
        /// <value>The e sluzba.</value>
        [DataMember]
        [Alias("eSluzba")]
        public string Sluzba { get; set; }

        /// <summary>
        /// Gets or sets the nazov.
        /// </summary>
        /// <value>The nazov.</value>
        [DataMember]
        [Alias("nazov")]
        public string Nazov { get; set; }

        /// <summary>
        /// Gets or sets the form id.
        /// </summary>
        /// <value>The form id.</value>
        [DataMember]
        [Alias("formId")]
        public string FormId { get; set; }

        /// <summary>
        /// Gets or sets the prijate od.
        /// </summary>
        /// <value>The prijate od.</value>
        [DataMember]
        [Alias("prijateOd")]
        public string PrijateOd { get; set; }

        /// <summary>
        /// Sposob dorucenia
        /// </summary>
        [DataMember]
        [Alias("sposobDorucenia")]
        public string SposobDorucenia { get; set; }

        /// <summary>
        /// Registraturne Cislo
        /// </summary>
        [DataMember]
        public string RegistraturneCislo { get; set; }

        /// <summary>
        /// Spis id
        /// </summary>
        [DataMember]
        public long? spisId { get; set; }

        /// <summary>
        /// Suvisiaci dokument id
        /// </summary>
        [DataMember]
        public long? suvisiaciDokumentId { get; set; }
    }
}