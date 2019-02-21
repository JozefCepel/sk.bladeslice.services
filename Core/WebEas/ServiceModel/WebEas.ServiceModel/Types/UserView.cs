using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("cfe")]
    [Alias("V_Users")]
    [DataContract]
    public class UserView : User
    {
        [DataMember]
        [PfeColumn(Text = "Nadradený")]
        [PfeCombo(typeof(UserView), NameColumn = "D_User_Id_Parent", DisplayColumn = "FullName")]
        [IgnoreInsertOrUpdate]
        public string ParentFullName { get; set; }

        [DataMember]
        [PfeColumn(Text = "Celé meno", ReadOnly = true)]
        public string FullName { get; set; }

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
