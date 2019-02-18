using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Pfe.Types
{
    [Schema("pfe")]
    [Alias("V_Pohlad")]
    [TenantUpdatable]
    [DataContract]
    public class PohladList : ITenantEntityNullable
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        [Alias("D_Pohlad_Id")]
        public int Id { get; set; }

        [DataMember]
        [PfeSort(Rank = 3, Sort = PfeOrder.Asc)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeSort(Rank = 2, Sort = PfeOrder.Desc)]
        public string Typ { get; set; }

        [DataMember]
        [PfeSort(Rank = 1, Sort = PfeOrder.Desc)]
        public int ViewSharing { get; set; }

        [DataMember]
        public bool ShowInActions { get; set; }

        [DataMember]
        public bool DefaultView { get; set; }

        [IgnoreDataMember]
        public string KodPolozky { get; set; }

        [IgnoreDataMember]
        public Guid? D_Tenant_Id { get; set; }

        [IgnoreDataMember]
        public string Vytvoril { get; set; }
    }
}