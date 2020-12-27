using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("cfe")]
    [Alias("V_User")]
    [DataContract]
    public class UserView : User, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Superior")]
        [PfeCombo(typeof(UserView), IdColumn = nameof(D_User_Id_Parent), ComboDisplayColumn = nameof(FullName))]
        [IgnoreInsertOrUpdate]
        public string ParentFullName { get; set; }

        [DataMember]
        [PfeColumn(Text = "User type")]
        [PfeCombo(typeof(UserTypeView), IdColumn = nameof(C_UserType_Id), ComboIdColumn = nameof(UserTypeView.C_UserType_Id), ComboDisplayColumn = nameof(UserTypeView.Nazov))]
        [IgnoreInsertOrUpdate]
        public string UserTypeNazov { get; set; }

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
