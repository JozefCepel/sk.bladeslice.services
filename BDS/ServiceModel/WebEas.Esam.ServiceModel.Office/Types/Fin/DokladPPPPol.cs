using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [DataContract]
    [Schema("fin")]
    [Alias("D_DokladPPPPol")]
    public class DokladPPPPol : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_DokladPPPPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id")]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pč", Editable = true, Mandatory = true)]
        public int Poradie { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Typ_Id", Mandatory = true)]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public long? D_BiznisEntita_Id_Predpis { get; set; }
        
        [DataMember]
        public long? D_Vymer_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "VS", Tooltip = "Variabilný symbol")]
        [StringLength(40)]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "ŠS", Tooltip = "Špecifický symbol")]
        [StringLength(10)]
        public string SS { get; set; }

        [DataMember]
        [PfeColumn(Text = "KS", Tooltip = "Konštantný symbol")]
        [StringLength(4)]
        public string KS { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }
        
        [DataMember]
        public long? D_OsobaBankaUcet_Id { get; set; }
        
        [DataMember]
        [PfeColumn(Text = "Príjemca", Mandatory = true)]
        [StringLength(70)]
        public string Prijemca { get; set; }

        [DataMember]
        [PfeColumn(Text = "IBAN", Mandatory = true)]
        [StringLength(40)]
        public string IBAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "BIC", Mandatory = true)]
        [StringLength(15)]
        public string BIC { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        [StringLength(140)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma", Mandatory = true)]
        public decimal DM_Cena { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CM_Cena", DefaultValue = 0)]
        public decimal CM_Cena { get; set; }
    }
}
