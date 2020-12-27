using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_OBP_0")]
    [DataContract]
    public class tblK_OBP_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int K_OBP_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_TOB_0")]
        public int K_TOB_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_PRF_0")]
        public int K_PRF_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_OPC_0")]
        public int K_OPC_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_K_OPK_0")]
        public int? K_OPK_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Business ID No.", Mandatory = true)]
        public string ICO { get; set; }

        [DataMember]
        [PfeColumn(Text = "Tax ID")]
        public string DRC { get; set; }

        [DataMember]
        [PfeColumn(Text = "VAT Reg. No.")]
        public string IC_DPH { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name", Mandatory = true)]
        public string NAZOV1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2")]
        public string NAZOV2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Street")]
        public string ULICA_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZIP code")]
        public string PSC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "City", Mandatory = true)]
        public string OBEC_S { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 1 - Operation")]
        public string NAZOV1_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name 2 - Operation")]
        public string NAZOV2_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "Street - Operation")]
        public string ULICA_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "ZIP code - Operation")]
        public string PSC_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "City - Operation")]
        public string OBEC_P { get; set; }

        [DataMember]
        [PfeColumn(Text = "State")]
        public string STAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Internal Name")]
        public string NICKNAME { get; set; }

        [DataMember]
        [PfeColumn(Text = "Business partner ID", Mandatory = true)]
        public string OBP_C { get; set; }

        [DataMember]
        [PfeColumn(Text = "_DEALER")]
        public int? DEALER { get; set; }

        [DataMember]
        [PfeColumn(Text = "Payment due", DefaultValue = 0)]
        public short? SPLAT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Discount", DefaultValue = 0)]
        public decimal? ZLAVA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PENALE", DefaultValue = 0)]
        public decimal? PENALE { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KREDIT", DefaultValue = 0)]
        public int KREDIT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Person")]
        public string OSOBA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Function")]
        public string FUNKCIA { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telephone 1")]
        public string TEL1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Telephone 2")]
        public string TEL2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Fax")]
        public string FAX { get; set; }

        [DataMember]
        [PfeColumn(Text = "Mobile")]
        public string MOBIL { get; set; }

        [DataMember]
        [PfeColumn(Text = "E-mail")]
        public string EMAIL { get; set; }

        [DataMember]
        [PfeColumn(Text = "WWW")]
        public string WWW { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OBP_POTENCIAL", DefaultValue = 0)]
        public int OBP_POTENCIAL { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Doplňujúci text")]
        public string DOPL_TEXT { get; set; }
    }
}
