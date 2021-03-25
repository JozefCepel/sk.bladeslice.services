using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_TypBiznisEntity")]
    public class TypBiznisEntityCis : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [PfeValueColumn]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string KodORS { get; set; }

        [DataMember]
        public bool Uctovany { get; set; }

        [DataMember]
        public bool StrediskoNaPolozke { get; set; }

        [DataMember]
        public bool ProjektNaPolozke { get; set; }

        [DataMember]
        public bool UctKluc1NaPolozke { get; set; }

        [DataMember]
        public bool UctKluc2NaPolozke { get; set; }

        [DataMember]
        public bool UctKluc3NaPolozke { get; set; }

        [DataMember]
        public bool EvidenciaDMS { get; set; }

        [DataMember]
        public bool EvidenciaSystem { get; set; }

        [DataMember]
        public bool CislovanieJedno { get; set; }
    }
}
