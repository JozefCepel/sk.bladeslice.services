using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("D_TreePermission")]
    public class TreePermission : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "ID povolenia na strom")]
        public int D_TreePermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Právo")]
        public byte Pravo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rola")]
        public int? C_Role_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Používateľ")]
        public Guid? D_User_Id { get; set; }

    }
}
