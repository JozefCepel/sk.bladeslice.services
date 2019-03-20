using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_OrsElementType")]
    public class OrsElementType : BaseEntity
    {
        [DataMember]
        [PfeColumn(Text = "ID typu oddelenia")]
        public int C_OrsElementType_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul")]
        public int C_Modul_Id { get; set; }

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
