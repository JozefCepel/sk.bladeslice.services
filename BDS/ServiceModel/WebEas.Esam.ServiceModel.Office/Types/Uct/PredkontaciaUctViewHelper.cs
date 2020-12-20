using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_PredkontaciaUct")]
    public class PredkontaciaUctViewHelper : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long C_PredkontaciaUct_Id { get; set; }

        [DataMember]
        public long C_Predkontacia_Id { get; set; }

        [DataMember]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public short Poradie { get; set; }

        [DataMember]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public string TypNazov { get; set; }

        [DataMember]
        public bool Polozka { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public int? C_Pokladnica_Id { get; set; }

        [DataMember]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public long? C_Druh_Id { get; set; }

        [DataMember]
        public string DruhDane { get; set; }

        [DataMember]
        public long? C_Kod_Id { get; set; }

        [DataMember]
        public string KodDane { get; set; }

        [DataMember]
        public long? C_Odsek_Id { get; set; }

        [DataMember]
        public string OdsekDane { get; set; }

        [DataMember]
        public short? C_OsobaTyp_Id { get; set; }

        [DataMember]
        public string OsobaTypKod { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public decimal? Percento { get; set; }

        [DataMember]
        public string DapRok { get; set; }

        [DataMember]
        public short SadzbaDph_Id { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public string SS { get; set; }

        [DataMember]
        public string KS { get; set; }

        [DataMember]
        public byte? C_Lokalita_Id { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public long? C_UctRozvrh_Id_MD { get; set; }

        [DataMember]
        public long? C_UctRozvrh_Id_Dal { get; set; }

        [DataMember]
        public string SDK { get; set; }
    }
}