using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("dbo")]
    [Alias("K_ORJ_0")]
    [DataContract]
    public class tblK_ORJ_0 : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int K_ORJ_0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        public string KOD { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string ORJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rank", Mandatory = true)]
        [PfeSort(Rank = 1, Sort = PfeOrder.Asc)]
        public int Serial_No { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Poznámka")]
        public string POZN { get; set; }
    }
}
