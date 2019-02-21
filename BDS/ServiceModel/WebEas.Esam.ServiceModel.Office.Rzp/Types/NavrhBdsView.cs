using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Reg.Types;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("V_NavrhZmenyBds")]
    [DataContract]
    public class NavrhBdsView : NavrhZmenyBds
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
