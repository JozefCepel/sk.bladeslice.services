using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [Schema("reg")]
    [Alias("V_TypBiznisEntity_Kniha")]
    [DataContract]
    public class TypBiznisEntity_KnihaView : TypBiznisEntity_Kniha, IOrsPravo, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ dokladu")]
        [PfeCombo(typeof(TypBiznisEntityView), ComboDisplayColumn = nameof(TypBiznisEntityView.KodNazov), IdColumn = nameof(C_TypBiznisEntity_Id), AdditionalWhereSql = "Kod NOT IN ('PDK', 'BAN', 'PPP', 'DAP')")]
        public string TypBeKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TypeElement_Id")]
        [Ignore]
        public string TypeElement_Id { get { return "5_" + C_TypBiznisEntity_Kniha_Id; } }

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
    }

}
