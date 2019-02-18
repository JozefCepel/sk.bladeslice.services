using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Types
{
    [Schema("reg")]
    [Alias("V_CIS_Kataster")]
    [PfeCaption("Katastrálne územie")]
    [CodeListCode("dcom_CadastralArea")]
    [DataContract]
    public class CisKataster : ITenantEntityNullable
    {
        [DataMember]
        [PfeColumn(Text = "Názov obce")]
        [PfeSort(Rank = 2)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Id obce")]
        public int C_Obec_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Tenant id")]
        public Guid? D_Tenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Id katastrálne územie")]
        [PrimaryKey]
        public int C_Ku_Id { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        [PfeSort(Rank = 1)]
        public int TenantSort { get; set; }
    }
}