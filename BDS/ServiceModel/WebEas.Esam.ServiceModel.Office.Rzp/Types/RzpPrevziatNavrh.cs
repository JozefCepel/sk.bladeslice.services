using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("F_RzpPrevziatNavrh (@Rok, @Mes)")]
    [DataContract]
    public class RzpPrevziatNavrh : Fin1PolBase
    {
        [DataMember]
        [PfeColumn(Text = "_C_RzpPol_Id", ReadOnly = true)]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Stredisko_Id", ReadOnly = true)]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id", ReadOnly = true)]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Návrh rozpočtu Rok+1", Mandatory = true)]
        public decimal NavrhRzp1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Návrh rozpočtu Rok+2", Mandatory = true)]
        public decimal NavrhRzp2 { get; set; }
    }
}
