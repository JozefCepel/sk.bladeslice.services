using System;
using System.Linq;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace WebEas.ServiceModel.Office.Egov.Reg.Types
{
    [Schema("reg")]
    [Alias("C_Formular")]
    [DataContract]
    [Dial(DialType.Global, DialKindType.BackOffice)]
    public class Formular : BaseEntity
    {
        [PrimaryKey]
        [DataMember]
        [PfeColumn(Hidden = true)]
        public int C_Formular_Id { get; set; }

        [DataMember]
        [PfeColumn(Hidden = true)]
        public long C_Jazyk_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Technický identifikátor")]
        public string TechnickyIdentifikator { get; set; }

        [DataMember]
        [PfeColumn(Text = "Vstupný formulár", Hidden = true, Hideable = false)]
        public bool VstupnyFormular { get; set; }

        [DataMember]
        [PfeColumn(Text = "Adresa", Hidden = true, Hideable = false)]
        public string Adresa { get; set; }

        [DataMember]
        [PfeColumn(Text = "eForm ID")]
        public string eForm_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "UPVS meno formulára")]
        public string UPVSFormName { get; set; }

        [DataMember]
        [PfeColumn(Text = "UPVS element formulára")]
        public string UPVSFormDocumentElement { get; set; }

        [DataMember]
        [PfeColumn(Text = "Položka stromu")]
        public string PolozkaStromu { get; set; }

        [DataMember]
        [PfeColumn(Text = "Namespace")]
        public string Namespace { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ")]
        public string Typ { get; set; }
    }
}