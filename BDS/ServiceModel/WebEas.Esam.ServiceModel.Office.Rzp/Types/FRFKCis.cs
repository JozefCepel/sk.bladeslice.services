using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("C_FRFK")]
    [DataContract]
    public class FRFKCis : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_FRFK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_FK komplet")]
        public string FK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string FKDot { get; set; }

        [DataMember]
        [PfeColumn(Text = "Oddiel")]
        public string FKOddiel { get; set; }

        [DataMember]
        [PfeColumn(Text = "Skupina")]
        public string FKSkupina { get; set; }

        [DataMember]
        [PfeColumn(Text = "Trieda")]
        public string FKTrieda { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podtrieda")]
        public string FKPodtrieda { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string FKNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platný")]
        public bool Platny { get; set; }
    }
}
