using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_TreePermission")]
    [DataContract]
    public class TreePermissionView : BaseEntity, IBaseView
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]        
        public int D_TreePermission_Id { get; set; }

        [DataMember]
        public Guid D_Tenant_Id { get; set; }

        [DataMember]
        public int C_Modul_Id { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Pravo")]
        public byte Pravo { get; set; }

        [DataMember]
        [PfeCombo(typeof(PravoCombo), IdColumn = nameof(Pravo))]
        [PfeColumn(Text = "Prístup")]
        [Ignore]
        public string PravoText
        {
            get
            {
                return PravoCombo.GetText(Pravo);
            }
        }

        [DataMember]
        public int? C_Role_Id { get; set; }

        [DataMember]
        public Guid? D_User_Id { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Created by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Edited by", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }
    }
}
