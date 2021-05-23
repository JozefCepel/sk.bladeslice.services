using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Rzp
{
    [DataContract]
    [Schema("rzp")]
    [Alias("D_Program")]
    [SqlValidation(Operation.Update,
        "SELECT SUM(CASE WHEN DatumDokladu NOT BETWEEN @PlatnostOd AND ISNULL(@PlatnostDo, '2100-01-01') THEN 1 ELSE 0 END), MIN(DatumDokladu), MAX(DatumDokladu) " +
        "FROM rzp.V_RzpDennik " +
        "WHERE D_Program_Id = @D_Program_Id AND R = 1 AND D_Tenant_Id = @D_Tenant_Id",
        "DatumDokladu", "Nie je možné zmeniť požadovanú platnosť! Na programe je účtované v rozmedzí < MinDatumDokladu; MaxDatumDokladu >.")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1), MIN(DatumDokladu), MAX(DatumDokladu) " +
        "FROM rzp.V_RzpDennik " +
        "WHERE D_Program_Id = @D_Program_Id AND R = 1 AND D_Tenant_Id = @D_Tenant_Id",
        "DatumDokladu", "Program nie je možné zmazať! Je použitý na zaúčtovaných dokladoch v rozmedzí < MinDatumDokladu; MaxDatumDokladu >. Na zneaktívnenie využite platnosť záznamu od-do.")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1), MIN(RzpDatum), MAX(RzpDatum) " +
        "FROM rzp.V_RzpPolZmena " +
        "WHERE D_Program_Id = @D_Program_Id AND C_StavEntity_Id NOT IN (1, 3) AND D_Tenant_Id = @D_Tenant_Id", //Stav - Nový, Neschválený
        "RzpDatum", "Program nie je možné zmazať! Nachádza sa v schválených zmenách rozpočtu v rozmedzí < MinRzpDatum; MaxRzpDatum >.")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1) " +
        "FROM rzp.V_RzpPolNavrh " +
        "WHERE D_Program_Id = @D_Program_Id AND C_StavEntity_Id NOT IN (1, 3) AND D_Tenant_Id = @D_Tenant_Id", //Stav - Nový, Neschválený
        "", "Program nie je možné zmazať! Nachádza sa v schválenom rozpočte.")]
    [SqlValidation(Operation.Update,
        "SELECT ISNULL(SUM(SCHVALENY_ROZPOCET + UPRAVY), 0.00) AS RzpUpraveny " +
        "FROM rzp.V_RzpPolVal " +
        "WHERE Rok = YEAR(@PlatnostDo) AND D_Program_Id = @D_Program_Id AND D_Tenant_Id = @D_Tenant_Id",
        "",
        "Nie je možné zmeniť požadovanú platnosť! Na programe je nenulový rozpočet po zmenách.")]
    [SqlValidation(Operation.Update,
        "SELECT CASE WHEN SUM(RzpSkutocnost) <> 0 THEN 1 ELSE 0 END " +
        "FROM rzp.F_RzpSkutocnost(YEAR(@PlatnostDo), 12, 0) " +
        "WHERE D_Program_Id = @D_Program_Id", //Funkcia filtruje Tenanta vo vnútri
        "",
        "Nie je možné zmeniť požadovanú platnosť! Na programe je nenulový zostatok skutočného čerpania/plnenia.")]
    [SqlValidation(Operation.Update,
        "SELECT CASE WHEN SUM(RzpSkutocnostPredb) <> 0 THEN 1 ELSE 0 END " +
        "FROM rzp.F_RzpSkutocnost(YEAR(@PlatnostDo), 12, 4) " +
        "WHERE D_Program_Id = @D_Program_Id", //Funkcia filtruje Tenanta vo vnútri
        "",
        "Nie je možné zmeniť požadovanú platnosť! Na programe je nenulový zostatok predbežného čerpania/plnenia.")]
    public class ProgramCis : BaseTenantEntity, IPfeCustomize
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public byte Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ", ReadOnly = true, Editable = false)]
        [PfeCombo(typeof(ProgramTypCombo), IdColumn = nameof(Typ))]
        [IgnoreInsertOrUpdate]
        public string TypNazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program")]
        public short? Program { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podprogram")]
        public short? Podprogram { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prvok")]
        public short? Prvok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW, Text = "Zámer")]
        public string Zamer { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW, Text = "Popis")]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp1")]
        public Guid? D_User_Id_Zodp1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp2")]
        public Guid? D_User_Id_Zodp2 { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW, Text = "Komentár")]
        public string Komentar { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Old_Id", ReadOnly = true, Editable = false)]
        public string Old_Id { get; set; }

        public void CustomizeModel(PfeDataModel model, IWebEasRepositoryBase repository, HierarchyNode node, string filter, HierarchyNode masterNode)
        {
            if (model.Fields != null)
            {
                var progFld = model.Fields.FirstOrDefault(p => p.Name == nameof(Program));
                if (progFld != null)
                {
                    progFld.Validator ??= new PfeValidator { Messages = new List<PfeMessage>() };
                    progFld.Validator.Messages = progFld.Validator.Messages ?? new List<PfeMessage>(); //Validátor už je vytvorený ale bez Messages
                    progFld.Validator.Messages.Add(new PfeMessage
                    {
                        MessageType = PfeMessageType.MessageWarning,
                        Message = "Hodnota programu je vačšia ako 999.<br>Export do RISSAM nebude fungovať správne!",
                        Condition = new List<PfeFilterAttribute>
                        {
                            new PfeFilterAttribute
                            {
                                ComparisonOperator = "gt",
                                Value = 999
                            }
                        }
                    });
                }

                var pprgFld = model.Fields.FirstOrDefault(p => p.Name == nameof(Podprogram));
                if (pprgFld != null)
                {
                    pprgFld.Validator ??= new PfeValidator { Messages = new List<PfeMessage>() };
                    pprgFld.Validator.Messages = pprgFld.Validator.Messages ?? new List<PfeMessage>(); //Validátor už je vytvorený ale bez Messages
                    pprgFld.Validator.Messages.Add(new PfeMessage
                    {
                        MessageType = PfeMessageType.MessageWarning,
                        Message = "Hodnota podprogramu je vačšia ako 99.<br>Export do RISSAM nebude fungovať správne!",
                        Condition = new List<PfeFilterAttribute>
                        {
                            new PfeFilterAttribute
                            {
                                ComparisonOperator = "gt",
                                Value = 99
                            }
                        }
                    });
                }

                var prvFld = model.Fields.FirstOrDefault(p => p.Name == nameof(Prvok));
                if (prvFld != null)
                {
                    prvFld.Validator ??= new PfeValidator { Messages = new List<PfeMessage>() };
                    prvFld.Validator.Messages = prvFld.Validator.Messages ?? new List<PfeMessage>(); //Validátor už je vytvorený ale bez Messages
                    prvFld.Validator.Messages.Add(new PfeMessage
                    {
                        MessageType = PfeMessageType.MessageWarning,
                        Message = "Hodnota prvku je vačšia ako 99.<br>Export do RISSAM nebude fungovať správne!",
                        Condition = new List<PfeFilterAttribute>
                        {
                            new PfeFilterAttribute
                            {
                                ComparisonOperator = "gt",
                                Value = 99
                            }
                        }
                    });
                }
            }

        }
    }
}


