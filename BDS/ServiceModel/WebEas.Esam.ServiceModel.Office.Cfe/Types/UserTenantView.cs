using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_UserTenant")]
    [DataContract]
    public class UserTenantView : UserTenant, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "User name")]
        [PfeCombo(typeof(UserView), IdColumn = nameof(D_User_Id), ComboDisplayColumn = nameof(UserView.FullName))]
        public string UserName { get; set; }

        [DataMember]
        [PfeColumn(Text = "City")]
        [PfeCombo(typeof(TenantView), IdColumn = nameof(D_Tenant_Id), ComboDisplayColumn = nameof(TenantView.Nazov))]
        public string TenantName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
