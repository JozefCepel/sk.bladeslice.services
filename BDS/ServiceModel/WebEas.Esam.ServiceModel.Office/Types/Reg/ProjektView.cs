using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Rzp;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_Projekt")]
    [DataContract]
    public class ProjektView : Projekt, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ rozpočtu")]
        [PfeCombo(typeof(RzpTyp), ComboDisplayColumn = nameof(RzpTyp.Nazov), IdColumn = nameof(C_RzpTyp_Id))]
        public string RzpTypName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zdroj", Xtype = PfeXType.SearchFieldSS)]
        [PfeCombo(typeof(FRZdrojView), ComboDisplayColumn = nameof(FRZdrojView.ZdrojFull), ComboIdColumn = nameof(FRZdrojView.C_FRZdroj_Id), 
            AdditionalWhereSql = "GetDate() BETWEEN PlatnostOd AND ISNULL(PlatnostDo, GetDate() + 1) AND Platny = 1")]
        public string ZdrojFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt", Hidden = true, Hideable = false)]
        [PfeValueColumn]
        public string KodNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            if (model.Fields != null)
            {
                var zdrojField = model.Fields.FirstOrDefault(p => p.Name == nameof(ZdrojFull));
                if (zdrojField != null)
                {
                    zdrojField.SearchFieldDefinition = new List<PfeSearchFieldDefinition>
                    {
                        new PfeSearchFieldDefinition
                        {
                            Code = "rzp-cis-zdr",
                            NameField = nameof(FRZdrojView.C_FRZdroj_Id),
                            DisplayField = nameof(FRZdrojView.ZdrojFull),
                            AdditionalFilterDesc = "(Platné zdroje)"
                        }
                    };
                }
            }

            int isoZdroj = (int)((IRepositoryBase)repository).GetNastavenieI("reg", "ISOZdroj");
            var isoZdrojNazov = ((IRepositoryBase)repository).GetNastavenieS("reg", "ISOZdrojNazov");
            if (isoZdroj != 1 || !repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any())
            {
                node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO");
            }
            else
            {
                var polozkaMenuAll = node.Actions.Where(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO").FirstOrDefault();
                if (polozkaMenuAll != null)
                {
                    polozkaMenuAll.Caption = isoZdrojNazov;
                }
            }
        }
    }
}
