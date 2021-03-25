using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("D_TypBiznisEntityNastav")]
    [DataContract]
    public class TypBiznisEntityNastav : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_TypBiznisEntityNastav_Id { get; set; }

        [DataMember]
        [PfeColumn(Mandatory = true)]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko na položke")]
        public bool? StrediskoNaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt na položke")]
        public bool? ProjektNaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účt. kľúč 1 na položke")]
        public bool? UctKluc1NaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účt. kľúč 2 na položke")]
        public bool? UctKluc2NaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účt. kľúč 3 na položke")]
        public bool? UctKluc3NaPolozke { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidovať v DMS")]
        public bool? EvidenciaDMS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Evidovať v systéme")]
        public bool? EvidenciaSystem { get; set; }

        [DataMember]
        [PfeColumn(Text = "Jedno číslovanie")]
        public bool? CislovanieJedno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovať položkovite")]
        public bool? UctovatPolozkovite { get; set; }

    }
}
