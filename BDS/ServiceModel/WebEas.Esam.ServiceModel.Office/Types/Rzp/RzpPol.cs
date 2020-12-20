using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [DataContract]
    [Schema("rzp")]
    [Alias("C_RzpPol")]
    [SqlValidation(Operation.Update,
        "SELECT SUM(CASE WHEN DatumDokladu NOT BETWEEN @PlatnostOd AND ISNULL(@PlatnostDo, '2100-01-01') THEN 1 ELSE 0 END), MIN(DatumDokladu), MAX(DatumDokladu) " +
        "FROM rzp.V_RzpDennik " +
        "WHERE C_RzpPol_Id = @C_RzpPol_Id AND R = 1 AND D_Tenant_Id = @D_Tenant_Id",
        "DatumDokladu", "Nie je možné zmeniť požadovanú platnosť rozpočtovej položky! Na položke je účtované v rozmedzí < MinDatumDokladu; MaxDatumDokladu >.")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1), MIN(DatumDokladu), MAX(DatumDokladu) " +
        "FROM rzp.V_RzpDennik " +
        "WHERE C_RzpPol_Id = @C_RzpPol_Id AND R = 1 AND D_Tenant_Id = @D_Tenant_Id",
        "DatumDokladu", "Rozpočtovú položku nie je možné zmazať! Je použitá na zaúčtovaných dokladoch v rozmedzí < MinDatumDokladu; MaxDatumDokladu >. Na zneaktívnenie položky využite platnosť záznamu od-do.")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1), MIN(RzpDatum), MAX(RzpDatum) " +
        "FROM rzp.V_RzpPolZmena " +
        "WHERE C_RzpPol_Id = @C_RzpPol_Id AND C_StavEntity_Id NOT IN (1, 3) AND D_Tenant_Id = @D_Tenant_Id", //Stav - Nový, Neschválený
        "RzpDatum", "Rozpočtovú položku nie je možné zmazať! Nachádza sa v schválených zmenách rozpočtu v rozmedzí < MinRzpDatum; MaxRzpDatum >.")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1) " +
        "FROM rzp.V_RzpPolNavrh " +
        "WHERE C_RzpPol_Id = @C_RzpPol_Id AND C_StavEntity_Id NOT IN (1, 3) AND D_Tenant_Id = @D_Tenant_Id", //Stav - Nový, Neschválený
        "", "Rozpočtovú položku nie je možné zmazať! Nachádza sa v schválenom rozpočte.")]
    [SqlValidation(Operation.Update,
        "SELECT ISNULL(SUM(SCHVALENY_ROZPOCET + UPRAVY), 0.00) AS RzpUpraveny " +
        "FROM rzp.V_RzpPolVal " +
        "WHERE Rok = YEAR(@PlatnostDo) AND C_RzpPol_Id = @C_RzpPol_Id AND D_Tenant_Id = @D_Tenant_Id",
        "",
        "Nie je možné zmeniť požadovanú platnosť! Na rozpočtovej položke je nenulový rozpočet po zmenách.")]
    [SqlValidation(Operation.Update,
        "SELECT CASE WHEN SUM(RzpSkutocnost) <> 0 THEN 1 ELSE 0 END " +
        "FROM rzp.F_RzpSkutocnost(YEAR(@PlatnostDo), 12, 0) " +
        "WHERE C_RzpPol_Id = @C_RzpPol_Id", //Funkcia filtruje Tenanta vo vnútri
        "",
        "Nie je možné zmeniť požadovanú platnosť! Na rozpočtovej položke je nenulový zostatok skutočného čerpania/plnenia.")]
    [SqlValidation(Operation.Update,
        "SELECT CASE WHEN SUM(RzpSkutocnostPredb) <> 0 THEN 1 ELSE 0 END " +
        "FROM rzp.F_RzpSkutocnost(YEAR(@PlatnostDo), 12, 4) " +
        "WHERE C_RzpPol_Id = @C_RzpPol_Id", //Funkcia filtruje Tenanta vo vnútri
        "",
        "Nie je možné zmeniť požadovanú platnosť! Na rozpočtovej položke je nenulový zostatok predbežného čerpania/plnenia.")]
    public class RzpPol : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long C_RzpPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FRZdroj_Id")]
        public long? C_FRZdroj_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FRFK_Id")]
        public int? C_FRFK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_FREK_Id")]
        public int? C_FREK_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "A1")]
        [StringLength(6)]
        public string A1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "A2")]
        [StringLength(4)]
        public string A2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "A3")]
        [StringLength(4)]
        public string A3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Príjem/Výdaj")]
        public byte PrijemVydaj { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko")]
        public bool Stredisko { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt")]
        public bool Projekt { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovať na opačnej strane")]
        public bool OpacnaStrana { get; set; }

        [DataMember]
        [PfeColumn(Text = "ID pôvodné", ReadOnly = true, Editable = false)]
        public string Old_Id { get; set; }
    }
}
