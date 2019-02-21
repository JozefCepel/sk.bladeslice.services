using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_RoleUsers")]
    [DataContract]
    public class RoleUsers : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_RoleUsers_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Užívateľ")]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Rola")]
        public int C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum pridelenia", Type = PfeDataType.Date)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum odobratia", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

    }
}
