using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Cislovanie")]
    public class CislovanieCis : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "Identifikátor")]
        public int C_Cislovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ číslovania")]
        public bool CislovanieJedno { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Kniha dokladov")]
        [HierarchyNodeParameter]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Bank. účet")]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pokladnica")]
        public int? C_Pokladnica_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Interné číslo")]
        [StringLength(50)]
        public string CisloInterneMask { get; set; }

        [DataMember]
        [PfeColumn(Text = "Variabilný symbol")]
        [StringLength(50)]
        public string VSMask { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        [StringLength(100)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Úvodný text na doklade", Xtype = PfeXType.RichTextRpt)]
        public string UT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Záverečný text na doklade", Xtype = PfeXType.RichTextRpt)]
        public string ZT { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}