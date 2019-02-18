using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_RoleUsers")]
    [DataContract]
    public class RoleUsersView : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_RoleUsers_Id { get; set; }

        [DataMember]
        public Guid D_User_Id { get; set; }

        [DataMember]
        public int C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum pridelenia", Hidden = true, Type = PfeDataType.DateTime, Editable = false, Rank = 103, ReadOnly = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum odobratia", Hidden = true, Type = PfeDataType.DateTime, Editable = false, Rank = 103, ReadOnly = true)]
        public DateTime? PlatnostDo { get; set; }

    }
}
