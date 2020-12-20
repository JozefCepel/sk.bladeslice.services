using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [Schema("cfe")]
    [Alias("V_OrsElementUser")]
    [DataContract]
    public class OrsElementUserView : OrsElement, IBaseView
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "V_OrsElementUser_Id")]
        public string V_OrsElementUser_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TypeElement_Id")]
        [Ignore]
        public string TypeElement_Id { get { return C_OrsElementType_Id + "_" + IdValue; } }

        [DataMember]
        [HierarchyNodeParameter]
        [PfeColumn(Text = "_V_OrsElementTypeUser_Id", Hidden = true, NameField = "V_OrsElementTypeUser_Id", IdField = "V_OrsElementTypeUser_Id")]
        public string V_OrsElementTypeUser_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Modul_Id", Hidden = true, NameField = "C_Modul_Id", IdField = "C_Modul_Id")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "D_User_Id", ReadOnly = true)]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "D_OrsElementPermission_Id")]
        public int? D_OrsElementPermission_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_IsPravoElement")]
        public bool IsElementPravo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PravoReal")]
        public byte PravoReal { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prístup")]
        [Ignore]
        public string PravoText
        {
            get
            {
                return (
                            (PravoReal == (int)Pravo.Ziadne)    ? "<žiadny>" :
                            (PravoReal == (int)Pravo.Citat)     ? "Čítať" :
                            (PravoReal == (int)Pravo.Upravovat) ?"Upravovať" :
                            (PravoReal == (int)Pravo.Full)      ? "Plný" : PravoReal + " (?)"
                        ) + 
                        (
                            (!IsElementPravo && PravoReal > (int)Pravo.Ziadne) ? "- zdedené" : ""
                        );
            }
        }

        [DataMember]
        [PfeColumn(Text = "Meno")]
        [PfeCombo(typeof(OrsElementTypeUsersView), ComboDisplayColumn = nameof(OrsElementTypeUsersView.FullName), ComboIdColumn = nameof(OrsElementTypeUsersView.D_User_Id))]
        public string FullName { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Deleted")]
        public new bool Deleted { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true, LoadWhenVisible = true)]
        public string ZmenilMeno { get; set; }

    }
}
