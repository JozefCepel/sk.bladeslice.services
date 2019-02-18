using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_FormularPoleFormulara")]
    [DataContract]
    public class FormularPoleFormularaView
    {
        [DataMember]
        [PrimaryKey]
        public long C_Formular_Id { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "Formulár")]
        public string Nazov { get; set; }
    }
}