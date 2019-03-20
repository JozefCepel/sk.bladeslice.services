using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("C_FREK")]
    [DataContract]
    public class FREKCis : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_FREK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Ekonomická klasifikácia")]
        public string EK { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string EKDot { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka")]
        public string EKPolozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podpoložka")]
        public string EKPodpolozka { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov účtu")]
        public string EKNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platný")]
        public bool Platny { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PrijemVydaj")]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        public int C_RzpTyp_Id { get; set; }
    }

}

