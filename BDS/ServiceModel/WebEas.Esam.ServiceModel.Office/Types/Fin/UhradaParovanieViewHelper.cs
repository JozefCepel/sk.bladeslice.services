using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [DataContract]
    [Schema("fin")]
    [Alias("V_UhradaParovanie")]
    public class UhradaParovanieViewHelper
    {
        [DataMember]
        [PrimaryKey]
        public long D_UhradaParovanie_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public short Rok_Predpis { get; set; } //Tento field nie je v gride vôbec zobrazený

        [DataMember]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id_Uhrada { get; set; }

        [DataMember]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public string UP_Popis { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public long? D_BiznisEntita_Id_Predpis { get; set; }

        [DataMember]
        public long? D_VymerPol_Id { get; set; }

        [DataMember]
        public long? D_VymerPol_Id_Externe { get; set; }

        [DataMember]
        public int C_Typ_Id { get; set; }

        [DataMember]
        public short? C_OsobaTyp_Id { get; set; }

        [DataMember]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        public decimal DM_Cena { get; set; }

        [DataMember]
        public decimal DM_Rozdiel { get; set; }

        [DataMember]
        public Guid? D_Osoba_Id_Externe { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        public DateTime? DatumValuta { get; set; }

        [DataMember]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        public long? C_UctKluc_Id1 { get; set; }

        [DataMember]
        public long? C_UctKluc_Id2 { get; set; }

        [DataMember]
        public long? C_UctKluc_Id3 { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public long? C_Druh_Id { get; set; }

        [DataMember]
        public long? C_Kod_Id { get; set; }

        [DataMember]
        public long? C_Odsek_Id { get; set; }

        [DataMember]
        public int RzpDefinicia { get; set; }

        [DataMember]
        public decimal? Suma { get; set; }

        [DataMember]
        public int? C_StavEntity_Id_Parovanie { get; set; }

        [DataMember]
        public long? D_Vymer_Id { get; set; }

        [DataMember]
        public string CisloInterne { get; set; }

        [DataMember]
        public decimal Uhradene { get; set; }

        [DataMember]
        public int C_StavEntity_Id { get; set; }
    }
}