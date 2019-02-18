using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Rzp.Types
{
    [Schema("rzp")]
    [Alias("V_PRCieleUkaz")]
    [PfeCaption("Evidencia - Ciele - Ukazovatele")]
    [DataContract]
    public class CieleUkazView : CieleUkaz
    {
        [DataMember]
        [PfeColumn(Text = "Názov ciela", RequiredFields = new string[] { "D_Program_Id" })]
        [PfeCombo(typeof(CieleView), NameColumn = "D_PRCiele_Id")]
        public string CielNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zodpovedný")]
        [PfeCombo(typeof(CmbOsobaView), NameColumn = "D_User_Id_Zodp", DisplayColumn = "FullName")]
        public string Zodpovedny { get; set; }

        [DataMember]
        [PfeCombo(typeof(CieleUkazTypCombo), NameColumn = "Typ")]
        [PfeColumn(Text = "Typ")]
        [Ignore]
        public string TypText
        {
            get
            {
                return CieleUkazTypCombo.GetText(Typ);
            }
        }
        
        [DataMember]
        public long D_Program_Id { get; set; }

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

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        public string ZmenilMeno { get; set; }
    }
}