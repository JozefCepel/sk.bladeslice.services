using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("D_Program")]
    [DataContract]
    public class Program : BaseTenantEntity
    {
        [DataMember]
        [PrimaryKey]
        [AutoIncrement]
        public long D_Program_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_PRTyp", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public byte PRTyp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ", ReadOnly = true, Editable = false)]
        [PfeCombo(typeof(ProgramTypCombo), NameColumn = "PRTyp")]
        [IgnoreInsertOrUpdate]
        public string PRTypMeno { get; set; }

        [DataMember]
        [PfeColumn(Text = "Program")]
        [PfeSort(Rank = 1, Sort = PfeOrder.Asc)]
        public short? program { get; set; }

        [DataMember]
        [PfeColumn(Text = "Podprogram")]
        [PfeSort(Rank = 2, Sort = PfeOrder.Asc)]
        public short? podprogram { get; set; }

        [DataMember]
        [PfeColumn(Text = "Prvok")]
        [PfeSort(Rank = 3, Sort = PfeOrder.Asc)]
        public short? prvok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Kód", ReadOnly = true)]
        [IgnoreInsertOrUpdate]
        public string PRKod { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string PRNazov { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW, Text = "Zámer")]
        public string PRZamer { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW, Text = "Popis")]
        public string PRPopis { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp1")]
        public Guid? D_User_Id_Zodp1 { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp2")]
        public Guid? D_User_Id_Zodp2 { get; set; }

        [DataMember]
        [PfeColumn(Xtype = PfeXType.TextareaWW, Text = "Komentár")]
        public string Komentar { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť od", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime PlatnostOd { get; set; }

        [DataMember]
        [PfeColumn(Text = "Platnosť do", Type = PfeDataType.Date)]
        public DateTime? PlatnostDo { get; set; }
    }
}


