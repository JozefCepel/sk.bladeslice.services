using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;


namespace WebEas.Esam.ServiceModel.Office
{
    [DataContract]
    public class PrijemVydajCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte? kod)
        {
            return kod switch
            {
                1 => "P",
                2 => "V",
                0 => "",
                _ => kod + "-",
            };
        }
    }

    [DataContract]
    public class PV3DCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case 1: return "+";
                case 2: return "-";
                default: return kod + " (?)";
            }
        }
    }

    [DataContract]
    public class SimulationType
    {
        [DataMember]
        public byte id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        /*
        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 0, 1, 2, 3 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
        }
        */

        public static string GetText(byte id)
        {
            switch (id)
            {
                case 0: return "-";
                case 1: return "Cube";
                case 2: return "Cylinder";
                case 3: return "Cube - oriented";
                default: return id + " (?)";
            }
        }
    }

    [DataContract]
    public class PredbeznyRzpCombo : IStaticCombo
    {
        [DataMember]
        public short Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            short[] list = new short[] { 0, 1, 2, 3, 4 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(short kod)
        {
            return kod switch
            {
                0 => "<žiadny>",           // none
                1 => "FA",                 // faktúry
                2 => "FA; ZF",             // faktúry, zálohové faktúry
                3 => "FA; ZF; OB, ZM",     // faktúry, zálohové faktúry, objednávky, zmluvy
                4 => "FA, ZF; OB, ZM; CP", // faktúry, zálohové faktúry, objednávky, zmluvy, cenové ponuky
                _ => kod + " (?)"
            };
        }
    }
    [DataContract]
    public class PravoCombo : IStaticCombo
    {
        [DataMember]
        public short Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case 1: return "Výstupovo orientovaný (operačný)";
                case 2: return "Výsledkovo orientovaný (špecifický)";
                default: return null;
            }
        }
    }

    [DataContract]
    public class CieleUkazTypCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2, 3, 4 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte? kod)
        {
            switch (kod)
            {
                case 1: return "Množstvo (kvantita)";
                case 2: return "Kvalita";
                case 3: return "Nákladovosť / efektívnosť";
                case 4: return "Výsledok (%)";
                default: return null;
            }
        }
    }

    [DataContract]
    public class ProgramTypCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2, 3 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte? kod)
        {
            switch (kod)
            {
                case 1: return "Program";
                case 2: return "Podprogram";
                case 3: return "Prvok";
                default: return null;
            }
        }
    }

    [DataContract]
    public class UctKlucCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            var ComboList = new List<IComboResult>();

            string kluc = ((IRepositoryBase)Repository).GetNastavenieS("uct", "UctKluc1Nazov");
            if (!string.IsNullOrEmpty(kluc))
            {
                ComboList.Add(new ComboResult() { Id = ((byte)1).ToString(), Value = kluc });
            }

            kluc = ((IRepositoryBase)Repository).GetNastavenieS("uct", "UctKluc2Nazov");
            if (!string.IsNullOrEmpty(kluc))
            {
                ComboList.Add(new ComboResult() { Id = ((byte)2).ToString(), Value = kluc });
            }
            kluc = ((IRepositoryBase)Repository).GetNastavenieS("uct", "UctKluc3Nazov");
            if (!string.IsNullOrEmpty(kluc))
            {
                ComboList.Add(new ComboResult() { Id = ((byte)3).ToString(), Value = kluc });
            }

            return ComboList;
        }

        public static string GetText(byte? kod, IWebEasRepositoryBase Repository)
        {
            switch (kod)
            {
                case 1: return ((IRepositoryBase)Repository).GetNastavenieS("uct", "UctKluc1Nazov");
                case 2: return ((IRepositoryBase)Repository).GetNastavenieS("uct", "UctKluc2Nazov");
                case 3: return ((IRepositoryBase)Repository).GetNastavenieS("uct", "UctKluc3Nazov");
                default: return null;
            }
        }
    }

    [DataContract]
    public class UctRozvrhTypCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            char[] list = new char[] { 'A', 'P', 'X', 'N', 'V' };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(char kod)
        {
            switch (kod)
            {
                case 'A': return "Aktívny";
                case 'P': return "Pasívny";
                case 'X': return "S premenlivým zost.";
                case 'N': return "Nákladový";
                case 'V': return "Výnosový";
                default: return null;
            }
        }
    }

    [DataContract]
    public class UctRozvrhDruhCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            char[] list = new char[] { 'S', 'V', 'U', 'P' };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(char kod)
        {
            switch (kod)
            {
                case 'S': return "Súvahový";
                case 'V': return "Výsledovkový";
                case 'U': return "Uzávierkový";
                case 'P': return "Podsúvahový";
                default: return null;
            }
        }
    }

    [DataContract]
    public class UctRozvrhSDKCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            char[] list = new char[] { 'P', 'Z' };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(char kod)
        {
            switch (kod)
            {
                case 'P': return "Pohľadávka";
                case 'Z': return "Záväzok";
                default: return "";
            }
        }
    }

    [DataContract]
    public class UctRozvrhCasoveRozlisenieCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            char[] list = new char[] { 'K', 'D' };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(char kod)
        {
            switch (kod)
            {
                case 'K': return "Krátkodobý";
                case 'D': return "Dlhodobý";
                default: return null;
            }
        }
    }

    [DataContract]
    public class ObdobieCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            int[] list = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(int? kod)
        {
            return kod switch
            {
                1 => "Január",
                2 => "Február",
                3 => "Marec",
                4 => "Apríl",
                5 => "Máj",
                6 => "Jún",
                7 => "Júl",
                8 => "August",
                9 => "September",
                10 => "Október",
                11 => "November",
                12 => "December",
                _ => GetText(DateTime.Now.Month),
            };
        }
    }

    [DataContract]
    public class OsobaTypCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            char[] list = new char[] { 'F', 'P' };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(char kod)
        {
            switch (kod)
            {
                case 'F': return "Fyzická osoba";
                case 'P': return "Podnikateľ alebo právnická osoba";
                default: return null;
            }
        }
    }

    [DataContract]
    public class FakturaciaVztahCombo : IStaticCombo
    {
        [DataMember]
        public short Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            short[] list = new short[] { (int)FakturaciaVztahEnum.NEURCENE, (int)FakturaciaVztahEnum.DOD, (int)FakturaciaVztahEnum.ODB, (int)FakturaciaVztahEnum.DOD_ODB };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case (int)FakturaciaVztahEnum.NEURCENE: return "neurčené";
                case (int)FakturaciaVztahEnum.DOD: return "dodávateľ";
                case (int)FakturaciaVztahEnum.ODB: return "odberateľ";
                case (int)FakturaciaVztahEnum.DOD_ODB: return "dodávateľ aj odberateľ";
                default: return null;
            }
        }
    }

    [DataContract]
    public class TypPOCombo : IStaticCombo
    {
        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2, 3 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte? kod)
        {
            switch (kod)
            {
                case 1: return "Poisťovňa";
                case 2: return "Poskytovateľ ZS";
                case 3: return "Zdrav. zariadenie";
                default: return null;
            }
        }
    }

    [DataContract]
    public class VzorAdresyCombo : IStaticCombo
    {
        public enum VzorAdresyEnum
        {
            [Description("štandard")]
            standard = 1,
            [Description("P.O.Box")]
            POBox = 2,
            [Description("neštruktúrované údaje")]
            nestrukturovaneUdaje = 3,
            [Description("pobyt na obci")]
            pobytNaObci = 4,
        }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            int[] list = null;
            string typOsoby = "";
            RequiredFields.TryGetValue("C_OsobaTyp_Id", out typOsoby);

            if(typOsoby == "2")
                list = new int[] { 1, 2, 3};
            else
                list = new int[] { 1, 2, 3, 4 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(int? kod)
        {
            return kod switch
            {
                1 => VzorAdresyEnum.standard.ToDescription(),
                2 => VzorAdresyEnum.POBox.ToDescription(),
                3 => VzorAdresyEnum.nestrukturovaneUdaje.ToDescription(),
                4 => VzorAdresyEnum.pobytNaObci.ToDescription(),
                _ => "",
            };
        }
    }

    [DataContract]
    public class DatumTUEUDVCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            int id = 0;
            int.TryParse(RequiredFields["C_TypBiznisEntity_Id"], out id);

            string[] list = GetDokladList(id);
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(string kod)
        {
            return kod switch
            {
                "DatumDokladu" => "DatumDokladu",
                "DatumPrijatia" => "DatumPrijatia",
                "DatumVystavenia" => "DatumVystavenia",
                "DatumDodania" => "DatumDodania",
                "DatumVyrubenia" => "DatumVyrubenia",
                "DatumPravoplatnosti" => "DatumPravoplatnosti",
                "DatumVykonatelnosti" => "DatumVykonatelnosti",
                _ => "",
            };
        }

        private string[] GetDokladList(int cTypBiznisEntityId)
        {
            return cTypBiznisEntityId switch
            {
                1 => new string[] { "DatumDokladu" },
                2 => new string[] { "DatumPrijatia", "DatumVystavenia", "DatumDodania" },
                3 => new string[] { "DatumVystavenia", "DatumDodania" },
                4 => new string[] { "DatumDokladu" },
                5 => new string[] { "DatumDokladu" },
                6 => new string[] { "DatumDokladu" },
                7 => new string[] { "DatumDokladu" },
                8 => new string[] { "DatumDokladu" },
                9 => new string[] { "DatumPrijatia", "DatumVystavenia", "DatumDodania" },
                10 => new string[] { "DatumVystavenia", "DatumDodania" },
                11 => new string[] { "DatumVystavenia", "DatumDodania" },
                12 => new string[] { "DatumPrijatia", "DatumVystavenia", "DatumDodania" },
                13 => new string[] { "DatumPrijatia", "DatumVystavenia", "DatumDodania" },
                14 => new string[] { "DatumVystavenia", "DatumDodania" },
                15 => new string[] { "DatumPrijatia", "DatumVystavenia", "DatumDodania" },
                16 => new string[] { "DatumVystavenia", "DatumDodania" },
                17 => new string[] { "DatumVystavenia" },
                18 => new string[] { "DatumPrijatia", "DatumVystavenia" },
                19 => new string[] { "DatumDokladu" },
                20 => new string[] { "DatumVystavenia", "DatumDodania" },
                50 => new string[] { "DatumVyrubenia", "DatumPravoplatnosti", "DatumVykonatelnosti" },
                _ => null,
            };
        }
    }

    [DataContract]
    public class VSCombo : IStaticCombo, IComboResult, IPfeCustomizeCombo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember]
        public decimal? DM_Neuhradene { get; set; }

        [DataMember]
        public decimal? DM_Nevyfakturovane { get; set; }

        [DataMember]
        public DateTime? DatumUhrady { get; set; }

        [DataMember]
        public long? D_BiznisEntita_Id_ZF { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public string IdFormatMeno { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        // dvojicka UhradaParovanieView, ak sa nieco meni kukni aj tam
        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            string additionalOsobaFilter = "";
            int typ = 0;
            long osobaId = 0;
            int rok = 0;
            if (RequiredFields != null)
            {
                if (RequiredFields.Any())
                {
                    if (RequiredFields.ContainsKey("D_Osoba_Id"))
                    {
                        long.TryParse(RequiredFields["D_Osoba_Id"], out osobaId);
                        if (osobaId != 0)
                        {
                            additionalOsobaFilter = $"D_Osoba_Id = {osobaId} AND ";
                        }
                    }
                    if (RequiredFields.ContainsKey("C_Typ_Id"))
                    {
                        int.TryParse(RequiredFields["C_Typ_Id"], out typ);
                    }
                    if (RequiredFields.ContainsKey("Rok"))
                    {
                        int.TryParse(RequiredFields["Rok"], out rok);
                    }
                }
            }

            string comboValueWhere = string.Empty;
            if (!value.IsNullOrEmpty())
            {
                comboValueWhere = $" AND VS COLLATE Latin1_general_CI_AI LIKE '%{value}%'";
            }

            //Filter musí byť identický ako v "ComboCustomize" - nižšie v kóde
            if (KodPolozky == "crm-vaz-zal")
            {
                List<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, long? D_BiznisEntita_Id_ZF, decimal Nevyfakturovane, DateTime? DatumUhrady, string Popis)> list = null;
                switch (typ)
                {
                    case (int)TypEnum.UhradaDZF:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, long? D_BiznisEntita_Id_ZF, decimal Nevyfakturovane, DateTime? DatumUhrady, string Popis)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_UhradaParovanie_Id, D_BiznisEntita_Id_Predpis, DM_Nevyfakturovane, DatumPohybu, ISNULL(Popis, UH_Popis) FROM fin.V_UhradaParovanie WHERE {additionalOsobaFilter} C_Typ_Id = {typ} AND C_StavEntity_Id > 1 AND DM_Nevyfakturovane <> 0 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.UhradaOZF:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, long? D_BiznisEntita_Id_ZF, decimal Nevyfakturovane, DateTime? DatumUhrady, string Popis)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_UhradaParovanie_Id, D_BiznisEntita_Id_Predpis, DM_Nevyfakturovane, DatumPohybu, ISNULL(Popis, UH_Popis) FROM fin.V_UhradaParovanie WHERE {additionalOsobaFilter} C_Typ_Id = {typ} AND C_StavEntity_Id > 1 AND DM_Nevyfakturovane <> 0 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.ZalohyPoskytnute:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, long? D_BiznisEntita_Id_ZF, decimal Nevyfakturovane, DateTime? DatumUhrady, string Popis)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, Zaloha_Id, NULL, DM_Nevyfakturovane, DatumPohybu, Popis FROM fin.V_ZalohyPoskytnutePrijate WHERE (D_Osoba_Id = {osobaId} OR D_Osoba_Id IS NULL AND YEAR(DatumPohybu) = {rok}) AND C_Typ_Id = {typ} AND DM_Nevyfakturovane <> 0 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.ZalohyPrijate:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, long? D_BiznisEntita_Id_ZF, decimal Nevyfakturovane, DateTime? DatumUhrady, string Popis)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, Zaloha_Id, NULL, DM_Nevyfakturovane, DatumPohybu, Popis FROM fin.V_ZalohyPoskytnutePrijate WHERE (D_Osoba_Id = {osobaId} OR D_Osoba_Id IS NULL AND YEAR(DatumPohybu) = {rok}) AND C_Typ_Id = {typ} AND DM_Nevyfakturovane <> 0 {comboValueWhere} ORDER BY VS");
                        break;
                    default:
                        // vratime prazdne combo
                        return new List<IComboResult>();
                }
                return list.Select(a => new VSCombo()
                {
                    Id = a.Predpis_Id.ToString(),
                    Value = a.VS,
                    D_BiznisEntita_Id_ZF = a.D_BiznisEntita_Id_ZF,
                    DM_Nevyfakturovane = a.Nevyfakturovane,
                    DatumUhrady = a.DatumUhrady,
                    Popis = a.Popis,
                    D_Osoba_Id = a.OsobaId,
                    IdFormatMeno = a.IdFormatMeno
                }).ToList<IComboResult>();
            }
            else
            {
                List<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)> list = null;
                switch (typ)
                {
                    case (int)TypEnum.UhradaDFA:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_BiznisEntita_Id, DM_Neuhradene, DatumUhrady FROM crm.V_DokladDFA WHERE {additionalOsobaFilter} R = 1 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.UhradaOFA:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_BiznisEntita_Id, DM_Neuhradene, DatumUhrady FROM crm.V_DokladOFA WHERE {additionalOsobaFilter} S = 1 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.UhradaDZF:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_BiznisEntita_Id, DM_Neuhradene, DatumUhrady FROM crm.V_DokladDZF WHERE {additionalOsobaFilter} R = 1 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.UhradaOZF:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_BiznisEntita_Id, DM_Neuhradene, DatumUhrady FROM crm.V_DokladOZF WHERE {additionalOsobaFilter} S = 1 {comboValueWhere} ORDER BY VS");
                        break;

                    case (int)TypEnum.DobropisDFA:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_BiznisEntita_Id, DM_Neuhradene, DatumUhrady FROM crm.V_DokladDFA WHERE {additionalOsobaFilter} DM_Suma < 0 AND R = 1 {comboValueWhere} ORDER BY VS");
                        break;
                    case (int)TypEnum.DobropisOFA:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_BiznisEntita_Id, DM_Neuhradene, DatumUhrady FROM crm.V_DokladOFA WHERE {additionalOsobaFilter} DM_Suma < 0 AND S = 1 {comboValueWhere} ORDER BY VS");
                        break;

                    //Nastavenie "0 AS DM_Neuhradene, NULL AS DatumUhrady" je preto, lebo sa musí neskôr vybrať položka rozhodnutia
                    case (int)TypEnum.DaPUhradaVsetky:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id IS NOT NULL {comboValueWhere} ORDER BY VS"); // VymerTypEnum.DaPUhradaVsetky
                        break;
                    case (int)TypEnum.DaPUhradaDane:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id = 1 {comboValueWhere} ORDER BY VS"); // VymerTypEnum.DAN
                        break;
                    case (int)TypEnum.DaPUhradaPokutyZaOneskorenie:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id = 2 {comboValueWhere} ORDER BY VS"); // VymerTypEnum.ONE
                        break;
                    case (int)TypEnum.DaPUhradaUrokuZOmeskania:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id = 4 {comboValueWhere} ORDER BY VS"); // VymerTypEnum.PEN
                        break;
                    case (int)TypEnum.DaPUhradaPokuty:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id = 5 {comboValueWhere} ORDER BY VS"); // VymerTypEnum.POK
                        break;
                    case (int)TypEnum.DaPUhradaPokutyZaDodatocnePodanie:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id = 6 {comboValueWhere} ORDER BY VS"); // VymerTypEnum.DOD
                        break;
                    case (int)TypEnum.DaPUhradaUrokuZOdlozeniaSplatok:
                        list = Repository.Db.Select<(string IdFormatMeno, string VS, long? OsobaId, long Predpis_Id, decimal Neuhradene, DateTime? DatumUhrady)>($"SELECT IdFormatMeno, VS, D_Osoba_Id, D_Vymer_Id, 0 AS DM_Neuhradene, NULL AS DatumUhrady FROM dap.V_Vymer WHERE {additionalOsobaFilter} C_VymerTyp_Id = 7 {comboValueWhere} ORDER BY VS"); // VymerTypEnum.URO
                        break;
                    default:
                        // vratime prazdne combo
                        return new List<IComboResult>();
                }
                return list.Select(a => new VSCombo()
                {
                    Id = a.Predpis_Id.ToString(),
                    Value = a.VS,
                    DM_Neuhradene = a.Neuhradene,
                    DatumUhrady = a.DatumUhrady,
                    D_Osoba_Id = a.OsobaId,
                    IdFormatMeno = a.IdFormatMeno
                }).ToList<IComboResult>();
            }
        }

        public static string GetText(string kod)
        {
            return null;
        }

        public void ComboCustomize(IWebEasRepositoryBase repository, string column, string kodPolozky, ref PfeComboAttribute comboAttribute)
        {
            if (column.ToLower() == "vs" && kodPolozky == "fin-pol-par")
            {
                //Filter musí byť identický ako v "switch (typ)" - vyššie v kóde
                comboAttribute.AdditionalWhereSql = Environment.NewLine +
                                                    $"CASE WHEN @C_Typ_Id IN ({(int)TypEnum.UhradaDFA  },{(int)TypEnum.UhradaDZF  }) AND R = 1 THEN 1 " +
                                                    $"     WHEN @C_Typ_Id IN ({(int)TypEnum.UhradaOFA  },{(int)TypEnum.UhradaOZF  }) AND S = 1 THEN 1 " +
                                                    $"     WHEN @C_Typ_Id IN ({(int)TypEnum.DobropisDFA}                           ) AND R = 1 AND DM_Suma < 0 THEN 1 " +
                                                    $"     WHEN @C_Typ_Id IN ({(int)TypEnum.DobropisOFA}                           ) AND S = 1 AND DM_Suma < 0 THEN 1 " +
                                                    $"     WHEN @C_Typ_Id = {(int)TypEnum.DaPUhradaDane                    } AND C_VymerTyp_Id = 1 THEN 1 " +  // VymerTypEnum.DAN
                                                    $"     WHEN @C_Typ_Id = {(int)TypEnum.DaPUhradaPokutyZaOneskorenie     } AND C_VymerTyp_Id = 2 THEN 1 " +  // VymerTypEnum.ONE
                                                    $"     WHEN @C_Typ_Id = {(int)TypEnum.DaPUhradaUrokuZOmeskania         } AND C_VymerTyp_Id = 4 THEN 1 " +  // VymerTypEnum.PEN
                                                    $"     WHEN @C_Typ_Id = {(int)TypEnum.DaPUhradaPokuty                  } AND C_VymerTyp_Id = 5 THEN 1 " +  // VymerTypEnum.POK
                                                    $"     WHEN @C_Typ_Id = {(int)TypEnum.DaPUhradaPokutyZaDodatocnePodanie} AND C_VymerTyp_Id = 6 THEN 1 " +  // VymerTypEnum.DOD
                                                    $"     WHEN @C_Typ_Id = {(int)TypEnum.DaPUhradaUrokuZOdlozeniaSplatok  } AND C_VymerTyp_Id = 7 THEN 1 " +  // VymerTypEnum.URO
                                                    $"     END = 1 AND (D_Osoba_Id = @D_Osoba_Id OR @D_Osoba_Id IS NULL)";
            }
            else if (column.ToLower() == "vs" && kodPolozky == "crm-vaz-zal")
            {
                //Filter musí byť identický ako v "switch (typ)" - vyššie v kóde
                comboAttribute.AdditionalWhereSql = Environment.NewLine +
                                                    $"CASE WHEN @C_Typ_Id IN ({(int)TypEnum.UhradaDZF},{(int)TypEnum.UhradaOZF}) AND C_StavEntity_Id > 1 AND DM_Nevyfakturovane <> 0 THEN 1 " +
                                                    $"     END = 1 AND (D_Osoba_Id = @D_Osoba_Id OR @D_Osoba_Id IS NULL)";
            }
        }
    }

    [DataContract]
    public class RegRzpDefiniciaCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            int[] list = new int[] { -1, 0, 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(int kod)
        {
            return kod switch
            {
                -1 => "Nerozpočtovať",
                0 => "Príjem aj výdaj",
                1 => "Iba príjem",
                2 => "Iba výdaj",
                _ => null,
            };
        }
    }

    [DataContract]
    public class SadzbaDPHCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            short[] list = new short[] { -1, 2, 1, 0 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(short? kod)
        {
            return kod switch
            {
                -1 => "(nerozhoduje)",
                0 => "bez DPH",
                1 => "znížená",
                2 => "základná",
                _ => null,
            };
        }
    }

    [DataContract]
    public class DAPRokCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            int rok = DateTime.Today.Year;
            string[] list = new string[] { "N", "A", "M", (rok - 1).ToString(), (rok - 2).ToString(), (rok - 3).ToString(), (rok - 4).ToString(), (rok - 5).ToString() };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(string kod)
        {
            return kod switch
            {
                "N" => "(nerozhoduje)",
                "A" => $"Aktuálny rok ({DateTime.Today.Year})",
                "M" => $"Minulé roky (<{DateTime.Today.Year})",
                _ => kod,
            };
        }
    }

    [DataContract]
    public class FormaUhradyCombo : IStaticCombo
    {
        [DataMember]
        public short Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2, 3, 9 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte kod)
        {
            return kod switch
            {
                1 => "Bankový prevod",
                2 => "Hotovosť",
                3 => "Dobierka",
                9 => "Iná",
                _ => null,
            };
        }
    }

    [DataContract]
    public class ParovanieNazovCombo : IStaticCombo
    {
        [DataMember]
        public short Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            var list = (StavEntityEnum[])Enum.GetValues(typeof(StavEntityEnum));
            return list.Where(x => (int)x >= 18 && (int)x <= 21).Select(a => new ComboResult() { Id = ((int)a).ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(StavEntityEnum stavEntityId)
        {
            return stavEntityId switch
            {
                StavEntityEnum.SILNAZHODA => "Silná zhoda",
                StavEntityEnum.SLABAZHODA => "Slabá zhoda",
                StavEntityEnum.PREPLATOK => "Preplatok",
                StavEntityEnum.RUCNE => "Ručne",
                _ => null,
            };
        }
    }

    [DataContract]
    public class ParovanieTypCombo : IStaticCombo
    {
        [DataMember]
        public byte Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2, 3, 4 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte kod)
        {
            return kod switch
            {
                1 => "1:1",
                2 => "1:N",
                3 => "N:1",
                4 => "N:M",
                _ => null,
            };
        }
    }

    [DataContract]
    public class DatumTypCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte? kod)
        {
            switch (kod)
            {
                case 1: return "aktuálny dátum";
                case 2: return "dátum dokladu";
                default: return null;
            }
        }
    }

    [DataContract]
    public class TypSazbyCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields, string value)
        {
            byte[] list = new byte[] { 0, 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(byte? kod)
        {
            switch (kod)
            {
                case 0: return "Bez DPH";
                case 1: return "Znížená sadzba";
                case 2: return "Základná sadzba";
                default: return null;
            }
        }
    }
}
