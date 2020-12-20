using ServiceStack.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("reg")]
    [Alias("V_Nastavenie")]
    [DataContract]
    public class NastavenieView : Nastavenie, IPfeCustomize
    {
        [PfeColumn(Text = "Modul")]
        [DataMember]
        public string Modul { get; set; }

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
