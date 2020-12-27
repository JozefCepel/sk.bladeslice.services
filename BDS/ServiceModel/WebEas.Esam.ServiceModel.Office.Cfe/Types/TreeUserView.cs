using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_TreeUser")]
    [DataContract]
    public class TreeUserView : Tree, IBaseView
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "V_TreeUser_Id")]
        public string V_TreeUser_Id { get; set; }

        [DataMember]
        [HierarchyNodeParameter]
        [PfeColumn(Text = "_C_Modul_Id", Hidden = true, NameField = "C_Modul_Id", IdField = "C_Modul_Id")]
        public new int C_Modul_Id { get; set; }

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
        //[PrimaryKey]
        [PfeColumn(Text = "D_TreePermission_Id")]
        public int? D_TreePermission_Id { get; set; }

        private byte? pravo;

        [DataMember]
        [PfeColumn(Text = "_Pravo", Mandatory = true)]
        public byte? Pravo
        {
            get
            {
                if ((pravo == null || pravo == 0) && PravoMaster)
                {
                    if (PravoZdedene > 1) return PravoZdedene;
                    return 1; //Aspoň čítať
                }

                return pravo;
            }

            set
            {
                pravo = value;
            }
        }

        [DataMember]
        [PfeColumn(Text = "_PravoMaster")]
        public bool PravoMaster { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PravoZdedene")]
        public byte? PravoZdedene { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prístup")]
        [Ignore]
        [PfeCombo(typeof(PravoCombo), IdColumn = nameof(Pravo), ComboDisplayColumn = nameof(PravoCombo.Nazov))]
        public string PravoText
        {
            get
            {
                if (Pravo != null)
                {
                    return (Pravo == 0) ? "<žiadny>" : (Pravo == 1)? "Čítať" : (Pravo == 2)? "Upravovať" : (Pravo == 3)? "Plný" : Pravo + " (?)";
                }

                return (PravoZdedene == 1)? "Čítať - zdedené" : 
                       (PravoZdedene == 2)? "Upravovať - zdedené" : 
                       (PravoZdedene == 3)? "Plný - zdedené" : 
                       "<žiadny>";
            }
        }

        [DataMember]
        [PfeColumn(Text = "Meno")]
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
