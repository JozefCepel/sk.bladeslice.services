using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_TenantUsers")]
    [DataContract]
    public class TenantUsersView : TenantUsers
    {
        [DataMember]
        [PfeColumn(Text = "Užívateľ")]
        [PfeCombo(typeof(UserView), NameColumn = "D_User_Id", DisplayColumn = "FullName")]
        [IgnoreInsertOrUpdate]
        public string UserName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Tenant")]
        [PfeCombo(typeof(TenantView), NameColumn = "D_Tenant_Id", DisplayColumn = "Nazov")]
        [IgnoreInsertOrUpdate]
        public string TenantName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Oddelenie")]
        [PfeCombo(typeof(DepartmentView), NameColumn = "D_Department_Id", DisplayColumn = "Nazov")]
        [IgnoreInsertOrUpdate]
        public string DepartmentName { get; set; }

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
