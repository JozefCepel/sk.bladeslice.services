using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Cfe;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Types;

namespace WebEas.Esam.ServiceModel.Office.Reg.Types
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_TextaciaPol")]
    public class TextaciaPol : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "Položka textácie ID", Mandatory = true)]
        public int C_TextaciaPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Textácia ID", Mandatory = true)]
        public int C_Textacia_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Vykonal")]
        public Guid? D_User_Id_Vykonal { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vykonal")]
        [PfeCombo(typeof(UserComboView), IdColumn = nameof(D_User_Id_Vykonal), ComboDisplayColumn = nameof(UserComboView.FullName),
                AdditionalWhereSql = "C_Modul_Id = 3", AllowComboCustomValue = true)]
        public string Vykonal { get; set; } // ID alebo Vykonal podla toho kt je vyplnene

        [DataMember]
        [PfeColumn(Text = "_DatumTyp")]
        public byte? DatumTyp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum", Type = PfeDataType.Date)]
        public DateTime? Datum { get; set; }

        [DataMember]
        [PfeColumn(Text = "Text")]
        [StringLength(255)]
        public string Text { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Tučné písmo")]
        public bool? PismoTucne { get; set; }

    }
}
