using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Cfe.Types
{
    [DataContract]
    [Schema("cfe")]
    [Alias("C_Tree")]
    public class Tree : BaseEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "ID stromu")]
        public int C_Tree_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_TreeParent")]
        public int? C_Tree_Id_Parent { get; set; }

        [DataMember]
        [PfeColumn(Text = "Modul")]
        public int C_Modul_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Code")]
        public string Kod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Name")]
        public string Nazov { get; set; }

    }
}
