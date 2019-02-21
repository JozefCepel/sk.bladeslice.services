using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Bds.Types
{
    [Schema("rzp")]
    [Alias("D_NavrhZmenyBds")]
    [DataContract]
    public class NavrhZmenyBds : BaseTenantEntity, IHasStateId
    {
        [PrimaryKey]
        [AutoIncrement]
        [DataMember]
        public long D_NavrhZmenyBds_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Typ", Description = "Návrh/Zmena(y)")]
        public bool Typ { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", Mandatory = true)]
        public int Rok { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime Datum { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_User_Id_Zodp", Mandatory = true)]
        public Guid D_User_Id_Zodp { get; set; }

        [DataMember]
        [PfeColumn(Text = "Uznesenie")]
        public string Uznesenie { get; set; }

        [DataMember]
        [PfeColumn(Text = "Názov", Mandatory = true)]
        public string Nazov { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis", Xtype = PfeXType.TextareaWW)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum schválenia", Type = PfeDataType.Date, ReadOnly = true)]
        public DateTime? DatumSchvalenia { get; set; }

        [DataMember]
        [PfeColumn(Text = "Zrušenie schválenia", Type = PfeDataType.Date, ReadOnly = true)]
        public DateTime? DatumZrusSchval { get; set; }

        [DataMember]
        [PfeColumn(Text = "_C_StavEntity_Id")]
        public int C_StavEntity_Id { get; set; }
    }
}
