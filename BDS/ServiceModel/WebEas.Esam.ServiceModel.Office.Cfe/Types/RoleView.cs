using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Role")]
    [DataContract]
    public class RoleView : Role 
    {
        [DataMember]
        [PfeColumn(Text = "Nadradená rola")]
        [PfeCombo(typeof(RoleView), NameColumn = "C_Roles_Id_Parent", DisplayColumn = "Nazov")]
        [IgnoreInsertOrUpdate]
        public string ParentNazov { get; set; }

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
