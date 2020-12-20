using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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
        public string Popis { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 0, 1, 2, 3 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
        }

        public static string GetText(byte id)
        {
            switch (id)
            {
                case 0: return "-";
                case 1: return "Cube";
                case 2: return "Cube - oriented";
                case 3: return "Cylinder";
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
        {
            short[] list = new short[] { 0, 1, 2, 3};
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList<IComboResult>();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case 0: return "neurčené";
                case 1: return "dodávateľ";
                case 2: return "odberateľ";
                case 3: return "dodávateľ aj odberateľ";
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

        public List<IComboResult> GetComboList(string[] requestFields)
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
            [System.ComponentModel.Description("štandard")]
            standard = 1,
            [System.ComponentModel.Description("P.O.Box")]
            POBox = 2,
            [System.ComponentModel.Description("neštruktúrované údaje")]
            nestrukturovaneUdaje = 3,
            [System.ComponentModel.Description("pobyt na obci")]
            pobytNaObci = 4,
        }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields)
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
                1 => GetDescription(VzorAdresyEnum.standard),
                2 => GetDescription(VzorAdresyEnum.POBox),
                3 => GetDescription(VzorAdresyEnum.nestrukturovaneUdaje),
                4 => GetDescription(VzorAdresyEnum.pobytNaObci),
                _ => "",
            };
        }

        public static string GetDescription(Enum e)
        {
            Type genericEnumType = e.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(e.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return e.ToString();
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

        public List<IComboResult> GetComboList(string[] requestFields)
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
    public class RegRzpDefiniciaCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public string KodPolozky { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public Dictionary<string, string> RequiredFields { get; set; }

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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

        public List<IComboResult> GetComboList(string[] requestFields)
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
}
