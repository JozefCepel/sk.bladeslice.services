using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebEas
{
    public enum WebEasSqlKnownErrorType
    {
        UniqueKey = 2627,
        ForeignKey = 547,
        Unknown = -1,
        Timeout = -2
    }

    /// <summary>
    /// 
    /// </summary>
    public class WebEasSqlException : WebEasValidationException
    {
        //private static readonly Regex rg = new Regex("constraint  ['|\"](?<constraint>\\w+)['|\"]");
        private static readonly Regex rgConstr = new Regex("(?:constraint|index) ['|\"](?<constraint>\\w+)['|\"]");
        private static readonly Regex rgTbl = new Regex("(?<obj>object|index) ['|\"](?<data>.*)['|\"]");

        /// <summary>
        /// Initializes a new instance of the <see cref="WebEasSqlException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WebEasSqlException(string message, SqlException innerException, object data) : base(message,innerException)
        {
            this.ErrorCode = innerException.Number;
            this.SqlData = data;

            if (Enum.IsDefined(typeof(WebEasSqlKnownErrorType), this.ErrorCode))
            {
                this.ErrorType = (WebEasSqlKnownErrorType)Enum.ToObject(typeof(WebEasSqlKnownErrorType), this.ErrorCode);
            }
            else
            {
                this.ErrorType = WebEasSqlKnownErrorType.Unknown;
            }

            Match matchContr = rgConstr.Match(innerException.Message);
            if (matchContr.Success)
            {
                if (matchContr.Groups["constraint"].Success)
                {
                    this.Constraint = matchContr.Groups["constraint"].Value;
                }
            }

            string tbl = "";
            string link = "<a href=\"#CFE/cfe-admin-users-modul!3\" target=\"_blank\">reg - Registre</a>";

            Match matchTable = rgTbl.Match(innerException.Message);
            if (matchTable.Success)
            {
                tbl = matchTable.Value;
            }

            if (data != null && data is IValidateConstraint && this.Constraint != null)
            {
                this.MessageUser = ((IValidateConstraint)data).ChangeConstraintMessage(this.Constraint, this.ErrorCode, this.ErrorType);
            }
            else if (innerException.Message.Contains("has a block predicate that conflicts with this operation") &&
                     innerException.Message.Contains("Modify the operation to target only the rows that are allowed by the block predicate"))
            {
                if (tbl.Contains("C_Stredisko"))
                {
                    tbl = "Strediská";
                }
                else if (tbl.Contains("C_Pokladnica"))
                {
                    tbl = "Pokladnice";
                }
                else if (tbl.Contains("C_BankaUcet"))
                {
                    tbl = "Bankové účty";
                }
                else if (tbl.Contains("C_TypBiznisEntity_Kniha"))
                {
                    tbl = "Knihy dokladov";
                }
                else if (tbl.Contains("C_Druh"))
                {
                    tbl = "Druhy daní a poplatkov";
                    link = "<a href=\"#CFE/cfe-admin-users-modul!9\" target=\"_blank\">dap - Pohľadávky DaP</a>";
                }
                this.MessageUser = $"Prihlásený používateľ nemá pridelené právo 'Upravovať' na typ prvku ORŠ - '{tbl}' ! " +
                                   $"Kontaktujte svojho administrátora pre nastavenie práv v položke {link}. Záložka 'Typy a Prvky ORŠ'.";
            }

            if (string.IsNullOrEmpty(this.MessageUser))
            {
                switch (this.ErrorType)
                {
                    case WebEasSqlKnownErrorType.UniqueKey:
                        this.MessageUser = "Nie je možné vložiť duplicitný záznam!";
                        break;
                    case WebEasSqlKnownErrorType.ForeignKey:
                        this.MessageUser = "Neexistuje nadväzujúci záznam!";
                        break;
                    case WebEasSqlKnownErrorType.Timeout:
                        this.MessageUser = "Databáza je zaneprázdnená, skúste akciu zopakovať za 2 minúty.";
                        break;
                    default:
                        this.MessageUser = "Nastala chyba pri ukladaní záznamu";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the SQL data.
        /// </summary>
        /// <value>The SQL data.</value>
        public object SqlData { get; private set; }

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public WebEasSqlKnownErrorType ErrorType { get; private set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Gets or sets the constraint.
        /// </summary>
        /// <value>The constraint.</value>
        public string Constraint { get; private set; }
    }
}