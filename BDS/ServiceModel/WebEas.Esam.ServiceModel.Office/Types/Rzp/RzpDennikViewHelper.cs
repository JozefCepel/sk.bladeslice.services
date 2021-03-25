using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [Schema("rzp")]
    [Alias("V_RzpDennik")]
    [DataContract]
    public class RzpDennikViewHelper : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_RzpDennik_Id { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public bool R { get; set; }

        [DataMember]
        public byte? PrijemVydaj { get; set; }

        [DataMember]
        public long? C_RzpPol_Id { get; set; }

        [DataMember]
        public long? D_Program_Id { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public decimal Suma { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public string BiznisEntitaPopis { get; set; }

        [DataMember]
        [Ignore]
        public string PrijemVydajText => PrijemVydajCombo.GetText(PrijemVydaj);

        [DataMember]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        public string RzpPolNazov { get; set; }

        [DataMember]
        public string ProgramFull { get; set; }

        [DataMember]
        public string StrediskoNazov { get; set; }

        [DataMember]
        public string ProjektNazov { get; set; }

        [DataMember]
        public string ZdrojKod { get; set; }

        [DataMember]
        public string FK { get; set; }

        [DataMember]
        public string EK { get; set; }

        [DataMember]
        public string A1 { get; set; }

        [DataMember]
        public string A2 { get; set; }

        [DataMember]
        public string A3 { get; set; }
    }
}