using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("V_FREK")]
    [PfeCaption("Rozpočet - Ekonomická klasifikácia")]
    [DataContract]
    public class FREKView : FREKCis
    {
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
        [PfeCombo(typeof(TypBdsCis), NameColumn = "C_TypBds_Id")]
        [PfeColumn(Text = "Typ rozpočtu")]
        public string TypBdsName { get; set; }
    }
}
