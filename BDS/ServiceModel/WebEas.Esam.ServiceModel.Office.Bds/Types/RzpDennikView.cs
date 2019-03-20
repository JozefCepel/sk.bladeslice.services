using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceInterface.Office;
using WebEas.Esam.ServiceModel.Office.Reg.Types;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_RzpDennik")]
    [DataContract]
    public class RzpDennikView : RzpDennik, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_UOMesiac")]
        [IgnoreInsertOrUpdate]
        public short? UOMesiac { get; set; }


        [DataMember]
        [PfeColumn(Text = "_Rok", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public short? Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovné obdobie", ReadOnly = true)]
        [Ignore]
        public string UO
        {
            get
            {
                return string.Concat(UOMesiac, "/", Rok);
            }
        }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu", ReadOnly = true)]
        public string TypDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo dokladu", ReadOnly = true)]
        public string CisloInterne { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum", Type = PfeDataType.Date, ReadOnly = true)]
        public DateTime? DatumPrijatia { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PrijemVydaj")]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        [PfeCombo(typeof(PrijemVydajCombo), NameColumn = "PrijemVydaj")]
        [PfeColumn(Text = "P/V", Tooltip = "Či ide o príjmovú alebo výdavkovú položku")]
        [Ignore]
        public string PrijemVydajText
        {
            get
            {
                return PrijemVydajCombo.GetText(PrijemVydaj);
            }
        }

        [DataMember]
        [PfeColumn(Text = "Rozpočtová položka", RequiredFields = new[] { "PrijemVydaj", "DatumDokladu" })]
        [PfeCombo(typeof(RzpPolView), DisplayColumn = "RzpUcetNazov", NameColumn = "C_RzpPol_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END))")]
        public string RzpUcetNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód účtu", ReadOnly = true)]
        public string RzpUcet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov účtu", ReadOnly = true)]
        public string RzpNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program", RequiredFields = new[] { "PrijemVydaj", "DatumDokladu" })]
        [PfeCombo(typeof(ProgramView), NameColumn = "D_Program_Id", AdditionalWhereSql = "@PrijemVydaj = 2 AND ((CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END)) AND Posledny = 1")]
        public string PRFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód strediska", ReadOnly = true)]
        public string StrediskoKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov strediska", ReadOnly = true)]
        public string StrediskoNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko", RequiredFields = new[] { "DatumDokladu" })]
        [PfeCombo(typeof(StrediskoView), NameColumn = "C_Stredisko_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END))")]
        public string StrediskoKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód projektu", ReadOnly = true)]
        public string ProjektKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov projektu", ReadOnly = true)]
        public string ProjektNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt", RequiredFields = new[] { "DatumDokladu" })]
        [PfeCombo(typeof(ProjektView), NameColumn = "C_Projekt_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END))")]
        public string ProjektKodNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_FinKey", ReadOnly = true)]
        public string FinKey { get; set; }

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