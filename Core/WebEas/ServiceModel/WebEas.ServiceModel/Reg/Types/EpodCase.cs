using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_EpodCase")]
    [DataContract]
    public class EpodCase : ITenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public int SpisId { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum vytvorenia")]
        public DateTime datumVytvorenia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zberný spis")]
        public bool Zberny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stav spisu")]
        public string SpisStav { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo spisu", Xtype = PfeXType.FolderLink, NameField = "SpisId")]
        public string CisloZapisu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vec")]
        public string Vec { get; set; }

        [DataMember]
        [PfeColumn(Text = "Riešiteľ")]
        public string RiesitelMeno { get; set; }

        [DataMember]
        public Guid D_Tenant_Id { get; set; }
    }
}