using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("bds")]
    [Alias("V_Nastavenie")]
    [DataContract]
    public class Nastavenie
    {
        [PrimaryKey]
        [DataMember]
        public string NazovId { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        [PfeLayoutDependency]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Hodnota")]
        public string Hodn { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public string Typ { get; set; }
    }
}
