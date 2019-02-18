using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Tenant")]
    [DataContract]
    public class TenantView : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public short C_TenantType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ")]
        [PfeCombo(typeof(TenantTypeView), NameColumn = "C_TenantType_Id")]
        public short TenantTypeName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]        
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Server")]
        public string Server { get; set; }

        [DataMember]
        [PfeColumn(Text = "Databáza")]
        public string Databaza { get; set; }
    }
}
