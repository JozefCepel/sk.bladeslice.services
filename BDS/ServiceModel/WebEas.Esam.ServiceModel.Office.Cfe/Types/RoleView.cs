using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Role")]
    [DataContract]
    public class RoleView : Role, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Nadradená rola")]
        [PfeCombo(typeof(RoleView), IdColumn = nameof(C_Role_Id_Parent), ComboDisplayColumn = nameof(RoleView.Nazov))]
        public string ParentNazov { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
