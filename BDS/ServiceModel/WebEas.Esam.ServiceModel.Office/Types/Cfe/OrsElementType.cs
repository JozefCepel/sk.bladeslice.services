using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Cfe
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_OrsElementType")]
    public class OrsElementType : BaseEntity
    {
        [DataMember]
        [PrimaryKey]
        [PfeColumn(Text = "C_OrsElementType_Id")]
        public int C_OrsElementType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov")]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "DbSchema")]
        public string DbSchema { get; set; }

        [DataMember]
        [PfeColumn(Text = "DbView")]
        public string DbView { get; set; }

        [DataMember]
        [PfeColumn(Text = "DbIdField")]
        public string DbIdField { get; set; }

        [DataMember]
        [PfeColumn(Text = "DbListField")]
        public string DbListField { get; set; }

        [DataMember]
        [PfeColumn(Text = "DbDeletedField")]
        public string DbDeletedField { get; set; }

    }
}
