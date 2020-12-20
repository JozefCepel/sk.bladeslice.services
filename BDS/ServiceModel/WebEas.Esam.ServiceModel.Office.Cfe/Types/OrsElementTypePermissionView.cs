using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("V_OrsElementTypePermission")]
    public class OrsElementTypePermissionView : OrsElementTypePermission, IBaseView
    {
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
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        [IgnoreInsertOrUpdate]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        [IgnoreInsertOrUpdate]
        public string ZmenilMeno { get; set; }
    }
}

