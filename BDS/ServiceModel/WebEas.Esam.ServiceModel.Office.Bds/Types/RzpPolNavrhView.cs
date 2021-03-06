﻿using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_RzpPolNavrh")]
    [DataContract]
    public class RzpPolNavrhView : RzpPolNavrh, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_Typ", Description = "Návrh/Zmena(y)")]
        public bool Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov návrhu", ReadOnly = true)]
        public string RzpNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov rozpočtovej položky", ReadOnly = true)]
        public string RzpPolNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka", RequiredFields = new[] { "Rok", "D_Program_Id" })]
        [PfeCombo(typeof(RzpPolView), NameColumn = "C_RzpPol_Id", AdditionalWhereSql = "((@D_Program_Id IS NOT NULL AND PrijemVydaj = 2) OR (@D_Program_Id IS NULL)) AND (@Rok BETWEEN year(PlatnostOd) AND ISNULL(year(PlatnostDo), year(getDate())))")]
        public string RzpUcet { get; set; }

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
                //model.Fields.FirstOrDefault(p => p.Name == "NavrhRzp1").Text = string.Concat("Návrh rozpočtu ", DateTime.Now.Year + 1);
                //model.Fields.FirstOrDefault(p => p.Name == "NavrhRzp2").Text = string.Concat("Návrh rozpočtu ", DateTime.Now.Year + 2);
                if (!((RepositoryBase)repository).GetNastavenieB("rzp", "VydProgrRzp"))
                {
                    model.Fields.First(p => p.Name == "PRFull").Name = "_PRFull";
                }
            }
        }
    }
}
