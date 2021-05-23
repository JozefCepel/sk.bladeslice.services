using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("reg")]
    [Alias("V_Nastavenie")]
    [DataContract]
    public class NastavenieView : Nastavenie, IPfeCustomize, IAfterGetList
    {
        [PfeColumn(Text = "Modul")]
        [DataMember]
        public string Modul { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Cust_Hodn", ReadOnly = true)]
        public bool Cust_Hodn { get; set; }

        public void AfterGetList<T>(IWebEasRepositoryBase repository, ref List<T> data, Filter filter)
        {
            var rows = data as List<NastavenieView>;
            var uctovneObdobieRow = rows.SingleOrDefault(x => x.Nazov == "UctovneObdobie");
            if (uctovneObdobieRow != null)
            {
                var datum = (uctovneObdobieRow.Hodn.IsNullOrEmpty() ? null : DateTime.Parse(uctovneObdobieRow.Hodn).ToString("MM\\/yyyy")) ?? repository.GetNastavenieD("reg", "eSAMStart")?.ToString("MM\\/yyyy") ?? new DateTime(DateTime.Today.Year, 1, 1).ToString("MM\\/yyyy");
                uctovneObdobieRow.Hodn = datum;
            }
            
        }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            if (node.KodRoot != "reg" && model?.Fields != null)
            {
                model.Fields.First(p => p.Name == nameof(Modul)).Text = "_Modul";
                //model.Fields.First(p => p.Name == nameof(Modul)).LoadWhenVisible = true;
            }
        }
    }
}
