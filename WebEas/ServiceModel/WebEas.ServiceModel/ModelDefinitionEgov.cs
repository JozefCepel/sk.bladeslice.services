using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace WebEas.ServiceModel
{
    /// <summary>
    /// Chyba v servicestack - override
    /// </summary>
    public class ModelDefinitionEgov : ModelDefinition
    {
        /// <summary>
        /// Gets the FieldName.
        /// </summary>
        /// <value>The FieldName for PFE defined column.</value>
        public string FieldName(string sPfeColumnName)
        {
            string s = sPfeColumnName;
            FieldDefinition fd = this.FieldDefinitions.Find(x => x.Name.Equals(sPfeColumnName));
            if (fd != null)
                s = fd.FieldName;
            return s;
        }

        /// <summary>
        /// Gets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        public new FieldDefinition PrimaryKey
        {
            get
            {
                return FieldDefinitions.FirstOrDefault<FieldDefinition>((FieldDefinition x) => x.IsPrimaryKey);
            }
        }

        /// <summary>
        /// Gets the has auto increment id.
        /// </summary>
        /// <value>The has auto increment id.</value>
        public new bool HasAutoIncrementId
        {
            get
            {
                if (PrimaryKey == null)
                {
                    return false;
                }
                return PrimaryKey.AutoIncrement;
            }
        }
    }
}
