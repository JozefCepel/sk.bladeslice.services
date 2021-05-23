using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Vyk
{
    [DataContract]
    public class Fin1PolBaseHelper: BaseTenantEntity
    {
        [DataMember]
        public bool Provizorium { get; set; }

        [DataMember]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        public string Cast { get; set; }

        [DataMember]
        [IgnoreInsertOrUpdate]
        public string RzpTypNazov { get; set; }

        [DataMember]
        public int? C_RzpTyp_Id { get; set; }

        [DataMember]
        public string ProgramKod { get; set; }

        [DataMember]
        public string DruhRzp { get; set; }

        [DataMember]
        public string ZdrojKod { get; set; }

        [DataMember]
        public string FK { get; set; }

        [DataMember]
        public string FKOddiel { get; set; }

        [DataMember]
        public string FKSkupina { get; set; }

        [DataMember]
        public string FKTrieda { get; set; }

        [DataMember]
        public string FKPodtrieda { get; set; }

        [DataMember]
        public string EK { get; set; }

        [DataMember]
        public string EKPolozka { get; set; }

        [DataMember]
        public string EKPodpolozka { get; set; }

        [DataMember]
        public string EKNazov { get; set; }

        [DataMember]
        public decimal? RzpSchvaleny { get; set; }

        [DataMember]
        public decimal? RzpZmeny { get; set; }

        [DataMember]
        public decimal? RzpUpraveny { get; set; }

        [DataMember]
        public decimal? RzpOcakavany { get; set; }

        [DataMember]
        public decimal? RzpSkutocnost { get; set; }

        [DataMember]
        [IgnoreInsertOrUpdate]
        public decimal? RzpRozdiel { get; set; }
    }
}
