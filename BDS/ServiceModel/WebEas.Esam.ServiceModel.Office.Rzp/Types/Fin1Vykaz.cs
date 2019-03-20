using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("F_Fin112 (@Rok, @Mes)")]
    [DataContract]
    public class Fin1Vykaz  : Fin1PolBase
    {
        // vazba na Dennik a Zmeny v rozpocte
        [PrimaryKey]
        [DataMember]
        [PfeColumn(Text = "_FinKey", ReadOnly = true)]
        public string FinKey { get; set; }

        // je v hlavicke ale tu to chceme zobrazit
        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public int Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obdobie", ReadOnly = true)]
        public short Obdobie { get; set; }

    }
}
