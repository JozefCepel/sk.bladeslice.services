using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [DataContract]
    public class UserView : User
    {
        [DataMember]
        [PfeColumn(Text = "Doménová skupina")]
        [Ignore]
        public bool DomenovaSkupina { get; set; }
    }
}