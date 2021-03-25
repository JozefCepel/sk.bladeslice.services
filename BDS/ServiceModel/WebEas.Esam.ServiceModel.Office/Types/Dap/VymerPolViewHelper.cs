using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Dap
{
    [Schema("dap")]
    [Alias("V_VymerPol")]
    [DataContract]
    public class VymerPolViewHelper
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_VymerPol_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public short C_VymerTyp_Id { get; set; }

        [DataMember]
        public DateTime DatumVyrubenia { get; set; }

        [DataMember]
        public DateTime? DatumPravoplatnosti { get; set; }

        [DataMember]
        public DateTime? DatumSplatnosti { get; set; } //Prvý nenulový z dátumov Dat01-Dat12

        [DataMember]
        public int C_StavEntity_Id_Zau { get; set; }

        [DataMember]
        public string C_StavEntity_Id_ZauPopis { get; set; }

        [DataMember]
        public string ZauctovanieDoklad { get; set; }

        [DataMember]
        public decimal? ZauctovanieSuma { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public short? C_OsobaTyp_Id { get; set; }

        [DataMember]
        public long C_Druh_Id { get; set; }

        [DataMember]
        public long C_Kod_Id { get; set; }

        [DataMember]
        public long C_Odsek_Id { get; set; }

        [DataMember]
        public decimal Suma { get; set; }

        [DataMember]
        public string KodDane { get; set; }

        [DataMember]
        public string DruhDane { get; set; }

        [DataMember]
        public string OdsekDane { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public int CisloVymeru { get; set; }

        [DataMember]
        public string RcIco { get; set; }

        [DataMember]
        public string MenoNazov { get; set; }
    }
}
