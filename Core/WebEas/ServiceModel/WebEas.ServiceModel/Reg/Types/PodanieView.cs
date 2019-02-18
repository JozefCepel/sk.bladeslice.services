using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Podanie")]
    [DataContract]
    public class PodanieView : BaseTenantEntity
    {
        /// <summary>
        /// Gets or sets the d_ podanie_ id.
        /// </summary>
        /// <value>The d_ podanie_ id.</value>
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_Podanie_Id { get; set; }

        /// !->D_Tenant_Id 

        /// <summary>
        /// Gets or sets the e podatelna id.
        /// </summary>
        /// <value>The e podatelna id.</value>
        [PfeColumn(Hidden = true, Hideable = false, ReadOnly = true)]
        [DataMember]
        public long? ePodatelnaId { get; set; }


        /// <summary>
        /// Gets or sets the eform id.
        /// </summary>
        /// <value>The eform id.</value>
        #if DEBUG || DEVELOP || INT
        [PfeColumn(Text = "Namespace*", Hidden = true)]
        #endif
        [DataMember]
        public string eForm_Id { get; set; }

        /// <summary>
        /// Gets or sets the spis id.
        /// </summary>
        /// <value>The spis id.</value>
        [PfeColumn(Hidden = true, Hideable = false, Rank = 11)]
        [DataMember]
        public int SpisId { get; set; }

        /// <summary>
        /// Gets or sets the stav.
        /// </summary>
        /// <value>The stav.</value>
        [IgnoreDataMember]
        public string Stav { get; set; }

        /// <summary>
        /// Gets or sets the stav.
        /// </summary>
        /// <value>The stav.</value>

        /// !->Stav
        [DataMember]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počiatočný stav", Hidden = true, Editable = false, ReadOnly = true)]
        public bool JePociatocnyStav { get; set; }

        [DataMember]
        [PfeColumn(Text = "Konečný stav", Hidden = true, Editable = false, ReadOnly = true)]
        public bool JeKoncovyStav { get; set; }

        /// <summary>
        /// Gets or sets the modul.
        /// </summary>
        /// <value>The modul.</value>
        [IgnoreDataMember]
        public string Modul { get; set; }

        /// <summary>
        /// Gets or sets the c_ sluzba_ id.
        /// </summary>
        /// <value>The c_ sluzba_ id.</value>
        [DataMember]
        public int C_Sluzba_Id { get; set; }

        /// <summary>
        /// Gets or sets the sluzba.
        /// </summary>
        /// <value>The sluzba.</value>
        [PfeColumn(Text = "Služba", Width = 320, Rank = 1)]
        [DataMember]
        public string SluzbaNazov { get; set; }

        /// <summary>
        /// Gets or sets the uradny dokument nazov.
        /// </summary>
        /// <value>The uradny dokument nazov.</value>
        [PfeColumn(Text = "Názov dokumentu", Rank = 2)]
        [DataMember]
        public string UradnyDokumentNazov { get; set; }

        /// <summary>
        /// Gets or sets the registraturne cislo.
        /// </summary>
        /// <value>The registraturne cislo.</value>
        [PfeColumn(Text = "Registratúrne číslo", Rank = 3, Xtype = PfeXType.EPodDokumentLink, NameField = "ePodatelnaId")]
        [DataMember]
        public string RegistraturneCislo { get; set; }

        /// <summary>
        /// Gets or sets the stav dokumentu.
        /// </summary>
        /// <value>The stav dokumentu.</value>
        [PfeColumn(Text = "Stav podania", Rank = 4)]
        [PfeCombo(StavovyPriestorEnum.RZP_NavrhUprava)] // stav je len aby tam nieco bolo, oprav ak sa pouzije niekedy
        [DataMember]
        public string StavPodania { get; set; }

        [PfeColumn(Text = "Stav záznamu v spise")]
        [DataMember]
        public string StavZaznamuVSpise { get; set; }

        /// <summary>
        /// Gets or sets the typ dokumentu.
        /// </summary>
        /// <value>The typ dokumentu.</value>
        [PfeColumn(Text = "Typ dokumentu", Rank = 5)]
        [DataMember]
        public string TypDokumentu { get; set; }

        /// <summary>
        /// Gets or sets the zdroj dokumentu.
        /// </summary>
        /// <value>The zdroj dokumentu.</value>
        [PfeColumn(Text = "Zdroj dokumentu", Rank = 6)]
        [DataMember]
        public string ZdrojDokumentu { get; set; }

        /// <summary>
        /// Gets or sets the cislo spisu.
        /// </summary>
        /// <value>The cislo spisu.</value>
        [PfeColumn(Text = "Číslo spisu", Rank = 7, Xtype = PfeXType.FolderLink, NameField = "SpisId")]
        [DataMember]
        public string CisloSpisu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Elektronické podanie", Hidden = true, Editable = false, ReadOnly = true)]
        public bool ElektronickePodanie { get; set; }

        /// <summary>
        /// Gets or sets the riesitel.
        /// </summary>
        /// <value>The riesitel.</value>
        [PfeColumn(Hidden = true, Hideable = false)]
        [DataMember]
        public string Riesitel { get; set; }

        [PfeColumn(Text = "Riešiteľ", Editable = false, Rank = 8)]
        [DataMember]
        public string RiesitelMeno { get; set; }

        [PfeColumn(Text = "_Zberný spis", Hidden = true, Hideable = false, ReadOnly = true)]
        [IgnoreDataMember]
        public bool ZbernySpis { get; set; }

        /// <summary>
        /// Gets or sets the stav konania.
        /// </summary>
        /// <value>The stav konania.</value>
        [PfeColumn(Text = "Stav konania", Rank = 9)]
        [DataMember]
        public string StavKonania { get; set; }

        /// <summary>
        /// Gets or sets the typ konania.
        /// </summary>
        /// <value>The typ konania.</value>
        [PfeColumn(Text = "Typ konania", Rank = 10)]
        [DataMember]
        public string TypKonania { get; set; }

        [PfeColumn(Text = "Anonymné podanie", Editable = false, Hidden = true, Hideable = false)]
        [IgnoreDataMember]
        public bool Anonymne { get; set; }

        //search stlpce na audit stlpcoch --datumy su zdedene z predka 
        [PfeColumn(Text = "Vytvoril", Hidden = true, Rank = 13, Editable = false)]
        [DataMember]
        public string VytvorilMeno { get; set; }

        [PfeColumn(Text = "Zmenil", Hidden = true, Rank = 14, Editable = false)]
        [DataMember]
        public string ZmenilMeno { get; set; }

        [PfeColumn(Text = "Prijaté od", Editable = false)]
        [DataMember]
        public string PrijateOd { get; set; }

        [PfeColumn(Text = "Popis chyby", Editable = false, Hidden = true, Xtype = PfeXType.Textarea)]
        [DataMember]
        public string PopisChyby { get; set; }
    }
}