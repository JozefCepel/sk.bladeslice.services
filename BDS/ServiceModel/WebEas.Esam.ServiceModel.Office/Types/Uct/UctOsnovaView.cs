using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_UctOsnova")]
    public class UctOsnovaView : UctOsnova, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ")]
        public string TypText { get; set; }

        [DataMember]
        [PfeColumn(Text = "Druh")]
        public string DruhText { get; set; }

        [DataMember]
        [PfeColumn(Text = "Saldokontný")]
        public string SDKText { get; set; }

        [DataMember]
        [PfeColumn(Text = "Trieda")]
        public string Trieda { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        [IgnoreInsertOrUpdate]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        [IgnoreInsertOrUpdate]
        public string ZmenilMeno { get; set; }
    }
}
