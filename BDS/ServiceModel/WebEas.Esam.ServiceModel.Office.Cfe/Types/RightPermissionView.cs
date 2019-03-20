using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("V_RightPermission")]
    public class RightPermissionView : RightPermission
    {
        [DataMember]
        [PfeColumn(Text = "Práva", RequiredFields = new[] { "C_Modul_Id" })]
        [PfeCombo(typeof(RightView), NameColumn = "C_Right_Id", DisplayColumn = "Nazov")]
        public string RightNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rola")]
        [PfeCombo(typeof(RoleView), NameColumn = "C_Role_Id", DisplayColumn = "Nazov")]
        [IgnoreInsertOrUpdate]
        public string RolaNazov { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "C_Modul_Id", Hidden = true, ReadOnly = true)]
        //public string C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "User")]
        [PfeCombo(typeof(RightView), NameColumn = "C_Role_Id", DisplayColumn = "UserMeno")]
        [IgnoreInsertOrUpdate]
        public string UserMeno { get; set; }

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

