using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [DataContract]
    [Schema("cfe")]
    [Alias("V_OrsElementTypeUser")]
    public class OrsElementTypeUsersView : OrsElementType, IBaseView
    {
        [DataMember]
        [PrimaryKey]        
        public string V_OrsElementTypeUser_Id { get; set; }

        [DataMember]
        [HierarchyNodeParameter]
        [PfeColumn(Text = "_C_Modul_Id", Hidden = true, NameField = "C_Modul_Id", IdField = "C_Modul_Id")]
        public new int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "D_User_Id", ReadOnly = true)]
        public Guid D_User_Id { get; set; }

        [DataMember]
        //[PrimaryKey]
        [PfeColumn(Text = "D_TreePermission_Id")]
        public int? D_OrsElementTypePermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Prístup")]
        public byte Pravo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prístup")]
        [PfeCombo(typeof(PravoCombo), IdColumn = nameof(Pravo), ComboDisplayColumn = nameof(PravoCombo.Nazov), ComboIdColumn = nameof(PravoCombo.Kod))]
        public string PravoText { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno")]
        [PfeCombo(typeof(ModulUserView), ComboDisplayColumn = nameof(ModulUserView.FullName), ComboIdColumn = nameof(ModulUserView.D_User_Id))]
        public string FullName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

    }
}

