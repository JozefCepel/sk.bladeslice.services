using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("reg")]
    [Alias("D_Nastavenie")]
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
        [PfeColumn(Text = "Description", Xtype = PfeXType.TextareaWW)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Value")]
        public string Hodn { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "User setting", ReadOnly = true, Editable = false)]
        public bool Pouzivatel { get; set; }
    }
}
