using System;
using System.Linq;
using System.Runtime.Serialization;

namespace WebEas.ServiceModel
{
    public interface ITenantEntityNullable
    {
        /// <summary>
        /// Gets or sets the d_ tenant_ id.
        /// </summary>
        /// <value>The d_ tenant_ id.</value>
        [IgnoreDataMember]
        Guid? D_Tenant_Id { get; set; }
    }

    /// <summary>
    /// Atribut sa pouziva iba ak trieda implementuje ITenantEntityNullable. 
    /// Umoznuje vytvorenie selectu, ktory odfiltruje 'globalne' zaznamy (bez tenanta) ak existuju zaznamy s tenantom.
    /// Vyzaduje aby sa zaznamy dali identifikovat 'unique' stlpcami (nie PK..)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PreferTenantAttribute : Attribute
    {
        private string[] keylist;

        public PreferTenantAttribute(string columns)
        {
            this.keylist = columns.Split(',');
        }

        /// <summary>
        /// Produces something like:
        /// join (select FK1 as F1, FK2 as F2, COUNT(*) as Num from schema.table where D_Tenant_Id IS NULL or D_Tenant_Id = 'tenant' Group by FK1, FK2) x 
        /// on schema.table.FK1 = x.F1 and schema.table.FK2 = x.F2 and (t.Num = 1 Or schema.table.D_Tenant_Id is not null)
        /// </summary>
        public void WriteCommand(ServiceStack.OrmLite.IOrmLiteDialectProvider dialect, System.Text.StringBuilder sb, string schema, string table, string tenant)
        {
            //prepare quoted columns here too
            string[] quotedlist = new string[keylist.Length];
            sb.Append(" JOIN (select ");
            for (int i = 0; i < quotedlist.Length; i++)
            {
                string col = dialect.GetQuotedColumnName(keylist[i].Trim());
                sb.AppendFormat("{0} as PT_FK{1},", col, i + 1);
                quotedlist[i] = col;
            }
            sb.AppendFormat(" COUNT(*) as PT_NUM from {0}.{1} where D_Tenant_Id IS NULL or D_Tenant_Id = '{2}' Group by ", schema, table, tenant);
            for (int i = 0; i < quotedlist.Length; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(quotedlist[i]);
            }
            sb.Append(") u ON ");
            for (int i = 0; i < quotedlist.Length; i++)
            {
                sb.AppendFormat("{0}.{1}.{2} = u.PT_FK{3} AND ", schema, table, quotedlist[i], i + 1);
            }
            sb.AppendFormat("(u.PT_NUM = 1 OR {0}.{1}.D_Tenant_Id IS NOT NULL) ", schema, table);
        }
    }
}