﻿using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_FRZdroj")]
    [PfeCaption("Rozpočet - Kódy zdrojov")]
    [DataContract]
    public class FRZdrojView : FRZdrojCis
    {
        [DataMember]
        [PfeColumn(Text = "Typ organizácie")]
        public string OrganizaciaTyp { get; set; }
    }
}
