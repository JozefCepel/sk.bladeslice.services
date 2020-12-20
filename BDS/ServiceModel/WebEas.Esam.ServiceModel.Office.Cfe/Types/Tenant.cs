using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_Tenant")]
    [DataContract]
    public class Tenant : BaseEntity
    {
        [PrimaryKey]
        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ", DefaultValue = 1)]
        public byte C_TenantType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OrganizaciaTyp")]
        public int C_OrganizaciaTyp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Server", Mandatory = true)]
        public string Server { get; set; }

        [DataMember]
        [PfeColumn(Text = "Databáza", Mandatory = true)]
        public string Databaza { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externý identif.")]
        [StringLength(36)]
        public Guid? D_Tenant_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "Identifikátor ISO")]
        public Guid? IsoId { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PO_Osoba_Id")]
        public long? D_PO_Osoba_Id { get; set; }
    }
}
