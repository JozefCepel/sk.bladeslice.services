using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("V_OrsElementPermission")]
    public class OrsElementPermissionView : OrsElementPermission
    {
        [DataMember]
        [PfeColumn(Text = "IdValue")]
        [PfeCombo(typeof(OrsElementTypeView), NameColumn = "C_OrsElement_Id", DisplayColumn = "IdValue")]
        [IgnoreInsertOrUpdate]
        public int IdValue { get; set; }

        [DataMember]
        [PfeColumn(Text = "ListValue")]
        [PfeCombo(typeof(OrsElementTypeView), NameColumn = "C_OrsElement_Id", DisplayColumn = "ListValue")]
        [IgnoreInsertOrUpdate]
        public string ListValue { get; set; }

        [DataMember]
        [PfeColumn(Text = "RoleNazov")]
        [PfeCombo(typeof(RoleView), NameColumn = "C_Role_Id", DisplayColumn = "RoleNazov")]
        [IgnoreInsertOrUpdate]
        public string RoleNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "UserName")]
        [PfeCombo(typeof(UserView), NameColumn = "D_User_Id", DisplayColumn = "UserName")]
        [IgnoreInsertOrUpdate]
        public string UserName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string ZmenilMeno { get; set; }
    }
}

