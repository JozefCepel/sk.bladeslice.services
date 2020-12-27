using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("D_TreePermission")]
    public class TreePermission : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_D_TreePermission_Id")]
        public int D_TreePermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Module")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Access")]
        public byte Pravo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Role_Id")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id")]
        public Guid? D_User_Id { get; set; }

    }
}
