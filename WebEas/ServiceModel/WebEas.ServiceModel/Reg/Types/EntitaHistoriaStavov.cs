using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("D_Entita_HistoriaStavov")]
    [DataContract]
    public class EntitaHistoriaStavov : BaseTenantEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        [DataMember]
        public long D_Entita_HistoriaStavov_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum zmeny")]
        public DateTime DatumZmenaStavu { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_StavovyPriestor_Id")]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id_Old")]
        public int? C_StavEntity_Id_Old { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id_New")]
        public int C_StavEntity_Id_New { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id")]
        public long? D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Id")]
        public short? C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Rzp_Id")]
        public long? D_Rzp_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Modul_Id")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vyjadrenie spracovateľa", Xtype = PfeXType.TextareaWW, LoadWhenVisible = true)]
        public string VyjadrenieSpracovatela { get; set; }
    }
}