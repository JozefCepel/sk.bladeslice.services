using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_TypBiznisEntity_Typ")]
    public class TypBiznisEntityTyp : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_TypBiznisEntity_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ", Mandatory = true)]
        public int C_Typ_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu", Mandatory = true)]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Konkrétna kniha")]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true, DefaultValue = 0)]
        public short Poradie { get; set; }
    }
}
