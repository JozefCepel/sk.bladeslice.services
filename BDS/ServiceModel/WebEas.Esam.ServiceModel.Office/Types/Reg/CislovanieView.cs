using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using System.Linq;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_Cislovanie")]
    [DataContract]
    public class CislovanieView : CislovanieCis, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ dokladu", ReadOnly = true, Editable = false)]
        public string TypDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kniha", ReadOnly = true, Editable = false)]
        public string KnihaKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", ReadOnly = true, Editable = false)]
        public byte Poradie { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "Stredisko", ReadOnly = true, Editable = false)]
        public string StrediskoNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Bank. účet", ReadOnly = true, Editable = false)]
        [PfeValueColumn]
        public string BankaUcetNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pokladnica", ReadOnly = true, Editable = false)]
        [PfeValueColumn]
        public string PokladnicaNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prvok ORŠ", ReadOnly = true, Editable = false)]
        [PfeValueColumn]
        public string ElementORS { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            if (model.Fields != null)
            {
                model.Fields.First(p => p.Name == nameof(StrediskoNazov)).Text = repository.GetNastavenieS("reg", "OrjNazovJC");
            }
        }
    }
}
