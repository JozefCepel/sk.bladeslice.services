using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [DataContract]
    [Schema("fin")]
    [Alias("V_UhradaParovanie_FA")]
    public class UhradaParovanieFaViewHelper
    {
        [DataMember]
        [PrimaryKey]
        public long? D_BiznisEntita_Id_Predpis { get; set; }

        [DataMember]
        public DateTime? DatumUhrady { get; set; }

        [DataMember]
        public decimal? Uhradene { get; set; }

        [DataMember]
        public string DokladUhrady { get; set; }
    }
}