using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Uct
{
    [DataContract]
    [Schema("uct")]
    [Alias("C_TransferCis")]
    public class TransferCis : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_TransferCis_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        [StringLength(255)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        [StringLength(512)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctRozvrh_Id_MD")]
        public long? C_UctRozvrh_Id_MD { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_UctRozvrh_Id_Dal")]
        public long? C_UctRozvrh_Id_Dal { get; set; }

        [DataMember]
        [PfeColumn(Text = "Účtovať s výdavkom", Mandatory = true)]
        public bool UctovatSVydavkom { get; set; }

        [DataMember]
        [PfeColumn(Text = "BAN", Mandatory = true)]
        public bool BAN { get; set; }

        [DataMember]
        [PfeColumn(Text = "PDK-V", Mandatory = true)]
        public bool PDK { get; set; }

        [DataMember]
        [PfeColumn(Text = "DFA", Mandatory = true)]
        public bool DFA { get; set; }

        [DataMember]
        [PfeColumn(Text = "IND", Mandatory = true)]
        public bool IND { get; set; }

        [DataMember]
        [PfeColumn(Text = "MAJ", Mandatory = true)]
        public bool MAJ { get; set; }

        [DataMember]
        [PfeColumn(Text = "SKL", Mandatory = true)]
        public bool SKL { get; set; }

        [DataMember]
        [PfeColumn(Text = "MZD", Mandatory = true)]
        public bool MZD { get; set; }
    }
}
