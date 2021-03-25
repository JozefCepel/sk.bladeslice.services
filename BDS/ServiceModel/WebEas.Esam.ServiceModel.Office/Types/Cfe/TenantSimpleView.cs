using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Tenant")]
    [DataContract]
    public class TenantSimpleView : ITenantEntity
    {
        // pouzite na zostavy, spolocna procka RptSetOwner pre vsetky moduly
        [DataMember]
        public Guid D_Tenant_Id { get; set; }
        [DataMember]
        public string ICO { get; set; }
        [DataMember]
        public string MenoObchodne { get; set; }
        [DataMember]
        public string AdresaUlicaCislo { get; set; }
        [DataMember]
        public string AdresaPSC { get; set; }
        [DataMember]
        public string AdresaObec { get; set; }
        [DataMember]
        public byte C_OrganizaciaTypDetail_Id { get; set; }
        [DataMember]
        public byte C_OrganizaciaTyp_Id { get; set; }
        [DataMember]
        public string OrganizaciaTypNazov { get; set; }
        [DataMember]
        public string OrganizaciaTypKod { get; set; }
        [DataMember]
        public string KodObce { get; set; }
    }
}
