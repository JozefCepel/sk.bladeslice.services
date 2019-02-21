using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

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
        [PfeColumn(Text = "Dátum zmeny stavu", Editable = false)]
        public DateTime ZmenaStavuDatum { get; set; }

        [DataMember]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public int? C_StavEntity_Id_Old { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false)]
        public int C_StavEntity_Id_New { get; set; }

        [DataMember]
        public long? D_BiznisEntita_Id { get; set; }

        [DataMember]
        public int? C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vyjadrenie spracovateľa", Xtype = PfeXType.TextareaWW, Editable = false)]
        public string VyjadrenieSpracovatela { get; set; }
    }
}