using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    //Class pre reg.FT_KAT_BYT
    [DataContract]
    public class KatBytView : ITenantEntity
    {
        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        public Guid? D_Prizn_IdOsoba { get; set; }

        [DataMember]
        public int KuCode { get; set; }

        [DataMember]
        public int LvNo { get; set; }

        [DataMember]
        public string CisloParcely { get; set; }

        [DataMember]
        public byte? PKU { get; set; }

        [DataMember]
        public int? P1 { get; set; }

        [DataMember]
        public short? P2 { get; set; }

        [DataMember]
        public byte? P3 { get; set; }

        [DataMember]
        public string HouseNo { get; set; }

        [DataMember]
        public string EntranceLabel { get; set; }

        [DataMember]
        public string FloorNo { get; set; }

        [DataMember]
        public string FlatNo { get; set; }

        [DataMember]
        public long? C_Sadzba_Id { get; set; }

        [DataMember]
        public short? Rok { get; set; }
    }
}