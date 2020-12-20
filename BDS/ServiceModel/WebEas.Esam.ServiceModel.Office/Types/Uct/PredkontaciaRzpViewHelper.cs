using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Uct.Types
{
    [DataContract]
    [Schema("uct")]
    [Alias("V_PredkontaciaRzp")]
    public class PredkontaciaRzpViewHelper : BaseTenantEntityNullable
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long C_PredkontaciaRzp_Id { get; set; }

        [DataMember]
        public long C_Predkontacia_Id { get; set; }

        [DataMember]
        public int? C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public short Poradie { get; set; }

        [DataMember]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public bool Polozka { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public long? C_Druh_Id { get; set; }

        [DataMember]
        public long? C_Kod_Id { get; set; }

        [DataMember]
        public long? C_Odsek_Id { get; set; }

        [DataMember]
        public short? C_OsobaTyp_Id { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public decimal? Percento { get; set; }

        [DataMember]
        public string DapRok { get; set; }

        [DataMember]
        public string Nazov { get; set; }
        
        [DataMember]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public byte? PrijemVydaj { get; set; }
    }
}
