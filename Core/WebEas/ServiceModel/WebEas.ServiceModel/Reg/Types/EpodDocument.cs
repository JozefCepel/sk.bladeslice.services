using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_EpodDocument")]
    [DataContract]
    public class EpodDocument : ITenantEntity, IAccessFlag
    {
        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public long? D_Podanie_Id { get; set; }

        [IgnoreDataMember]
        public string StateEn { get; set; }

        /// <summary>
        /// Gets or sets the e podatelna id.
        /// </summary>
        /// <value>The e podatelna id.</value>
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long ePodatelnaId { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public int? SpisId { get; set; }

        /// <summary>
        /// Gets or sets the document source.
        /// </summary>
        /// <value>The document source.</value>
        [DataMember(Name = "documentsource")]
        [PfeColumn(Text = "Zdroj")]
        public string DocumentSource { get; set; }

        /// <summary>
        /// Gets or sets the type of the document.
        /// </summary>
        /// <value>The type of the document.</value>
        [DataMember(Name = "documenttype")]
        [PfeColumn(Text = "Typ")]
        public string DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Name = "documentname")]
        [PfeColumn(Text = "Name dokumentu")]
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [DataMember(Name = "state")]
        [PfeColumn(Text = "Stav záznamu v spise")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the registry number.
        /// </summary>
        /// <value>The registry number.</value>
        [DataMember(Name = "registrynumber")]
        [PfeColumn(Text = "Reg. číslo", Xtype = PfeXType.EPodDokumentLink, NameField = "ePodatelnaId")]
        public string RegistryNumber { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo spisu", Xtype = PfeXType.FolderLink, NameField = "SpisId")]
        public string CisloZapisu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Riešiteľ")]
        public string RiesitelMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stav spisu")]
        public string SpisStav { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prijaté od", ReadOnly = true, Xtype = PfeXType.PersonLink, NameField = "IdOsoba")]
        public string FormatovaneMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Trvalý pobyt")]
        public string AdresaTp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Elektronické podanie", Hidden = true)]
        public bool ElektronickePodanie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum prijatia", Type = PfeDataType.Date)]
        public DateTime? DatumPrijatia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul")]
        public string Modul { get; set; }

        [DataMember]
        [PfeColumn(Text = "Služba")]
        public string SluzbaNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_formId")]
        public string formId { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Sluzba_Id", Hidden = true)]
        public int C_Sluzba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_IdOsoba", Hidden = true)]
        public Guid? IdOsoba { get; set; }

        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        /// <summary>
        /// Gets or sets the access flag.
        /// </summary>
        /// <value>The access flag.</value>
        [Ignore]
        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false, Type = PfeDataType.Number, Editable = false, ReadOnly = true)]
        public long AccessFlag { get; set; }
    }
}