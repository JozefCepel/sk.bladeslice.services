using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.Esam.ServiceModel.Office.Types.Uct;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [Schema("fin")]
    [Alias("V_DokladBAN")]
    [DataContract]
    public class DokladBANView : BiznisEntitaDokladView, IPfeCustomize, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Kredit", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Kredit { get; set; }

        [DataMember]
        [PfeColumn(Text = "Debet", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Debet { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Kredit CM", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Kredit { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Debet CM", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Debet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Starý zostatok", ReadOnly = true)]
        public decimal DM_ZostatokStary { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nový zostatok", ReadOnly = true)]
        public decimal DM_ZostatokNovy { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma položiek", ReadOnly = true)]
        public new decimal DM_Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Počet položiek", DefaultValue = 0)]
        public short PocetPol { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        [DataMember]
        [HierarchyNodeParameter]
        public new int? C_BankaUcet_Id { get; set; }

        public new void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
        {
            base.CustomizeModel(model, repository, node, filter, masterNodeParameter, masterNodeKey);
            if (model.Fields != null)
            {
                var eSAMRezim = ((IRepositoryBase)repository).GetNastavenieI("reg", "eSAMRezim");
                var isoZdroj = ((IRepositoryBase)repository).GetNastavenieI("reg", "ISOZdroj");
                var isoZdrojNazov = ((IRepositoryBase)repository).GetNastavenieS("reg", "ISOZdrojNazov");

                if (eSAMRezim != 1)
                {
                    var dcom = model.Fields.FirstOrDefault(p => p.Name == nameof(DCOM));
                    dcom.Text = "_DCOM";
                }

                if (eSAMRezim == 1)
                {
                    var na = new NodeAction(NodeActionType.MenuButtonsAll)
                    {
                        ActionIcon = NodeActionIcons.Refresh,
                        Caption = "DCOM",
                        MenuButtons = new List<NodeAction>()
                        {
                            new NodeAction(NodeActionType.DoposlanieUhradDoDcomu) 
                            {
                                SelectionMode = PfeSelection.Multi,
                                IdField = "D_BiznisEntita_Id",
                                Url = "/office/reg/long/DoposlanieUhradDoDcomu"
                            }
                        }
                    };
                    node.Actions.Add(na);
                }

                if (isoZdroj > 0 && repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any())
                {
                    if (node.Actions.Any(x => x.ActionType == NodeActionType.MenuButtonsAll))
                    {
                        var polozkaMenuAll = node.Actions.Where(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO").FirstOrDefault();
                        if (polozkaMenuAll != null)
                        {
                            polozkaMenuAll.Caption = isoZdrojNazov;
                        }
                    }
                }
                else
                {
                    node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO");
                }
            }
        }
    }
}
