using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Fin
{
    [DataContract]
    [Schema("fin")]
    [Alias("V_DokladBANPol")]
    public class DokladBANPolViewHelper : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_DokladBANPol_Id { get; set; }

        [DataMember]
        public short Rok { get; set; }

        [DataMember]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        public string BP_Popis { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public int C_Typ_Id { get; set; }

        //[DataMember]
        //public int? C_StavEntity_Id_Parovanie { get; set; }

        [DataMember]
        public string VS { get; set; }

        [DataMember]
        public string SS { get; set; }

        [DataMember]
        public string KS { get; set; }

        [DataMember]
        public DateTime DatumPohybu { get; set; }

        [DataMember]
        public DateTime DatumValuta { get; set; }

        //[DataMember]
        //public string Osoba { get; set; }

        //[DataMember]
        //public string OsobaBankaUcetNazov { get; set; }

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
        public decimal Suma { get; set; }

        //[DataMember]
        //public string IBAN { get; set; }

        //[DataMember]
        //public string BIC { get; set; }

        //[DataMember]
        //public int C_StavEntity_Id { get; set; }

        [DataMember]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        public int RzpDefinicia { get; set; }
    }
}
