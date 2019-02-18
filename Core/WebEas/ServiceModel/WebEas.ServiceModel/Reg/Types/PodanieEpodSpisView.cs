using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [DataContract]
    [Schema("zdv_int")]
    [Alias("V_EPOD_UradnyDokument_Spis")]
    public class PodanieEpodSpisView : PodanieEpodView
    {
        [DataMember]
        [Alias("riesitel")]
        public string Riesitel { get; set; }

        [PfeColumn(Text = "_Zberný spis", Hidden = true, Hideable = false, ReadOnly = true)]
        [IgnoreDataMember]
        public bool ZbernySpis { get; set; }
    }
}