using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_BiznisEntita_Parovanie")]
    public class BiznisEntita_Parovanie : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_D_BiznisEntita_Parovanie_Id")]
        public long D_BiznisEntita_Parovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_ParovanieDef_Id", Mandatory = true)]
        public int C_TypBiznisEntity_ParovanieDef_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id_1", Mandatory = true)]
        public long D_BiznisEntita_Id_1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id_2", Mandatory = true)]
        public long D_BiznisEntita_Id_2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true, Editable = false)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Identifikátor", ReadOnly = true, Editable = false)]
        public long Identifikator { get; set; }

        [DataMember]
        [PfeColumn(Text = "Plnenie", Format = "0| %", DecimalPlaces = 0, DefaultValue = 100)]
        public decimal Plnenie { get; set; }
    }
}
