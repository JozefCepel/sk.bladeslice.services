using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_Parcela")]
    public class Parcela : BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public long D_Parcela_Id { get; set; }

        [DataMember]        
        public int C_Ku_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kmeň")]
        public int CpaKmen { get; set; }

        [DataMember]
        [PfeColumn(Text = "Poddelenie")]
        public short CpaPoddel { get; set; }

        [DataMember]
        [PfeColumn(Text = "Diel")]
        public short CpaDiel { get; set; }

        [DataMember]
        [PfeColumn(Text = "Č.pôv.kat.úz.")]
        public byte? KnCpu { get; set; }

        [DataMember]
        [Compute]
        [PfeColumn(Text = "Register", ReadOnly = true, Editable = false)]
        //[Required]
        public char cRegister { get; set; }

        [DataMember]
        [PfeColumn(Text = "Majetok obce")]
        public bool JeMajetkomObce { get; set; }
    }

    [DataContract]
    [Schema("reg")]
    [Alias("V_Parcela")]
    public class ParcelaSimpleView
    {
        [PrimaryKey]
        [DataMember]
        public long D_Parcela_Id { get; set; }

        [DataMember]
        public int C_Ku_Id { get; set; }

        [DataMember]
        public int CpaKmen { get; set; }

        [DataMember]
        public short CpaPoddel { get; set; }

        [DataMember]
        public short CpaDiel { get; set; }

        [DataMember]
        public byte? KnCpu { get; set; }

        [DataMember]
        public char cRegister { get; set; }

        [DataMember]
        public bool JeMajetkomObce { get; set; }

        [DataMember]
        public string Cislo { get; set; }

        [DataMember]
        public string KuNazov { get; set; }
    }
}
