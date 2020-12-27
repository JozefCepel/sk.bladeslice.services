using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_RightUser")]
    [DataContract]
    public class RightUserView : Right, IBaseView
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "V_RightUser_Id", Hidden = true, NameField = "V_RightUser_Id")]
        public string V_RightUser_Id { get; set; }

        [DataMember]
        [HierarchyNodeParameter]
        [PfeColumn(Text = "_C_Modul_Id", Hidden = true, NameField = "C_Modul_Id", IdField = "C_Modul_Id")]
        public new int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "ModulKod", ReadOnly = true)]
        public string ModulKod { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "Kód", ReadOnly = true)]
        //public string Kod { get; set; }

        //[DataMember]
        //[PfeColumn(Text = "Názov", ReadOnly = true)]
        //public string Nazov { get; set; }

        [DataMember]        
        [PfeColumn(Text = "D_User_Id", ReadOnly = true)]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "_D_RightPermission_Id", Hidden = true, ReadOnly = true)]
        public int? D_RightPermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Má právo")]
        public bool HasRight { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno", ReadOnly = true)]
        [PfeCombo(typeof(ModulUserView), ComboDisplayColumn = nameof(ModulUserView.FullName), ComboIdColumn = nameof(ModulUserView.D_User_Id))]
        public string FullName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
