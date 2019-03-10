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

        public static string GetText(byte kod)
        {
            switch (kod)
            {
                case 1: return "Príjem";
                case 2: return "Výdaj";
                default: return kod + " (?)";
            }
        }
    }
}
