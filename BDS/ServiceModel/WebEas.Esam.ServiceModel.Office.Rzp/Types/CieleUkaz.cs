using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("D_PRCieleUkaz")]
    [DataContract]
    public class CieleUkaz : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_PRCieleUkaz_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_PRCiele_Id")]
        public long D_PRCiele_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public byte? Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp")]
        public Guid? D_User_Id_Zodp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Komentár")]
        public string Komentar { get; set; }
    }
}
