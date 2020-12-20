using System;

namespace WebEas.ServiceModel
{

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SqlValidationAttribute : Attribute
    {
        /// <summary>
        /// Vykona sql validaciu nad entitou (reference), pred operaciou (operation) s danou sql podmienkou (sqlWhereExpression). Ak bude pocet vysledkov > 0  zobrazi chyb. hlasku (errorMessage).
        /// </summary>
        /// <param name="operation">Typ operacie</param>
        /// <param name="sql">SELECT ... FROM ... WHERE ...</param>
        /// <param name="datumField">dátumové pole, ktoré sa použije do hlášky na zistenie MIN(DatumField) a MAX(DatumField). Nahrádza Min{DatumField} a Max{DatumField}</param>
        /// <param name="errorMessage">chybova hlaska na zobrazenie</param>
        public SqlValidationAttribute(Operation operation, string sql, string datumField, string errorMessage)
        {
            OperationType = operation;
            Sql = sql;
            DatumField = datumField;
            ErrorMessage = errorMessage;
        }

        public Operation OperationType { get; private set; }
        
        public string Sql { get; private set; }

        public string DatumField { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}