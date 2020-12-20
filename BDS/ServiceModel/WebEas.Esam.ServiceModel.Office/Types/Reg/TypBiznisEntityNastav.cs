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
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_TypBiznisEntityNastav_Id { get; set; }

        [PrimaryKey]
        [PfeColumn(Mandatory = true)]
        [DataMember]
        public short C_TypBiznisEntity_Id { get; set; }

        [PfeColumn(Text = "Stredisko na položke")]
        [DataMember]
        public bool? StrediskoNaPolozke { get; set; }

        [PfeColumn(Text = "Evidovať v DMS")]
        [DataMember]
        public bool? EvidenciaDMS { get; set; }

        [PfeColumn(Text = "Evidovať v systéme")]
        [DataMember]
        public bool? EvidenciaSystem { get; set; }

        [DataMember]
        [PfeColumn(Text = "Jedno číslovanie")]
        public bool? CislovanieJedno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovať položkovite")]
        public bool? UctovatPolozkovite { get; set; }
      
    }
}
