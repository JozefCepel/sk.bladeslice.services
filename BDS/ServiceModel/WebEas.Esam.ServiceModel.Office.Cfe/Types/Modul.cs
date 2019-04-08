using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("C_Modul")]
    [DataContract]
    public class Modul : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int C_Modul_Id { get; set; }                     

        [DataMember]
        [PfeColumn(Text = "Code", Mandatory = true)]        
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_URL")]
        public string BasicUrl { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Json")]
        public string TreeJson { get; set; }
    }
}
