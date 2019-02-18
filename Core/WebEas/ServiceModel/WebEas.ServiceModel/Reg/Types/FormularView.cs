using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel.Office.Egov.Cis.Types;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_Formular")]
    [DataContract]
    public class FormularView : Formular
    {
        [DataMember]
        [PfeColumn(Text = "Jazyk", Rank = 7, Width = 80)]
        [PfeCombo(typeof(Jazyk), IdColumnCombo = "Id", NameColumn = "C_Jazyk_Id")]
        public string JazykNazov { get; set; }
    }
}