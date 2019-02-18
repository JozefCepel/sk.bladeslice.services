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
        private static readonly Regex rg = new Regex("(?:constraint|index) ['|\"](?<constraint>\\w+)['|\"]");

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

            Match match = rg.Match(innerException.Message);
            if (match.Success)
            {
                if (match.Groups["constraint"].Success)
                {
                    this.Constraint = match.Groups["constraint"].Value;
                }
            }

            if (data != null && data is IValidateConstraint)
            {
                this.MessageUser = ((IValidateConstraint)data).ChangeConstraintMessage(this.Constraint, this.ErrorCode, this.ErrorType);
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
                        this.MessageUser = "Nastala chyba pri ukladani záznamu";
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