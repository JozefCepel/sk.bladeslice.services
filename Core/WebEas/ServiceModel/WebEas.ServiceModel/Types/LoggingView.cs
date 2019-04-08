using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Types
{
    [Schema("reg")]
    [Alias("V_Logging")]
    [DataContract]
    public class LoggingView : ITenantEntityNullable
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        [PfeColumn(Text = "Id", Hidden = true)]
        public long D_Logging_Id { get; set; }

        [IgnoreDataMember]
        public Guid? D_Tenant_Id { get; set; }

        [IgnoreDataMember]
        public long Row_Id { get; set; }

        [IgnoreDataMember]
        public string Schema { get; set; }

        [IgnoreDataMember]
        public string NazovTabulky { get; set; }

        [DataMember]
        [PfeSort]
        [PfeColumn(Text = "Name stĺpca", Editable = false, ReadOnly = true)]
        public string NazovStlpca { get; set; }

        [DataMember]
        [PfeSort]
        [PfeColumn(Text = "Typ stĺpca")]
        public string TypStlpca { get; set; }

        [DataMember]
        [PfeColumn(Text = "Pôvodná hodnota", Editable = false, ReadOnly = true)]
        public string PovodnaHodnota { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nová hodnota", Editable = false, ReadOnly = true)]
        public string NovaHodnota { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis zmeny", Width = 250, Xtype = PfeXType.RichTextRpt, Editable = false, ReadOnly = true)]
        public string PopisZmeny { get; set; }

        [DataMember]
        [PfeSort(Sort = PfeOrder.Desc)]
        [PfeColumn(Text = "Dátum zmeny", Type = PfeDataType.DateTime, Editable = false, ReadOnly = true)]
        public DateTime DatumVytvorenia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmenil", Editable = false, ReadOnly = true)]
        public string VytvorilMeno { get; set; }
    }
}