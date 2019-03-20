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

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case 1: return "P";
                case 2: return "V";
                default: return kod + " (?)";
            }
        }
    }

    [DataContract]
    public class DepartmentCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case 1: return "Oddelenie";
                case 2: return "Divízia";
                default: return kod + " (?)";
            }
        }
    }

    [DataContract]
    public class PravoCombo : IStaticCombo
    {
        [DataMember]
        public short Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 0, 1, 2, 3 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
        }

        public static string GetText(short kod)
        {
            switch (kod)
            {
                case 0: return "<žiadny>";  // none
                case 1: return "Čítať";     // read
                case 2: return "Upravovať"; // read, insert, update
                case 3: return "Plný";      // read, insert, update, delete
                default: return kod + " (?)";
            }
        }
    }

    [DataContract]
    public class CieleTypCombo : IStaticCombo
    {
        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 1, 2 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
        }

        public static string GetText(byte? kod)
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

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 1, 2, 3, 4 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
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

        public IWebEasRepositoryBase Repository { get; set; }

        public List<ComboResult> GetComboList(string[] requestFields)
        {
            byte[] list = new byte[] { 1, 2, 3 };
            return list.Select(a => new ComboResult() { Id = a.ToString(), Value = GetText(a) }).ToList();
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
}
