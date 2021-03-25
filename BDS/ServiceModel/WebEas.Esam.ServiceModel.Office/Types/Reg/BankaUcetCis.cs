using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("C_BankaUcet")]
    public class BankaUcetCis : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_BankaUcet_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Pokladnica_Id_Externe")]
        public string C_BankaUcet_Id_Externe { get; set; }

        [DataMember]
        [PfeCombo(typeof(BankaView),
            ComboDisplayColumn = nameof(BankaView.Kod), 
            ComboIdColumn = nameof(BankaView.Kod),
            AdditionalFields = new[] { nameof(BankaView.Nazov), nameof(BankaView.BIC) },
            AllowComboCustomValue = true, Tpl = "{value};{Nazov}")]
        [PfeColumn(Text = "Kód banky", Mandatory = true)]
        [StringLength(30)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov účtu", Mandatory = true)]
        [StringLength(255)]
        [PfeValueColumn]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "IBAN", Mandatory = true)]
        [StringLength(40)]
        public string IBAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "BIC", Mandatory = true)]
        [StringLength(15)]
        public string BIC { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Mandatory = true)]
        public byte Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "DCOM", DefaultValue = 0, ReadOnly = true)]
        public bool? DCOM { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Mena_Id", Mandatory = true, DefaultValue = (short)MenaEnum.EUR)]
        public short C_Mena_Id { get; set; }
    }
}