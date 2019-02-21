using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("V_Program")]
    [DataContract]
    public class ProgramovyRozpocetPodprogramView : Program
    {
        [DataMember]
        [PfeColumn(Text = "_D_Program_Id_Parent")]
        [IgnoreInsertOrUpdate]
        public long? D_Program_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zodpovedný 1")]
        [PfeCombo(typeof(CmbOsobaView), NameColumn = "D_User_Id_Zodp1", DisplayColumn = "FullName")]
        public string Zodpovedny1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zodpovedný 2")]
        [PfeCombo(typeof(CmbOsobaView), NameColumn = "D_User_Id_Zodp2", DisplayColumn = "FullName")]
        public string Zodpovedny2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podprogram")]
        [HierarchyNodeParameter]
        public new short? podprogram { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}



