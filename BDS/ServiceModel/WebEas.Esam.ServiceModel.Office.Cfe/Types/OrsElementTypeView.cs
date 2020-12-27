using ServiceStack.DataAnnotations;
using System;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("V_OrsElementType")]
    public class OrsElementTypeView : OrsElementType, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "Modul")]
        [PfeCombo(typeof(ModulView), IdColumn = nameof(C_Modul_Id), ComboDisplayColumn = nameof(ModulView.Nazov))]
        [IgnoreInsertOrUpdate]
        public string ModulNazov { get; set; }

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

