using ServiceStack.DataAnnotations;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Vyk
{
    [DataContract]
    [Schema("vyk")]
    [Alias("C_VykazDruh")]
    public class VykazDruhHelper : BaseTenantEntityNullable
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public int C_VykazDruh_Id { get; set; }

        [DataMember]
        public int? C_VykazDruh_Id_Parent { get; set; }

        [DataMember]
        public string Kod { get; set; }

        [DataMember]
        public string Nazov { get; set; }

        [DataMember]
        public string Popis { get; set; }

        [DataMember]
        public string Csv { get; set; }

        [DataMember]
        public string UctyEliminacie { get; set; }

        [DataMember]
        public short RokOd { get; set; }

        [DataMember]
        public short? RokDo { get; set; }

        [DataMember]
        public int Poradie { get; set; }

        [DataMember]
        public short PocetRiadkov { get; set; }

        [DataMember]
        public short ZdrojTyp { get; set; }

        [DataMember]
        public byte DialogTyp { get; set; }

        [DataMember]
        public short DefiniciaTyp { get; set; }

        [DataMember]
        public bool Zobrazovat { get; set; }
    }
}
