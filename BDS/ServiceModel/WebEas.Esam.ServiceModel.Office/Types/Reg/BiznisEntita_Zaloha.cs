using ServiceStack.DataAnnotations;
using System;
using System.Runtime.Serialization;
using WebEas.ServiceModel;

namespace WebEas.Esam.ServiceModel.Office.Types.Reg
{
    [DataContract]
    [Schema("reg")]
    [Alias("D_BiznisEntita_Zaloha")]
    public class BiznisEntita_Zaloha : BaseTenantEntity
    {
        [DataMember]
        [AutoIncrement]
        [PrimaryKey]
        [PfeColumn(Text = "_D_BiznisEntita_Zaloha_Id", Mandatory = true)]
        public long D_BiznisEntita_Zaloha_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id_FA")] // faktúra
        public long D_BiznisEntita_Id_FA { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_BiznisEntita_Id_ZF")] // zálohova faktúra
        public long? D_BiznisEntita_Id_ZF { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_DokladBANPol_Id")]
        public long? D_DokladBANPol_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "_D_UhradaParovanie_Id")]
        public long? D_UhradaParovanie_Id { get; set; }

        [DataMember]
        [PfeColumn(Text = "Typ zálohy", Mandatory = true)]
        public int C_Typ_Id { get; set; }

        // Pretazeny hore vo Viewe
        [DataMember]
        [PfeColumn(Text = "VS", Mandatory = true)]
        [StringLength(40)]
        public string VS { get; set; }

        [DataMember]
        [PfeColumn(Text = "Čiastka", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Cena { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Kurzový zisk/strata", Mandatory = true, DefaultValue = 0)]
        public decimal DM_Rozdiel { get; set; }

        [DataMember]
        [PfeColumn(Text = "_CM_Cena", Mandatory = true, DefaultValue = 0)]
        public decimal CM_Cena { get; set; }

        [DataMember]
        [PfeColumn(Text = "_Kurz", Mandatory = true, DefaultValue = 1)]
        public decimal Kurz { get; set; }

        [DataMember]
        [PfeColumn(Text = "Dátum úhrady zálohy", Type = PfeDataType.Date, Mandatory = true)]
        public DateTime DatumUhrady { get; set; }

        [DataMember]
        [PfeColumn(Text = "Popis")]
        [StringLength(210)]
        public string Popis { get; set; }

        [DataMember]
        [PfeColumn(Text = "Rok", ReadOnly = true)] // plnime ho pri Save/Insert
        public short Rok { get; set; }

    }
}
