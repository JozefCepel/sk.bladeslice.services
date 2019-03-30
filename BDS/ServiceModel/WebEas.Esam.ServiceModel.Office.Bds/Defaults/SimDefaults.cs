using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Defaults
{
    public class SimDefaults
    {
        [DataMember]
        public int RANK { get; set; }

        [DataMember]
        public byte PV { get; set; }
    }
}
