using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_ZmenyRzpVal")]
    [DataContract]
    public class ZmenyRzpValView : ZmenyRzpVal, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_Typ", Description = "Návrh/Zmena(y)")]
        public bool Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov návrhu", ReadOnly = true)]
        public string NavrhZmenyRzpNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov rozpočtovej položky", ReadOnly = true)]
        public string RzpPolozkyNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka", RequiredFields = new[] { "Datum" })]
        [PfeCombo(typeof(RzpPolozkyView), NameColumn = "C_RzpPolozky_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END))")]
        public string RzpUcet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum", Type = PfeDataType.Date, ReadOnly = true)]
        public DateTime Datum { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program", RequiredFields = new[] { "Datum" })]
        [PfeCombo(typeof(ProgramView), NameColumn = "D_Program_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END)) AND Posledny = 1")]
        public string PRFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko", RequiredFields = new[] { "Datum" })]
        [PfeCombo(typeof(StrediskoView), NameColumn = "C_Stredisko_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END))")]
        public string StrediskoNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt", RequiredFields = new[] { "Datum" })]
        [PfeCombo(typeof(ProjektView), NameColumn = "C_Projekt_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@Datum) = 1 THEN @Datum END))")]
        public string ProjektKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id")]
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeCombo(typeof(StavEntityView), NameColumn = "C_StavEntity_Id")]
        [PfeColumn(Text = "Stav", Editable = false, ReadOnly = true)]
        public string StavNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter)
        {
            if (model.Fields != null)
            {
                if (!((RepositoryBase)repository).GetNastavenieB("rzp", "VydProgrRzp"))
                {
                    model.Fields.First(p => p.Name == "PRFull").Name = "_PRFull";
                }
            }
        }
    }
}
