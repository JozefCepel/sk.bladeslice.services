using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.Esam.ServiceModel.Office.Types.Osa;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_BiznisEntita")]
    public class BiznisEntita : BaseTenantEntity, IValidateConstraint
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long D_BiznisEntita_Id { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true, Hideable = false, Description = "Id biznisentity z externého systému (MADE...)")]
        public long? D_BiznisEntita_Id_Externe { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Id")]
        public short C_TypBiznisEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_TypBiznisEntity_Kniha_Id", Mandatory = true)]
        public int C_TypBiznisEntity_Kniha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Stav entity", DefaultValue = (int)StavEntityEnum.NOVY, Editable = false)] //Editable = false spôsobi, ze pri funkcii COPY nebude field kopirovat
        public int C_StavEntity_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Stavový priestor")]
        public int C_StavovyPriestor_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Stredisko")]
        public int? C_Stredisko_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Bank. účet")]
        public int? C_BankaUcet_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Pokladnica")]
        public int? C_Pokladnica_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Projekt_Id")]
        public long? C_Projekt_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Položka stromu")]
        public string PolozkaStromu { get; set; }

        [DataMember]
        [PfeColumn(Text = "ÚO", Editable = false, ReadOnly = true)]
        public byte UOMesiac { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", Editable = false)]
        public short Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum dokladu", Type = PfeDataType.Date)]
        public DateTime DatumDokladu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum prijatia", Type = PfeDataType.Date)]
        public DateTime? DatumPrijatia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum vystavenia", Type = PfeDataType.Date)]
        public DateTime? DatumVystavenia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum splatnosti", Type = PfeDataType.Date)]
        public DateTime? DatumSplatnosti { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum dodania", Type = PfeDataType.Date)]
        public DateTime? DatumDodania { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Dátum VDP", Type = PfeDataType.Date)]
        public DateTime? DatumVDP { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo", Editable = false, DefaultValue = 0)]
        public int Cislo { get; set; }

        [DataMember]
        [PfeColumn(Text = "Číslo dokladu", Tooltip = "Interné číslo dokladu", Editable = false)]
        public string CisloInterne { get; set; }

        [DataMember]
        [PfeColumn(Text = "Externé č.")]
        public string CisloExterne { get; set; }

        [DataMember]
        [PfeColumn(Text = "VS", Tooltip = "Variabilný symbol", Editable = false)]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_Osoba_Id")]
        public long? D_Osoba_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Suma", Tooltip = "Suma dokladu (v domácej mene)", DefaultValue = 0, ReadOnly = true)]
        public decimal DM_Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "Centové vyrovnanie", Tooltip = "Centové vyrovnanie (v domácej mene)", DefaultValue = 0)]
        public decimal DM_CV { get; set; }

        [DataMember]
        [PfeColumn(Text = "Základ - bez DPH", Tooltip = "Suma základu pre nulovú sadzbu DPH (v domácej mene)", DefaultValue = 0)]
        public decimal DM_Zak0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Základ - znížená sadzba", Tooltip = "Suma základov znížených sadzieb DPH (v domácej mene)", DefaultValue = 0)]
        public decimal DM_Zak1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Základ - základná sadzba", Tooltip = "Suma základu pre základnú sadzbu DPH (v domácej mene)", DefaultValue = 0)]
        public decimal DM_Zak2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "DPH - znížená sadzba", Tooltip = "Suma DPH pre znížené sadzby DPH (v domácej mene)", DefaultValue = 0)]
        public decimal DM_DPH1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "DPH - základná sadzba", Tooltip = "Suma DPH pre základnú sadzbu DPH (v domácej mene)", DefaultValue = 0)]
        public decimal DM_DPH2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-Suma", Tooltip = "Suma dokladu (v mene dokladu)", DefaultValue = 0, ReadOnly = true)]
        public decimal CM_Suma { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-Centové vyrovnanie", Tooltip = "Centové vyrovnanie (v mene dokladu)", DefaultValue = 0)]
        public decimal CM_CV { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-Základ - bez DPH", Tooltip = "Suma základu pre nulovú sadzbu DPH (v mene dokladu)", DefaultValue = 0)]
        public decimal CM_Zak0 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-Základ - znížená sadzba", Tooltip = "Suma základov znížených sadzieb DPH (v mene dokladu)", DefaultValue = 0)]
        public decimal CM_Zak1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-Základ - základná sadzba", Tooltip = "Suma základu pre základnú sadzbu DPH (v mene dokladu)", DefaultValue = 0)]
        public decimal CM_Zak2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-DPH - znížená sadzba", Tooltip = "Suma DPH pre znížené sadzby DPH (v mene dokladu)", DefaultValue = 0)]
        public decimal CM_DPH1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Z-DPH - základná sadzba", Tooltip = "Suma DPH pre základnú sadzbu DPH (v mene dokladu)", DefaultValue = 0)]
        public decimal CM_DPH2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Mena", DefaultValue = (short)MenaEnum.EUR )]
        public short C_Mena_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KurzECB")]
        public decimal KurzECB { get; set; }

        [DataMember]
        [PfeColumn(Text = "_KurzBanka")]
        public decimal KurzBanka { get; set; }

        [DataMember]
        [PfeColumn(Text = "PS", Editable = false)]
        public bool PS { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Lokalita", DefaultValue = 1)]
        public byte C_Lokalita_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.Textarea)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_Predkontacia_Id")]
        public long? C_Predkontacia_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_OsobaKontakt_Id_Komu")]
        public long? D_OsobaKontakt_Id_Komu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kontaktná osoba", RequiredFields = new[] { nameof(D_Osoba_Id) })]
        [PfeCombo(typeof(OsobaKontaktView), IdColumn = nameof(D_OsobaKontakt_Id_Komu), ComboDisplayColumn = nameof(OsobaKontaktView.FormatMenoCombo),
                    CustomSortSqlExp = nameof(OsobaKontaktView.Hlavny) + " DESC, " + nameof(OsobaKontaktView.ZastupcaMeno) + " ASC",
                    AdditionalWhereSql = nameof(OsobaKontaktView.FormatMenoCombo) + " <> ''", AllowComboCustomValue = true)]
        public string OsobaKontaktKomu { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_ADR_Adresa_Id")]
        public long? D_ADR_Adresa_Id { get; set; }

        public string ChangeConstraintMessage(string constraintName, int errorCode, WebEasSqlKnownErrorType errorType)
        {
            if (constraintName.StartsWith("IX_D_BiznisEntita_CisloInterne")) //IX_D_BiznisEntita_CisloInterne1, 2, 3
            {
                return "Nastala chyba pri uložení. 'Číslo dokladu' nie je jedinečné.";
            }

            return null;
        }
    }
}
