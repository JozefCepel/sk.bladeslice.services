using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("D_IntDoklad")]
    [DataContract]
    public class IntDoklad : BaseTenantEntity
    {
        [PrimaryKey]
        [DataMember]
        public long D_IntDoklad_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id", ReadOnly = true)]   // pouzite len na vazbu medzi gridmi, musia mat rovnake ID, to iste co D_IntDoklad_Id
        [Ignore]
        public long D_BiznisEntita_Id { get { return D_IntDoklad_Id; } }

        [DataMember]
        [PfeColumn(Text = "Počiatočný stav", ReadOnly = true)]
        public bool PS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma", Mandatory = true)]
        public decimal? Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", Mandatory = true)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_RzpPredkontacia_Id")]
        public int? C_RzpPredkontacia_Id { get; set; }

    }
}
