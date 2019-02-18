using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_PRCiele")]
    [PfeCaption("Evidencia - Ciele")]
    [DataContract]
    public class CieleView : Ciele
    {
        [DataMember]
        [PfeColumn(Text = "Kód programu", ReadOnly = true)]
        public string ProgramKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov programu", ReadOnly = true)]
        public string ProgramNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program")]
        [PfeCombo(typeof(ProgramView), NameColumn = "D_Program_Id")]
        public string PRFull { get; set; }

        //Pomocny stlpec pre LayoutDependencies
        [DataMember]
        [PfeColumn(Text = "_P", ReadOnly = true)]
        public short? P { get; set; }

        //Pomocny stlpec pre LayoutDependencies
        [DataMember]
        [PfeColumn(Text = "_PP", ReadOnly = true)]
        public string PP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zodpovedný")]
        [PfeCombo(typeof(CmbOsobaView), NameColumn = "D_User_Id_Zodp", DisplayColumn = "FullName")]
        public string Zodpovedny { get; set; }

        [DataMember]
        [PfeCombo(typeof(CieleTypCombo), NameColumn = "Typ")]
        [PfeColumn(Text = "Typ")]
        [Ignore]
        public string TypText
        {
            get
            {
                return CieleTypCombo.GetText(Typ);
            }
        }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}