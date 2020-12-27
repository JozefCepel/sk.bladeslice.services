using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_TenantType")]
    [DataContract]
    public class TenantTypeView : TenantType, IBaseView
    {
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
