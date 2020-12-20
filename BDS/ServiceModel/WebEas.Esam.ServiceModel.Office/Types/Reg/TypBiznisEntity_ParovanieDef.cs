using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_TypBiznisEntity_ParovanieDef")]
    public class TypBiznisEntity_ParovanieDef : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_TypBiznisEntity_ParovanieDef_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Id_1", Mandatory = true)]
        public short C_TypBiznisEntity_Id_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Id_2", Mandatory = true)]
        public short C_TypBiznisEntity_Id_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_ParovanieTyp", Mandatory = true)]
        public byte ParovanieTyp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(100)]
        public string Nazov { get; set; }
    }

    [Flags]
    public enum ParovanieTypEnum
    {  
        par_1_1 = 1,
        par_1_N = 2,
        par_N_1 = 3,
        par_N_M = 4
    }
}
