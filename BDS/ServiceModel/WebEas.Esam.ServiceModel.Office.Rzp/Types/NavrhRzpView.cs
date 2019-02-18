using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_NavrhZmenyRzp")]
    [DataContract]
    public class NavrhRzpView : NavrhZmenyRzp
    {
        [DataMember]
        [PfeColumn(Text = "Zodpovedný")]
        [PfeCombo(typeof(CmbOsobaView), NameColumn = "D_User_Id_Zodp", DisplayColumn = "FullName")]
        public string Zodpovedny { get; set; }

        [DataMember]
        [PfeCombo(typeof(StavEntityView), NameColumn = "C_StavEntity_Id")]
        [PfeColumn(Text = "Stav", Editable = false)]
        public string StavNazov { get; set; }
    }
}
