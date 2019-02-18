using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using WebEas.ServiceModel;
using WebEas.ServiceModel.Office.Egov.Types;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [Schema("cfe")]
    [Alias("D_Department")]
    [DataContract]
    public class DepartmentView : BaseTenantEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public int D_Department_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ")]
        public int Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Nadradenné oddelenie")]
        public int? C_Department_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zodpovedný")]
        public int? Zodpovedny_id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platné od", Hidden = true, Type = PfeDataType.DateTime, Editable = false, Rank = 103, ReadOnly = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platné do", Hidden = true, Type = PfeDataType.DateTime, Editable = false, Rank = 103, ReadOnly = true)]
        public DateTime? PlatnostDo { get; set; }
    }
}