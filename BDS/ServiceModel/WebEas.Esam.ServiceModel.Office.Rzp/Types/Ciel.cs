using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_PRCiel")]
    [DataContract]
    public class Ciel : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_PRCiel_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Program_Id")]
        public long D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public byte? Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp")]
        public Guid? D_User_Id_Zodp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [PfeValueColumn]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.Textarea)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Komentár", Xtype = PfeXType.Textarea)]
        public string Komentar { get; set; }
    }
}
