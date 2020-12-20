using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("C_LoggingConfig")]
    [DataContract]
    [Dial(DialType.Hybrid, DialKindType.BackOffice)]
    public class LoggingConfig : BaseTenantEntityNullable
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int C_LoggingConfig_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Schéma", Width = 75)]
        public string Schema { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov tabuľky", Width = 160)]
        public string NazovTabulky { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov stĺpca", Width = 140)]
        public string NazovStlpca { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ stĺpca", Width = 120)]
        public string TypStlpca { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis zmeny", Width = 250, Xtype = PfeXType.RichTextRpt)]
        public string PopisZmeny { get; set; }
    }
}