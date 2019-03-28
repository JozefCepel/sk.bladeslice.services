using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office
{
    [DataContract]
    public class PV3DCombo : IStaticCombo
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
                case 1: return "+";
                case 2: return "-";
                default: return kod + " (?)";
            }
        }
    }

    [DataContract]
    public class SimulationType : IStaticCombo
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

}
