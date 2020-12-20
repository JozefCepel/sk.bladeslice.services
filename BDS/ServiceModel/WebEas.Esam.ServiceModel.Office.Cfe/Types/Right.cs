using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_Right")]
    public class Right : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_Right_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "C_Modul_Id")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string Nazov { get; set; }

    }
}

