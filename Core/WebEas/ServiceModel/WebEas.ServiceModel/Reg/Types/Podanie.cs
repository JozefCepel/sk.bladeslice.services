using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_Podanie")]
    public class Podanie : BaseTenantEntity
    {
        /// <summary>
        /// Gets or sets the d_ podanie_ id.
        /// </summary>
        /// <value>The d_ podanie_ id.</value>
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        public long D_Podanie_Id { get; set; }

        /// <summary>
        /// Gets or sets the e podatelna id.
        /// </summary>
        /// <value>The e podatelna id.</value>
        [DataMember]
        public long? ePodatelnaId { get; set; }

        /// <summary>
        /// Gets or sets the c_ sluzba_ id.
        /// </summary>
        /// <value>The c_ sluzba_ id.</value>
        [DataMember]
        public int C_Sluzba_Id { get; set; }

        /// <summary>
        /// Gets or sets the c_ stav entity_ id.
        /// </summary>
        /// <value>The c_ stav entity_ id.</value>
        [DataMember]
        public int C_StavEntity_Id { get; set; }

        /// <summary>
        /// Gets or sets the e form_ id.
        /// </summary>
        /// <value>The e form_ id.</value>
        [DataMember]
        public string eForm_Id { get; set; }

        /// <summary>
        /// Gets or sets the ulozene do evidencie pouzivatel.
        /// </summary>
        /// <value>The ulozene do evidencie pouzivatel.</value>
        [DataMember]
        public string UlozeneDoEvidenciePouzivatel { get; set; }

        /// <summary>
        /// Gets or sets the ulozene do evidencie datum.
        /// </summary>
        /// <value>The ulozene do evidencie datum.</value>
        [DataMember]
        public DateTime? UlozeneDoEvidencieDatum { get; set; }

        [DataMember]
        public bool Anonymne { get; set; }

        [DataMember]
        public string PopisChyby { get; set; }
    }
}