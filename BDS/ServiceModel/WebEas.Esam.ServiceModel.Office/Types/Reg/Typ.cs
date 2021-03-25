using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_Typ")]
    public class Typ : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Identifikátor")]
        public int C_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        [StringLength(40)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položky dokladov", Mandatory = true, Editable = false)]
        public bool Polozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "_RzpDefinicia", Mandatory = true)]
        public int RzpDefinicia { get; set; }
    }

}
