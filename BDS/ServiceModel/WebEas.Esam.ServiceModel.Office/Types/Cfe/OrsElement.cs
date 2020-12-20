using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_OrsElement")]
    public class OrsElement : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        public int C_OrsElement_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ oddelenia", NameField = "C_OrsElementType_Id")]
        [HierarchyNodeParameter]
        public int C_OrsElementType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Identifikátor", Hidden = true)]
        public int? IdValue { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string ListValue { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zmazané")]
        public bool Deleted { get; set; }

    }
}
