using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_Department")]
    [DataContract]
    public class Department : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_Department_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ")]
        public short Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", Mandatory = true)]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Nadradenné oddelenie")]
        public int? C_Department_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Zodpovedný")]
        public Guid? Zodpovedny_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platné od", Mandatory = true, Type = PfeDataType.Date)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platné do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}