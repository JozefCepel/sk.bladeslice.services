﻿using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Reg;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [Schema("uct")]
    [Alias("V_DokladIND")]
    [DataContract]
    public class DokladINDView : BiznisEntitaDokladView, IPfeCustomize, IBaseView
    {
        //[DataMember]
        //[PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        //public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool DCOM { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        public new void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            base.CustomizeModel(model, repository, node, filter, masterNode);
            var eSAMRezim = repository.GetNastavenieI("reg", "eSAMRezim");
            var isoZdroj = repository.GetNastavenieI("reg", "ISOZdroj");
            var isoZdrojNazov = repository.GetNastavenieS("reg", "ISOZdrojNazov");

            if (model.Fields != null)
            {

                if (eSAMRezim != 1)
                {
                    var dcom = model.Fields.FirstOrDefault(p => p.Name == nameof(DCOM));
                    dcom.Text = "_DCOM";
                }
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

            if (isoZdroj > 0 && repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any() && model.Type != PfeModelType.Form)
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