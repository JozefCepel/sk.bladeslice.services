using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("V_Department")]
    [DataContract]
    public class DepartmentView : Department
    {
        [DataMember]
        [PfeCombo(typeof(DepartmentCombo), NameColumn = "Typ")]
        [PfeColumn(Text = "Typ")]
        [Ignore]
        public string TypText
        {
            get
            {
                return DepartmentCombo.GetText(Typ);
            }
        }

        [DataMember]
        [PfeColumn(Text = "Zodpovedný")]
        [PfeCombo(typeof(UserView), NameColumn = "Zodpovedny_Id", DisplayColumn = "FullName")]
        [IgnoreInsertOrUpdate]
        public string Zodpovedny { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nadradenné oddelenie")]
        [PfeCombo(typeof(DepartmentView), NameColumn = "C_Department_Id_Parent", DisplayColumn = "Nazov")]
        [IgnoreInsertOrUpdate]
        public string ParentName { get; set; }

        //audit stlpce
        [DataMember]
        [PfeColumn(Text = "Vytvoril", Hidden = true, Editable = false, ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string VytvorilMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Hidden = true, Editable = false, ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string ZmenilMeno { get; set; }
    }
}