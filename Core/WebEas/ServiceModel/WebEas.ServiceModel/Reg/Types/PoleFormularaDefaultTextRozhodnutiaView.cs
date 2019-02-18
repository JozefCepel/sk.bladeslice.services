using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("V_PoleFormulara_DefaultTextRozhodnutia")]
    [DataContract]
    public class PoleFormularaDefaultTextRozhodnutiaView
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long C_PoleFormulara_Id { get; set; }

        [DataMember]
        [PfeValueColumn]
        public string PoleFormularaNazov { get; set; }

        [DataMember]
        public int C_DefaultTextRozhodnutia_Id { get; set; }
    }

    [Schema("reg")]
    [Alias("V_PoleFormulara_DefaultTextRozhodnutia")]
    [DataContract]
    public class DefaultTextRozhodnutiaPoleFormularaView
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public int C_DefaultTextRozhodnutia_Id { get; set; }

        [DataMember]
        [PfeValueColumn]
        public string Nazov { get; set; }

        [DataMember]
        public long C_PoleFormulara_Id { get; set; }
    }
}