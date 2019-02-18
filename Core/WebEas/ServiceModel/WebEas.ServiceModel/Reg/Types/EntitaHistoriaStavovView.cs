using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Entita_HistoriaStavov")]
    [DataContract]
    public class EntitaHistoriaStavovView : EntitaHistoriaStavov
    {
        [PfeColumn(Text = "Typ dokladu", Editable = false)]
        [DataMember]
        public string TypNazov { get; set; }

        [PfeColumn(Text = "Číslo dokladu", Editable = false)]
        [DataMember]
        public string CisloDokladu { get; set; }

        [DataMember]
        public string C_StavovyPriestor_Name { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pôvodný stav", Editable = false)]
        public string C_StavEntity_Old_Name { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenený stav", Editable = false)]
        public string C_StavEntity_New_Name { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}