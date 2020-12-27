using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("D_OrsElementTypePermission")]
    public class OrsElementTypePermission : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]        
        public int D_OrsElementTypePermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_OrsElementType_Id")]
        public int C_OrsElementType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Role")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id")]
        public Guid? D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Access")]
        public byte Pravo { get; set; }

    }
}

