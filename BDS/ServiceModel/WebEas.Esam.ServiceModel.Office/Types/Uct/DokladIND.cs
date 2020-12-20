using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [Schema("uct")]
    [Alias("D_DokladIND")]
    [DataContract]
    public class DokladIND : BaseTenantEntity
    {
        [PrimaryKey]
        [DataMember]
        public long D_DokladIND_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id", ReadOnly = true)]   // pouzite len na vazbu medzi gridmi, musia mat rovnake ID, to iste co D_DokladIND_Id
        [Ignore]
        public long D_BiznisEntita_Id { get { return D_DokladIND_Id; } }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }


        //[DataMember]
        //[PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        //public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", Mandatory = true)]
        public short Rok { get; set; }
    }
}
