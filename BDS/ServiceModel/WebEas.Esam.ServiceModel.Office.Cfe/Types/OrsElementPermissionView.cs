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
    public class OrsElementPermissionView : OrsElementPermission, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Typ oddelenia", NameField = "C_OrsElementType_Id")]
        public int C_OrsElementType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "IdValue", Hidden = true)]
        [PfeCombo(typeof(OrsElementTypeView), IdColumn = nameof(C_OrsElement_Id), ComboDisplayColumn = nameof(OrsElementTypeView.Nazov))]
        [IgnoreInsertOrUpdate]
        public int? IdValue { get; set; }

        [DataMember]
        [PfeColumn(Text = "ListValue", Hidden = true)]
        [PfeCombo(typeof(OrsElementTypeView), IdColumn = nameof(C_OrsElement_Id), ComboDisplayColumn = nameof(OrsElementTypeView.Nazov))]
        [IgnoreInsertOrUpdate]
        public string ListValue { get; set; }

        [DataMember]
        [PfeColumn(Text = "C_Modul_Id", Hidden = true, ReadOnly = true, NameField = "C_Modul_Id")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "RoleNazov")]
        [PfeCombo(typeof(RoleView), IdColumn = nameof(C_Role_Id), ComboDisplayColumn = nameof(RoleView.Nazov))]
        [IgnoreInsertOrUpdate]
        public string RoleNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "UserName")]
        [PfeCombo(typeof(UserView), IdColumn = nameof(D_User_Id), ComboDisplayColumn = nameof(UserView.FullName))]
        [IgnoreInsertOrUpdate]
        public string UserName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        [IgnoreInsertOrUpdate]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        [IgnoreInsertOrUpdate]
        public string ZmenilMeno { get; set; }
    }
}

