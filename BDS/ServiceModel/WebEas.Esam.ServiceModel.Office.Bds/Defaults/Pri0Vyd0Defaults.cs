using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Defaults
{
    public class Pri0Vyd0Defaults
    {
        [DataMember]
        public DateTime DAT_DKL { get; set; }

        [DataMember]
        public int K_SKL_0 { get; set; }

        [DataMember]
        public int K_ORJ_0 { get; set; }
    }
}
