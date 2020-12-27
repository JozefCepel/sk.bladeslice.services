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
    [Alias("V_OrsElement")]
    public class OrsElementView : OrsElement, IBaseView
    {
        [DataMember]
        [PfeColumn(Text = "C_Modul_Id", Hidden = true, ReadOnly = true, NameField = "C_Modul_Id")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ Prvku ORŠ")]
        [PfeCombo(typeof(OrsElementTypeView), IdColumn = nameof(C_OrsElementType_Id), ComboDisplayColumn = nameof(OrsElementTypeView.Nazov))]
        public string TypNazov { get; set; }

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

