﻿using ServiceStack;
using System;
using System.Runtime.Serialization;

namespace WebEas.Esam.ServiceModel.Office.Bds.Dto
{
    // Get type of parameter
    [Route("/GetParameterType", "GET")]
    [Api("BdsIne")]
    [DataContract]
    public class GetParameterType
    {
        [DataMember(IsRequired = true)]
        public string Nazov { get; set; }
    }

    // Update
    [Route("/UpdateNastavenie", "PUT")]
    [DataContract]
    public class UpdateNastavenie
    {
        [DataMember(IsRequired = true)]
        public string Nazov { get; set; }

        [DataMember]
        public string sHodn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public long iHodn { get; set; }

        [DataMember]
        public bool? bHodn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime dHodn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public DateTime tHodn { get; set; }

        [DataMember]
        [NotEmptyOrDefault]
        public decimal nHodn { get; set; }
    }
}