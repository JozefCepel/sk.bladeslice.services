using System;
using System.Linq;

namespace WebEas
{
    public interface IValidateConstraint
    {
        /// <summary>
        /// Zmena default textu chyby ak vyskoci SQL Exception pri insert alebo update (null ak sa ma nastavit default text)
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errorType">Type of the error.</param>
        /// <returns></returns>
        string ChangeConstraintMessage(string constraintName, int errorCode, WebEasSqlKnownErrorType errorType);
    }
}