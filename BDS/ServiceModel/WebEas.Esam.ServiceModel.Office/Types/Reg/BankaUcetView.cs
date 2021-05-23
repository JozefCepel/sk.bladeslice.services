using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_BankaUcet")]
    [DataContract]
    public class BankaUcetView : BankaUcetCis, IPfeCustomize, IOrsPravo, IPfeCustomizeCombo, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Mena", Mandatory = true)]
        [PfeCombo(typeof(MenaView), IdColumn = nameof(C_Mena_Id), ComboDisplayColumn = nameof(MenaView.Kod), AdditionalWhereSql = "Poradie > 0")]
        public string MenaKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TypeElement_Id")]
        [Ignore]
        public string TypeElement_Id => "3_" + C_BankaUcet_Id;

        [DataMember]
        public DateTime? DatumPlatnosti { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zostatok", ReadOnly = true)]
        public decimal? DM_Zostatok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zostatok k", ReadOnly = true, Type = PfeDataType.Date)]
        public DateTime? DatumZostatok { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "_OrsPravo", Hidden = true, Editable = false, ReadOnly = true)]
        public byte OrsPravo { get; set; }

        public void ApplyOrsPravoToAccesFlags()
        {
            if (OrsPravo < (int)Pravo.Upravovat) AccessFlag &= (long)(~(NodeActionFlag.Update | NodeActionFlag.Change));
            if (OrsPravo < (int)Pravo.Full) AccessFlag &= (long)(~NodeActionFlag.Delete);
        }

        public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, ref PfeComboAttribute comboAttribute)
        {
            if (kodPolozky.Equals("fin-bnk-ban") || kodPolozky.Equals("fin-bnk-ppp"))
                comboAttribute.FilterByOrsPravo = true;
            if (kodPolozky.Equals("fin-bnk-ppp"))
            {
                comboAttribute.AdditionalWhereSql += $" AND C_Mena_Id = {(int)MenaEnum.EUR}";
            }
        }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            if (model.Fields != null)
            {
                var poradieField = model.Fields.FirstOrDefault(p => p.Name == nameof(Poradie));

                if (poradieField != null)
                {
                    poradieField.Validator = new PfeValidator
                    {
                        Rules = new List<PfeRule>
                        {
                            new PfeRule
                            {
                                ValidatorType = PfeValidatorType.SetMin,
                                Min = "1"
                            }
                        }
                    };
                }

                if (repository.GetNastavenieI("reg", "eSAMRezim") != 1)
                {
                    model.Fields.FirstOrDefault(p => p.Name == nameof(DCOM)).Text = "_DCOM";
                }
            }

            int isoZdroj = (int)repository.GetNastavenieI("reg", "ISOZdroj");
            var isoZdrojNazov = repository.GetNastavenieS("reg", "ISOZdrojNazov");
            if (isoZdroj > 0 && repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any() && model.Type != PfeModelType.Form) 
            {
                //Akcie dostupné aj v tomto režime
                //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "Pridať");
                
                if (node.Actions.Any(x => x.ActionType == NodeActionType.MenuButtonsAll))
                {
                    var polozkaMenuAll = node.Actions.Where(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO").FirstOrDefault();
                    if (polozkaMenuAll != null)
                    {
                        polozkaMenuAll.Caption = isoZdrojNazov;
                    }
                    //Akcie dostupné aj v tomto režime
                    //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Change);
                    //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Update);
                    //node.Actions.RemoveAll(x => x.ActionType == NodeActionType.Delete);
                }
            }
            else
            {
                node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "ISO");
            }
        }
    }
}