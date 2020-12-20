using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel.Types
{
    [Schema("cfe")]
    [Alias("V_UserType")]
    [DataContract]
    public class UserTypeView : UserType, IBaseView
    {
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
