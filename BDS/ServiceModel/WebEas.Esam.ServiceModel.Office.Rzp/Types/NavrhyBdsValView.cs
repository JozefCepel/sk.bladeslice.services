using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("V_NavrhyBdsVal")]
    [DataContract]
    public class NavrhyBdsValView : NavrhyBdsVal, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_Typ", Description = "Návrh/Zmena(y)")]
        public bool Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov návrhu", ReadOnly = true)]
        public string NavrhZmenyBdsNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov rozpočtovej položky", ReadOnly = true)]
        public string BdsPolozkyNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka", RequiredFields = new[] { "Rok" })]
        [PfeCombo(typeof(BdsPolozkyView), NameColumn = "C_BdsPolozky_Id", AdditionalWhereSql = "(@Rok BETWEEN year(PlatnostOd) AND ISNULL(year(PlatnostDo), year(getDate())))")]
        public string BdsUcet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public int Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program", RequiredFields = new[] { "Rok" })]
        [PfeCombo(typeof(ProgramView), NameColumn = "D_Program_Id", AdditionalWhereSql = "(@Rok BETWEEN year(PlatnostOd) AND ISNULL(year(PlatnostDo), year(getDate()))) AND Posledny = 1")]
        public string PRFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko", RequiredFields = new[] { "Rok" })]
        [PfeCombo(typeof(StrediskoView), NameColumn = "C_Stredisko_Id", AdditionalWhereSql = "(@Rok BETWEEN year(PlatnostOd) AND ISNULL(year(PlatnostDo), year(getDate())))")]
        public string StrediskoNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt", RequiredFields = new[] { "Rok" })]
        [PfeCombo(typeof(ProjektView), NameColumn = "C_Projekt_Id", AdditionalWhereSql = "(@Rok BETWEEN year(PlatnostOd) AND ISNULL(year(PlatnostDo), year(getDate())))")]
        public string ProjektKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Suma zmien", Editable = false)]
        public decimal SumaZmeny { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Suma čerpania", Editable = false)]
        public decimal SumaCerpanie { get; set; }

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
                // JP: Nevieme menit za jazdy nazvy stlpcov, ked stojis na zaznamoch s roznym rokom tak to neukazovalo spravne, ukazovalo vzdy YEAR(NOW) + 1
                // budeme teda zobrazovt Rok+1 a Rok+2
                //model.Fields.FirstOrDefault(p => p.Name == "NavrhBds1").Text = string.Concat("Návrh rozpočtu ", DateTime.Now.Year + 1);
                //model.Fields.FirstOrDefault(p => p.Name == "NavrhBds2").Text = string.Concat("Návrh rozpočtu ", DateTime.Now.Year + 2);
                if (!((RepositoryBase)repository).GetNastavenieB("rzp", "VydProgrBds"))
                {
                    model.Fields.First(p => p.Name == "PRFull").Name = "_PRFull";
                }
            }
        }
    }
}
