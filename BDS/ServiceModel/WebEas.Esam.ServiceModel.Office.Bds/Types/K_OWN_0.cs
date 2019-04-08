using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_OWN_0")]
    [DataContract]
    public class tblK_OWN_0 : BaseEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        public int K_OWN_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "IČO", Mandatory = true)]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "DIČ")]
        public string DRC { get; set; }

        [DataMember]
        [PfeColumn(Text = "IČ DPH")]
        public string IC_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name", Mandatory = true)]
        public string NAZOV1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2")]
        public string NAZOV2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ulica")]
        public string ULICA_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "PSČ")]
        public string PSC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obec", Mandatory = true)]
        public string OBEC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štát")]
        public string STAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 1 - Prevádzka")]
        public string NAZOV1_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2 - Prevádzka")]
        public string NAZOV2_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ulica - Prevádzka")]
        public string ULICA_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "PSČ - Prevádzka")]
        public string PSC_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obec - Prevádzka")]
        public string OBEC_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Štát - Prevádzka")]
        public string STAT_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok")]
        public string ROK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 1")]
        public string TEL1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telefón 2")]
        public string TEL2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fax")]
        public string FAX { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mobil")]
        public string MOBIL { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mail")]
        public string EMAIL { get; set; }

        [DataMember]
        [PfeColumn(Text = "WWW")]
        public string WWW { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platca dane z pridanej hodnoty")]
        public bool? PLATIC_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mesačný platca dane")]
        public bool? MES_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Metóda FIFO", Tooltip = "Skladová metóda FIFO alebo priemerné ceny")]
        public bool? FIFO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zapísaná v")]
        public string REGISTER1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pod číslom")]
        public string REGISTER2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_INDEX_DPH")] //Koeficient pomerného odpočítania dane
        public decimal? INDEX_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "_START_UCT")] //Mesiac v ktorom sa začína fiškálny rok
        public byte? START_UCT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_SKL_SO_UP")] //v rámci so a vyššie (pri false je iba v rámci so)
        public bool? SKL_SO_UP { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DPH_UCT_DIFF_UO")] //DPH účty daňových dokladov účtovať do UO podľa rozpisu DPH
        public bool? DPH_UCT_DIFF_UO { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VRB_ONLINE")] //Online výroba
        public bool? VRB_ONLINE { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vyskladňovať chronologicky")]
        public bool? SKL_CHRONOL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_VRB_DIRECT")]
        public bool? VRB_DIRECT { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KONCERN_NO")]
        public string KONCERN_NO { get; set; }

        [DataMember]
        [PfeColumn(Text = "SK NACE")]
        public string SK_NACE { get; set; }
    }
}
