using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_RzpPol")]
    [DataContract]
    public class RzpPolPrijView : RzpPolView
    {
        [DataMember]
        [PfeColumn(Text = "_C_FRZdroj_Id", Mandatory = true)]
        public new long? C_FRZdroj_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FREK_Id", Mandatory = true)]
        public new int? C_FREK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zdroj")]
        [PfeCombo(typeof(FRZdrojView), NameColumn = "C_FRZdroj_Id", DisplayColumn = "ZdrojFull", AdditionalWhereSql = "(getDate() BETWEEN PlatnostOd AND ISNULL(PlatnostDo,getDate()+1)) AND Platny=1")]
        public string ZdrojFull { get; set; }

        [DataMember]
        [PfeColumn(Text = "Ekonomická klasifikácia")]
        [PfeCombo(typeof(FREKView), NameColumn = "C_FREK_Id", DisplayColumn = "EKFull", AdditionalWhereSql = "(PrijemVydaj = 1 AND Platny = 1 AND (getDate() BETWEEN PlatnostOd AND ISNULL(PlatnostDo,getDate()+1)))")]
        public string EKFull { get; set; }
    }
}