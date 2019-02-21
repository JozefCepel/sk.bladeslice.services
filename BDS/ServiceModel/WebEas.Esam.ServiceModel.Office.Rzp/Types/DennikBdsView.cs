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
    [Alias("V_DennikBds")]
    [DataContract]
    public class DennikBdsView : DennikBds, IPfeCustomize
    {
        [DataMember]
        [PfeColumn(Text = "_UO")]
        [IgnoreInsertOrUpdate]
        public byte? UO { get; set; }


        [DataMember]
        [PfeColumn(Text = "_Rok", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public int Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovné obdobie", ReadOnly = true)]
        [Ignore]
        public string UORok
        {
            get
            {
                return string.Concat(UO, "/", Rok);
            }
        }

        [DataMember]
        [PfeColumn(Text = "Typ dokladu", ReadOnly = true)]
        public string TypDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo dokladu", ReadOnly = true)]
        public string CisloDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum", Type = PfeDataType.Date, ReadOnly = true)]
        public DateTime? DatumDokladu { get; set; }

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
        [PfeCombo(typeof(BdsPolozkyView), DisplayColumn = "BdsUcetNazov", NameColumn = "C_BdsPolozky_Id", AdditionalWhereSql = "((CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END) BETWEEN PlatnostOd AND ISNULL(PlatnostDo, CASE WHEN ISDATE (@DatumDokladu) = 1 THEN @DatumDokladu END))")]
        public string BdsUcetNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód účtu", ReadOnly = true)]
        public string BdsUcet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov účtu", ReadOnly = true)]
        public string BdsNazov { get; set; }

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
                if (!((RepositoryBase)repository).GetNastavenieB("rzp", "VydProgrBds"))
                {
                    model.Fields.First(p => p.Name == "PRFull").Name = "_PRFull";
                }
            }
        }
    }
}