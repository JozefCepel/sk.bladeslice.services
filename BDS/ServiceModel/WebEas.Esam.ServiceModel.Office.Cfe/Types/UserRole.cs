using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_UserRole")]
    [DataContract]
    public class UserRole : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_UserRole_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id")]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Role_Id")]
        public int C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Date of assignment", Type = PfeDataType.Date)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Date taken", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

    }
}
