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
        [IgnoreInsertOrUpdate]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public short C_TenantType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]        
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Server")]
        public string Server { get; set; }

        [DataMember]
        [PfeColumn(Text = "Databáza")]
        public string Databaza { get; set; }
    }
}
