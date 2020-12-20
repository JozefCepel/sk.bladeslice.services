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
        [PfeColumn(Text = "Oddelenie")]
        public int C_OrsElement_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rola")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Používateľ")]
        public Guid? D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prístup")]
        public byte Pravo { get; set; }

    }
}

