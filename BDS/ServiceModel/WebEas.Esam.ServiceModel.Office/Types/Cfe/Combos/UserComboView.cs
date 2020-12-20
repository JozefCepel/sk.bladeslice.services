using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [Schema("cfe")]
    [Alias("V_UserCombo")]
    [PfeCaption("Zoznam zamestnancov")]
    [DataContract]
    public class UserComboView
    {
        [DataMember]
        [PrimaryKey]
        public Guid D_User_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Meno")]
        public string FullName { get; set; }

        [DataMember]
        public int C_Modul_Id { get; set; }

    }
}