using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_UserRole")]
    [DataContract]
    public class UserRoleView : UserRole, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Užívateľ")]
        [PfeCombo(typeof(UserView), IdColumn = nameof(D_User_Id), ComboDisplayColumn = nameof(UserView.FullName))]
        [IgnoreInsertOrUpdate]
        public string UserName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rola")]
        [PfeCombo(typeof(RoleView), IdColumn = nameof(C_Role_Id), ComboDisplayColumn = nameof(RoleView.Nazov))]
        [IgnoreInsertOrUpdate]
        public string RoleName { get; set; }

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
