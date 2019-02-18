using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_Fin1")]
    [DataContract]
    public class Fin1 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_Fin1_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public int Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Obdobie", ReadOnly = true)]
        public int Obdobie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum uloženia", ReadOnly = true)]
        public DateTime Datum { get; set; }

        [DataMember]
        [PfeColumn(Text = "PČ", ReadOnly = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum exportu", ReadOnly = true)]
        public DateTime ExportDatum { get; set; }

        [DataMember]
        [PfeColumn(Text = "Export", ReadOnly = true)]
        public bool Export { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", ReadOnly = true)]
        public string Popis { get; set; }

    }
}
