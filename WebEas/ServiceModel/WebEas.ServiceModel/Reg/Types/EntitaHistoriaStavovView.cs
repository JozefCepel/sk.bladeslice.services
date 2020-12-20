using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Entita_HistoriaStavov")]
    [DataContract]
    public class EntitaHistoriaStavovView : EntitaHistoriaStavov, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ dokladu", LoadWhenVisible = true)]
        public string TypNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo dokladu")]
        public string CisloInterne { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul")]
        public string Modul { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov stavového priestoru")]
        public string C_StavovyPriestor_Name { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pôvodný stav")]
        public string C_StavEntity_Old_Name { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenený stav")]
        public string C_StavEntity_New_Name { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vykonal", LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Zmenil", LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            if (node.KodRoot != "reg" && model?.Fields != null)
            {
                model.Fields.First(p => p.Name == nameof(Modul)).Text = "_Modul";
                model.Fields.First(p => p.Name == nameof(Modul)).LoadWhenVisible = true;
            }
        }
    }
}