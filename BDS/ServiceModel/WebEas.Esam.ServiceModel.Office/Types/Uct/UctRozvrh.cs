using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("C_UctRozvrh")]
    [SqlValidation(Operation.Update,
        "SELECT SUM(CASE WHEN DatumUctovania NOT BETWEEN @PlatnostOd AND ISNULL(@PlatnostDo, '2100-01-01') THEN 1 ELSE 0 END), MIN(DatumUctovania), MAX(DatumUctovania) " +
        "FROM uct.V_UctDennik " +
        "WHERE C_UctRozvrh_Id = @C_UctRozvrh_Id AND U = 1 AND D_Tenant_Id = @D_Tenant_Id",
        "DatumUctovania",
        "Nie je možné zmeniť požadovanú platnosť účtu! Na účte je účtované v rozmedzí < MinDatumUctovania; MaxDatumUctovania >")]
    [SqlValidation(Operation.Delete,
        "SELECT COUNT(1), MIN(DatumUctovania), MAX(DatumUctovania) " +
        "FROM uct.V_UctDennik " +
        "WHERE C_UctRozvrh_Id = @C_UctRozvrh_Id AND U = 1 AND D_Tenant_Id = @D_Tenant_Id",
        "DatumUctovania",
        "Účet nie je možné zmazať! Je použitý na zaúčtovaných dokladoch v rozmedzí < MinDatumUctovania; MaxDatumUctovania >. Na zneaktívnenie účtu využite platnosť záznamu od-do.")]
    public class UctRozvrh : BaseTenantEntityNullable, IValidateConstraint
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public long C_UctRozvrh_Id { get; set; }

        [DataMember]
        [PfeCombo(typeof(UctOsnovaView), ComboDisplayColumn = nameof(UctOsnovaView.SU), ComboIdColumn = nameof(UctOsnovaView.SU), 
            AdditionalWhereSql = "LEN(SU) = 3", 
            AdditionalFields = new string[] { nameof(UctOsnovaView.Nazov), nameof(UctOsnovaView.Typ), nameof(UctOsnovaView.Druh), nameof(UctOsnovaView.SDK) })]
        [PfeColumn(Text = "SÚ", Xtype = PfeXType.SearchFieldSS, Mandatory = true)]
        [StringLength(3)]
        public string SU { get; set; }

        [DataMember]
        [PfeColumn(Text = "AÚ", Mandatory = true)]
        [StringLength(50)]
        public string AU { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účet", ReadOnly = true)]
        [IgnoreInsertOrUpdate] //Custom riešenie eSAM
        [IgnoreOnUpdate] //Potrebné pre import z KORWIN-u, ktorý ide mimo validačných kontrol priamo cez ServiceStack DB.Update()
        public string Ucet { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public string Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Druh")]
        public string Druh { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Saldokontný")]
        public string SDK { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CasoveRozlisenie")]
        public string CasoveRozlisenie { get; set; }

        [DataMember]
        public string Old_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Anal. účet (import z pôv. účt.)")]
        [StringLength(50)]
        public string Old_AU { get; set; }

        [DataMember]
        [PfeColumn(Text = "Stredisko")]
        public bool VyzadovatStredisko { get; set; }

        [DataMember]
        [PfeColumn(Text = "Projekt")]
        public bool VyzadovatProjekt { get; set; }

        [DataMember]
        [PfeColumn(Text = "VyzadovatUctKluc1")]
        public bool VyzadovatUctKluc1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "VyzadovatUctKluc2")]
        public bool VyzadovatUctKluc2 { get; set; }

        [DataMember]
        [PfeColumn(Text = "VyzadovatUctKluc3")]
        public bool VyzadovatUctKluc3 { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [Alias("PlatnostDo")]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }

        public string ChangeConstraintMessage(string constraintName, int errorCode, WebEasSqlKnownErrorType errorType)
        {
            if (constraintName.StartsWith("CK_C_UctRozvrh_AU"))
            {
                return "Hodnota 'AÚ' nesmie obsahovať znaky \" \"; \".\"; \" - \"; \" / \". Znaky je možné nadefinovať pomocou parametra <a href=\"/#UCT/all-sm-mset!uct\" target =\"_blank\">'FormatUctuVRozvrhu'</a>.";
            }

            return null;
        }
    }
}
