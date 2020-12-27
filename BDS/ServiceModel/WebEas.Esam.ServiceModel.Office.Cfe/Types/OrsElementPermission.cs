using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("D_OrsElementPermission")]
    public class OrsElementPermission : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int D_OrsElementPermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_OrsElement_Id")]
        public int C_OrsElement_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Role_Id")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id")]
        public Guid? D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Access")]
        public byte Pravo { get; set; }

    }
}

