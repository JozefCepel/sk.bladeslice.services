using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Osa
{
    [Schema("osa")]
    [Alias("V_OsobaKontakt")]
    [DataContract]
    public class OsobaKontaktView : OsobaKontakt, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "RČ / IČO", Mandatory = true, ReadOnly = true, Editable = false)]
        public string IdentifikatorOsoby { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno / Názov", ReadOnly = true, Editable = false)]
        public string FormatMenoSort { get; set; }

        [DataMember]
        [PfeColumn(Text = "_FormatMenoCombo")]
        [IgnoreInsertOrUpdate]
        public string FormatMenoCombo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FO_StavExistencny_Id", ReadOnly = true)]
        public short? C_FO_StavExistencny_Id { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
