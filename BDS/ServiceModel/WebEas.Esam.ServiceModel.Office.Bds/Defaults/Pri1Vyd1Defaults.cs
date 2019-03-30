using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Defaults
{
    public class Pri1Vyd1Defaults
    {
        [DataMember]
        public int? K_SKL_0 { get; set; }

        [DataMember]
        public int RANK { get; set; }
    }
}
