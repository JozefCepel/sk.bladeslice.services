using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("C_Module")]
    [DataContract]
    public class Module : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int C_Module_Id { get; set; }                     

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]        
        public string ModulKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string ModulNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_URL")]
        public string BasicUrl { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Json")]
        public string TreeJson { get; set; }
    }
}
