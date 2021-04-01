using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_Pokladnica")]
    [DataContract]
    public class PokladnicaView : Pokladnica, IPfeCustomize, IOrsPravo, IPfeCustomizeCombo, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Mena", Mandatory = true)]
        [PfeCombo(typeof(MenaView), IdColumn = nameof(C_Mena_Id), ComboDisplayColumn = nameof(MenaView.Kod), AdditionalWhereSql = "Poradie > 0")]
        public string MenaKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schválil", Mandatory = true)]
        [PfeCombo(typeof(UserComboView), IdColumn = nameof(D_User_Id_Podpisal), ComboDisplayColumn = nameof(UserComboView.FullName), AdditionalWhereSql = "C_Modul_Id = 6")]
        public string PodpisalMeno { get; set; }

        [DataMember]
        public DateTime? DatumPlatnosti { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ")]
        [PfeCombo(typeof(TerminalovaTypCombo), IdColumn = nameof(Terminalova))]
        [Ignore]
        public string TerminalovaTyp => TerminalovaTypCombo.GetText(Terminalova);

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
            if (kodPolozky.StartsWith("fin-pok-pdk"))
                comboAttribute.FilterByOrsPravo = true;

        }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, object masterNodeParameter, string masterNodeKey)
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

                model.Fields
                    .First(p => p.Name == nameof(Kod))
                    .Validator = new PfeValidator
                    {
                        Rules = new List<PfeRule>
                        {
                            new PfeRule
                            {
                                ValidatorType = PfeValidatorType.SetMin,
                                Min = "2"
                            }
                        }
                    };

                if (((IRepositoryBase)repository).GetNastavenieI("reg", "eSAMRezim") != 1)
                {
                    model.Fields.FirstOrDefault(p => p.Name == nameof(DCOM)).Text = "_DCOM";
                }
            }

            int isoZdroj = (int)((IRepositoryBase)repository).GetNastavenieI("reg", "ISOZdroj");
            var isoZdrojNazov = ((IRepositoryBase)repository).GetNastavenieS("reg", "ISOZdrojNazov");
            if (isoZdroj > 0 && repository.Session.Roles.Where(w => w.Contains("REG_MIGRATOR")).Any())
            {
                //Akcie dostupné aj v tomto režime
                // node.Actions.RemoveAll(x => x.ActionType == NodeActionType.MenuButtonsAll && x.Caption == "Pridať");
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